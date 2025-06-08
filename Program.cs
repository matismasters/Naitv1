using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Establecer cultura por defecto a en-US (usa punto como separador decimal)
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

builder.Services.AddScoped<ServicioDashboard>(); //Agregando el servicio nuevo creado Dashboard
builder.Services.AddScoped<ServicioCiudad>(); //Agregado el servicio para poder obtener la ciudad con la lat y long
builder.Services.AddScoped<ServicioExportadorCsv>(); //Agregado servicio generador de Csv

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
