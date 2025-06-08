using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Models;
using Naitv1.Services;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Naitv1.Controllers
{
    public class PanelAdminController : Controller
    {        
        private readonly AppDbContext _context;
        private readonly ServicioDashboard _servicioDashboard;
        private readonly ServicioExportadorCsv _servicioExportadorCsv;

        public PanelAdminController(AppDbContext context, ServicioDashboard servicioDashboard, ServicioExportadorCsv servicioExportadorCsv)
        {            
            _context = context;
            _servicioDashboard = servicioDashboard;
            _servicioExportadorCsv = servicioExportadorCsv;
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

        public async Task<IActionResult> Filtrar([FromBody] FiltroDashboard filtro)
        {            

            if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue && (filtro.FechaFin - filtro.FechaInicio)?.TotalDays > 90)
            {
                return BadRequest(new { error = "El rango maximo es de 90 dias" });
            }

            var datos = await _servicioDashboard.ObtenerMetrics(filtro);

            if (!datos.ActividadesPorHora.Any(kvp => kvp.Value > 0) && !datos.ActividadesPorCiudad.Any(kvp => kvp.Value > 0))
            {
                return Ok(new { mensaje = "No hay datos para mostrar con este filtro" });
            }            

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

        [HttpGet]
        public async Task<IActionResult> ExportarCsv(int? ciudadId, DateTime? fechaInicio, DateTime? fechaFin)
        {            

            FiltroDashboard filtro = new FiltroDashboard
            {
                CiudadId = ciudadId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            string csvString = await _servicioExportadorCsv.GenerarCsv(filtro);

            byte[] csvBytes = Encoding.UTF8.GetBytes(csvString);

            FileContentResult archivoCsv = File(csvBytes, "text/csv", "dashboard.csv");

            return archivoCsv;
        }


    }
}
