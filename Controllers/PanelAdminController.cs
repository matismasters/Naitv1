using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Models;
using Naitv1.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Naitv1.Controllers
{
    public class PanelAdminController : Controller
    {        
        private readonly AppDbContext _context;
        private readonly ServicioDashboard _servicioDashboard;

        public PanelAdminController(AppDbContext context, ServicioDashboard servicioDashboard)
        {            
            _context = context;
            _servicioDashboard = servicioDashboard;
        }

        /*[Authorize(Roles = "Admin")]*/
        public async Task<ActionResult> Index()
        {

            /*var actividadesPorHora = await _servicioDashboard.ObtenerMetrics();
            ViewBag.ActividadesPorHora = actividadesPorHora;*/

            FiltroDashboard filtro = new FiltroDashboard();

            // Para testear el metodo a ver si anda con filtros
            /*FiltroDashboard filtro = new FiltroDashboard
            {
                FechaInicio = new DateTime(2025, 5, 26),
                FechaFin = DateTime.Now,
                CiudadId = 10 // Montevideo
            };*/

            var datos = await _servicioDashboard.ObtenerMetrics(filtro); //le paso un filtro vacio
            List<Ciudad> ciudades = await _context.Ciudades.ToListAsync();

            ViewBag.Ciudades = ciudades;
            ViewBag.ActividadesPorHora = datos.ActividadesPorHora;
            ViewBag.ActividadesPorCiudad = datos.ActividadesPorCiudad;

            return View();
        }

        public async Task<IActionResult> Filtrar([FromBody] FiltroDashboard filtro)
        {

            Console.WriteLine("=== Filtro recibido ===");
            Console.WriteLine($"Inicio: {filtro.FechaInicio}, Fin: {filtro.FechaFin}, CiudadId: {filtro.CiudadId}");

            if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue && (filtro.FechaFin - filtro.FechaInicio)?.TotalDays > 90)
            {
                return BadRequest(new { error = "El rango maximo es de 90 dias" });
            }

            var datos = await _servicioDashboard.ObtenerMetrics(filtro);

            if (datos.ActividadesPorHora == null && datos.ActividadesPorCiudad == null)
            {
                return Ok(new { mensaje = "No hay datos para mostrar con este filtro" });
            }

            // asi conservo los datos de los filtros cuando se refresquen las graficas y mapa
            //ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            //ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");

            return Json(datos);
        }

    }
}
