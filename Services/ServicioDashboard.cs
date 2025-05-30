using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Naitv1.Controllers;
using Naitv1.Data;
using Naitv1.Models;

namespace Naitv1.Services
{    
    public class ServicioDashboard
    {
        private readonly AppDbContext _context;

        public ServicioDashboard(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MetricasDashboard> ObtenerMetrics() // Devuelvo un objeto del tripo MetricasDashboard que es un dto, que me sirve para mandar varios datos 
        {

            MetricasDashboard? datos = new MetricasDashboard

            {
                ActividadesPorHora = await ActividadesPorHora(),
                ActividadesPorCiudad = await ActividadesPorCiudad(),

            };                        

            return datos;
            
        }

        private async Task<Dictionary<string, int>> ActividadesPorCiudad()
        {
            DateTime desde = DateTime.Now.AddDays(-7);

            List<Actividad> actividades = await _context.Actividades
                .Include(a => a.Ciudad)
                .Where(a => a.FechCreado >= desde)
                .ToListAsync();

            Dictionary<string, int> resultado = actividades
                .GroupBy(a => a.Ciudad.Nombre)
                .ToDictionary(g => g.Key, g => g.Count());

            return resultado;
        }

        private async Task<Dictionary<int, int>> ActividadesPorHora()
        {

            DateTime desde = DateTime.Now.AddHours(-24);

            List<Actividad> actividades = await _context.Actividades
                .Where(a => a.FechCreado >= desde)
                .ToListAsync();

            Dictionary<int, int> resultado = actividades
                .GroupBy(a => a.FechCreado.Hour)
                .ToDictionary(g => g.Key, g => g.Count());

            // Asegura que existan todas las horas del 0 al 23, sino queda vacio y no se muestra correctamente la grafica
            for (int h = 0; h < 24; h++)
            {
                if (!resultado.ContainsKey(h))
                {
                    resultado[h] = 0;
                }
            }

            return resultado;
        }

        public async Task ActividadesActivas()
        {
            // Cargar las listas de Actividades actividad.Activa == true
        }
        
    }
}
