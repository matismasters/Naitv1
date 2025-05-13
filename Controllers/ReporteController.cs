using Microsoft.AspNetCore.Mvc;
using Naitv1.Services;

namespace Naitv1.Controllers
{
    public class ReporteController : Controller
    {
        private readonly GeneradorReportesService _reporteService;
        private readonly pdfServices _pdfServices;

        public ReporteController(GeneradorReportesService reporteService, pdfServices pdfServices)
        {
            _reporteService = reporteService;
            _pdfServices = pdfServices;
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
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "graficos", "graficoKpis.png");
            System.IO.File.WriteAllBytes(path, Convert.FromBase64String(base64Data));

            return Ok();
        }

        public class ImagenDto
        {
            public string Base64 { get; set; }
        }

    }

}


