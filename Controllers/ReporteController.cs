using Microsoft.AspNetCore.Mvc;
using Naitv1.Services;
using Naitv1.Helpers;
using Naitv1.Data.Repositories;
using Naitv1.Data;
using Naitv1.Models;
using Naitv1.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace Naitv1.Controllers
{
    public class ReporteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GeneradorReportesService _reporteService;
        private readonly pdfServices _pdfServices;
        private readonly IEmailServices _emailServices;
        private readonly IActividadRepository _actividadRepository;
        private readonly ConfiguracionReporteService _configuracionReporteService;
        public ReporteController(AppDbContext context,GeneradorReportesService reporteService, pdfServices pdfServices, IEmailServices emailServices, IActividadRepository actividadRepository, ConfiguracionReporteService configService)
        {
            _context = context;
            _reporteService = reporteService;
            _pdfServices = pdfServices;
            _emailServices = emailServices;
            _actividadRepository = actividadRepository;
            _configuracionReporteService = configService;
        }

        public IActionResult Imprimir()
        {
            string html = _reporteService.GenerarHtmlConReporte();
            byte[] pdfBytes = _pdfServices.GeneradorPdfHTML(html);


            return File(pdfBytes, "application/pdf", "reporte.pdf");
        }

        public IActionResult VerGrafico()
        {
            int actividades = _context.Actividades.Count();
            int usuarios = _context.Usuarios.Count();
            ViewBag.Actividad = actividades;
            ViewBag.Usuarios = usuarios;
            

            if (UsuarioLogueado.esSuperAdmin(HttpContext.Session) == false)
            {
                return RedirectToAction("RestriccionAcceso", "Reporte");
            }
            return View(); 
        }

        [HttpPost]
        public IActionResult SubirGraficoBase64([FromBody] ImagenDto data)
        {
            var base64Data = data.Base64.Split(',')[1];

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "graficos", "graficoKpirs.png");
            System.IO.File.WriteAllBytes(path, Convert.FromBase64String(base64Data));

            return Ok();
        }

        public IActionResult EnviarReporte()
        {
            var html = _reporteService.GenerarHtmlConReporte();
            _emailServices.Enviar("founder@empresa.com", "Reporte Semanal", html);

            return Ok("Correo enviado.");
        }

        public IActionResult RestriccionAcceso()
        {
            return View();
        }

        public IActionResult FormReport()
        {
            if (UsuarioLogueado.esSuperAdmin(HttpContext.Session) == false)
            {
                return RedirectToAction("RestriccionAcceso", "Reporte");
            }
            return View();
        }

        public IActionResult conteoActividad()
        {
            int cantidad = _actividadRepository.ContarActividades();
            ViewBag.Cantidad = cantidad;
            return View();

        }

        [HttpPost]
        public IActionResult CrearCrearRegistroEmailManual(string destinatario, string asunto, DateTime fechaProgramada)
        {
            _reporteService.CrearRegistro(fechaProgramada, destinatario, asunto);
            TempData["Mensaje"] = "Mensaje programado correctamente.";
            return RedirectToAction("Index", "Home");
        }

		[HttpGet]
		public IActionResult FormularioCsvCiudad()
		{
			ViewBag.CiudadId = new SelectList(_context.Ciudades, "Id", "Nombre");
			return View();
		}

		[HttpPost]
		public IActionResult FormularioCsvCiudad(Actividad model)
		{
            if (UsuarioLogueado.esSuperAdmin(HttpContext.Session) == false)
            {
                return RedirectToAction("RestriccionAcceso", "Reporte");
            }
            ViewBag.CiudadId = new SelectList(_context.Ciudades, "Id", "Nombre");

			int ciudadId = model.CiudadId;
			DateTime fechaInicio = model.FechCreación;
			DateTime? fechaFinal = model.FechaFinal;

			if (!fechaFinal.HasValue)
			{
				ModelState.AddModelError("FechaFinal", "Debe ingresar una fecha final.");
				return View(model);
			}

			var actividades = _context.Actividades
				.Where(a => a.CiudadId == ciudadId &&
							a.FechCreación >= fechaInicio &&
							a.FechCreación <= fechaFinal.Value)
				.ToList();

            var resumen = new
            {
                CiudadId = ciudadId,
                FechaInicio = fechaInicio.ToString("yyyy-MM-dd"),
                FechaFinal = fechaFinal.Value.ToString("yyyy-MM-dd"),
                CantidadActividades = _context.Actividades.Count(a =>
                    a.CiudadId == ciudadId &&
                    a.FechCreación >= fechaInicio &&
                    a.FechCreación <= fechaFinal.Value)
            };


            var csvBytes = CsvHelper.ConvertirAFormatoCSV(new[] { resumen }); // Ya es byte[]
			return File(csvBytes, "text/csv", "actividades_filtradas.csv");
		}


        public IActionResult EditConfigReporte()
        {
            ///va a mostrar la ultima fecha que se programo
            var config = _configuracionReporteService.Obtener();
            return View(config);
        }

        [HttpPost]
        public IActionResult EditConfigReporte(ConfiguracionReporte config)
        {
           
            Console.WriteLine("Post recibido: " + JsonSerializer.Serialize(config));
               

            if (ModelState.IsValid)
            {
                Console.WriteLine("Los datos ingresados no son validos");
            }

            _configuracionReporteService.Guardar(config);
            return RedirectToAction("EditConfigReporte");

        }


    }

}


