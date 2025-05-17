using Naitv1.Data;
using Naitv1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naitv1.Services
{
    public class GeneradorReportesService
    {
        private readonly AppDbContext _context;

        public GeneradorReportesService(AppDbContext context)
        {
            _context = context;
        }

        public void CrearRegistro(DateTime fechaProgramada, string desitinatario, string asunto)
        {
            string html = GenerarHtmlConReporte();

            var registro = new RegistroEmail
            {
                Destinatario = desitinatario,
                Asunto = asunto,
                CuerpoHtml = html,
                FechaProgramada = fechaProgramada,
            };

            _context.RegistroEmails.Add(registro);
            _context.SaveChanges();
        }

        public string GenerarHtmlConReporte()
        {
            string rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "graficos", "graficoKpis.png");

            // Leemos la imagen como base64
            byte[] imagenBytes = File.ReadAllBytes(rutaImagen);
            string base64Imagen = Convert.ToBase64String(imagenBytes);

            return $@"
            <html>
            <head><meta charset='utf-8'></head>
            <body>
                <h2>Reporte Semanal</h2>
                <ul>
                    <li><b>Usuarios nuevos:</b> 124</li>
                    <li><b>Ventas:</b> $3,200</li>
                    <li><b>Actividades creadas:</b> 57</li>
                </ul>
                <h3>Gráfico:</h3>
                <img src='data:image/png;base64,{base64Imagen}' width='400' height='200' />
            </body>
            </html>";
        }

    }
}