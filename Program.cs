using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Set the JSON serializer options globally
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
});


// Add services to the container.
builder.Services.AddControllersWithViews(); //comentario

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

builder.Services.AddScoped<ServicioDashboard>(); //Agregando el servicio nuevo creado Dashboard

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