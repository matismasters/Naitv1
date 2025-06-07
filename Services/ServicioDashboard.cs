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

        public async Task<MetricasDashboard> ObtenerMetrics(FiltroDashboard? filtro) // Devuelvo un objeto del tripo MetricasDashboard que es un dto, que me sirve para mandar varios datos 
        {

            MetricasDashboard? datos = new MetricasDashboard

            {
                ActividadesPorHora = await ActividadesPorHora(filtro),
                ActividadesPorCiudad = await ActividadesPorCiudad(filtro),

            };                        

            return datos;
            
        }

        private async Task<Dictionary<string, int>> ActividadesPorCiudad(FiltroDashboard? filtro)
        {
            DateTime desde = filtro?.FechaInicio ?? DateTime.Now.AddDays(-7); // Si vienen vacios tomo valores por defecto
            DateTime hasta = (filtro?.FechaFin?.Date.AddDays(1).AddMilliseconds(-1)) ?? DateTime.Now;

            List<Actividad> actividades = await _context.Actividades
                .Include(a => a.Ciudad)
                .Where(a => a.FechCreado >= desde && a.FechCreado <= hasta)
                .ToListAsync();

            if(filtro.CiudadId.HasValue)
            {
                actividades = actividades
                    .Where(a => a.CiudadId == filtro.CiudadId)
                    .ToList();
            }

            Dictionary<string, int> resultado = actividades
                .GroupBy(a => a.Ciudad.Nombre)
                .ToDictionary(g => g.Key, g => g.Count());

            return resultado;
        }

        private async Task<Dictionary<int, int>> ActividadesPorHora(FiltroDashboard? filtro)
        {

            Console.WriteLine("====Fechas en el metodo Actividades por hora");
            Console.WriteLine($"Fecha ini: {filtro.FechaInicio}");
            Console.WriteLine($"Fecha End: {filtro.FechaFin}");
            Console.WriteLine("====Fechas en el metodo Actividades por hora");

            DateTime desde = filtro?.FechaInicio ?? DateTime.Now.AddHours(-24); // Si vienen vacios tomo valores por defecto 
            DateTime hasta = (filtro?.FechaFin?.Date.AddDays(1).AddMilliseconds(-1)) ?? DateTime.Now; //le tengo que sumar un dia mas y restarle un milisec asi toma todas las actividades hasta las 11:59:59 pm

            List<Actividad> actividades = await _context.Actividades
                .Where(a => a.FechCreado >= desde && a.FechCreado <= hasta)
                .ToListAsync();

            if (filtro?.CiudadId.HasValue == true)
            {
                actividades = actividades
                    .Where(a => a.CiudadId == filtro.CiudadId.Value)
                    .ToList();
            }

            Dictionary<int, int> resultado = actividades
                    .GroupBy(a => a.FechCreado.Hour)
                    .ToDictionary(g => g.Key, g => g.Count());

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
