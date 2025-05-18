using Microsoft.AspNetCore.Mvc;
using Naitv1.Services;
using Naitv1.Helpers;
using Naitv1.Data.Repositories;

namespace Naitv1.Controllers
{
    public class ReporteController : Controller
    {
        private readonly GeneradorReportesService _reporteService;
        private readonly pdfServices _pdfServices;
        private readonly IEmailServices _emailServices;
        private readonly IActividadRepository _actividadRepository;
        public ReporteController(GeneradorReportesService reporteService, pdfServices pdfServices, IEmailServices emailServices, IActividadRepository actividadRepository)
        {
            _reporteService = reporteService;
            _pdfServices = pdfServices;
            _emailServices = emailServices;
            _actividadRepository = actividadRepository;
        }

        public IActionResult Imprimir()
        {
            string html = _reporteService.GenerarHtmlConReporte();
            byte[] pdfBytes = _pdfServices.GeneradorPdfHTML(html);


            return File(pdfBytes, "application/pdf", "reporte.pdf");
        }

        public IActionResult VerGrafico()
        {
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
        public IActionResult CrearCrearRegistroEmailManual(string destinatario, string asunto)
        {
            DateTime fechaProgramada = DateTime.Now;

            _reporteService.CrearRegistro(fechaProgramada, destinatario, asunto);
            TempData["Mensaje"] = "Mensaje programado correctamente.";
            return RedirectToAction("Index", "Home");
        }



        public class ImagenDto
        {
            public string Base64 { get; set; }
        }

    }

}


