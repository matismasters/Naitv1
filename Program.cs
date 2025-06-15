using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using Naitv1.wkhtmltox;
using Quartz;
using Quartz.Impl;
using Naitv1.Jobs;
using Naitv1.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar el DbContext para que se inyecte en tus servicios.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios como GeneradorReportesService
builder.Services.AddScoped<GeneradorReportesService>();

// Registrar el job ReporteSemanalJob
builder.Services.AddScoped<IJob, ReporteSemanalJob>();

// Registrar DinkToPdf para la conversión a PDF
var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltox", "libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<pdfServices>();

// Registrar el servicio de Email
builder.Services.AddScoped<IEmailServices, SmtpEmailService>();

// Agregar soporte para sesiones
builder.Services.AddSession();

builder.Services.AddScoped<IActividadRepository, ActividadRepository>();


// Configurar Quartz.NET
builder.Services.AddQuartz(q =>
{
    // Registrar un JobFactory personalizado para permitir la inyección de dependencias en los jobs
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Programar el trabajo ReporteSemanalJob
    var jobKey = new JobKey("ReporteSemanalJob");
    q.AddJob<ReporteSemanalJob>(opts => opts.WithIdentity(jobKey));

    // Configurar el Trigger para ejecutarlo todos los lunes a las 9 AM
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ReporteSemanalTrigger")
        .WithCronSchedule("0 0 9 ? * MON") // Cron expression para lunes a las 9 AM
        .StartAt(DateTimeOffset.Now));
});

// Registrar servicios como GeneradorReportesService
builder.Services.AddScoped<GeneradorReportesService>();

// 🔧 Registrar ConfiguracionReporteService para el job
builder.Services.AddScoped<ConfiguracionReporteService>();

// Registrar el job ReporteSemanalJob
builder.Services.AddScoped<IJob, ReporteSemanalJob>();

// Configurar el scheduler de Quartz.NET
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Configurar las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
