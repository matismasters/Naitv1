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

            FiltroDashboard filtro = new FiltroDashboard();            

            var datos = await _servicioDashboard.ObtenerMetrics(filtro); //le paso un filtro vacio
            List<Ciudad> ciudades = await _context.Ciudades.ToListAsync();

            ViewBag.Ciudades = ciudades;
            ViewBag.ActividadesPorHora = datos.ActividadesPorHora;
            ViewBag.ActividadesPorCiudad = datos.ActividadesPorCiudad;

            return View();
        }

        public async Task<IActionResult> ObtenerConteoHoy()
        {
            int actividadesCreadasHoy = await _context.Actividades
                .Where(a => a.FechCreado.Date == DateTime.Today)
                .CountAsync();

            return Json(actividadesCreadasHoy);
        }

        public async Task<IActionResult> ActividadesActivas()
        {
            int cantidadActivas = await _context.Actividades
                .Where(a => a.Activa == true)
                .CountAsync();

            return Json(cantidadActivas);                
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

        [HttpPost]
        public async Task<IActionResult> ActividadesFiltradas([FromBody] FiltroDashboard filtro)
        {
            //DateTime desde = filtro?.FechaInicio ?? DateTime.Now; // Si vienen vacios tomo valores por defecto
            //DateTime hasta = (filtro?.FechaFin?.Date.AddDays(1).AddMilliseconds(-1)) ?? DateTime.Now;

            if (filtro == null)
            {
                filtro = new FiltroDashboard();
            }

            // Rango de fechas por defecto si no vienen
            DateTime desde = filtro.FechaInicio ?? DateTime.Now.AddDays(-7);
            DateTime hasta = filtro.FechaFin?.Date.AddDays(1).AddMilliseconds(-1) ?? DateTime.Now;

            // Empiezo desde IQueryable para construir la consulta progresivamente, gracias a esto entendí el uso de IQueryable
            IQueryable<Actividad> consulta = _context.Actividades
                .Include(a => a.Ciudad)
                .Include(a => a.Anfitrion);

            // Si no hay ningún filtro, muestro solo las actividades activas
            /*if (!filtro.FechaInicio.HasValue && !filtro.FechaFin.HasValue && !filtro.CiudadId.HasValue)
            {
                consulta = consulta.Where(a => a.Activa == true);
            }*/

            if (!filtro.FechaInicio.HasValue && !filtro.FechaFin.HasValue && !filtro.CiudadId.HasValue)
            {
                consulta = consulta.Where(a => a.Activa == true);
            }

            // Agrego filtro por fechas si vienen
            if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue)
            {
                consulta = consulta.Where(a => a.FechCreado >= desde && a.FechCreado <= hasta);
            }

            // Agrego filtro por ciudad si viene
            if (filtro.CiudadId.HasValue)
            {
                consulta = consulta.Where(a => a.CiudadId == filtro.CiudadId.Value);
            }

            List<Actividad> actividades = await consulta.ToListAsync();

            return Json(actividades);
        }


    }
}
