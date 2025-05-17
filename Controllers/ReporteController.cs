using Microsoft.AspNetCore.Mvc;
using Naitv1.Services;

namespace Naitv1.Controllers
{
    public class ReporteController : Controller
    {
        private readonly GeneradorReportesService _reporteService;
        private readonly pdfServices _pdfServices;
        private readonly IEmailServices _emailServices;
        public ReporteController(GeneradorReportesService reporteService, pdfServices pdfServices, IEmailServices emailServices)
        {
            _reporteService = reporteService;
            _pdfServices = pdfServices;
            _emailServices = emailServices;
        }

        public IActionResult Ver()
        {
            string html = _reporteService.GenerarHtmlConReporte();
            byte[] pdfBytes = _pdfServices.GeneradorPdfHTML(html);


            return File(pdfBytes, "application/pdf", "reporte.pdf");
        }

        public IActionResult VerGrafico()
        {
            return View(); // Busca Views/Reporte/VerGrafico.cshtml
        }

        [HttpPost]
        public IActionResult SubirGraficoBase64([FromBody] ImagenDto data)
        {
            // Elimina el encabezado "data:image/png;base64,"
            var base64Data = data.Base64.Split(',')[1];

            // Guardamos el archivo en wwwroot/graficos
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

        public class ImagenDto
        {
            public string Base64 { get; set; }
        }

    }

}


