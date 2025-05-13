using Naitv1.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naitv1.Helpers
{
    // DTOs
    public class ActividadesPorHoraDTO
    {
        public int Hora { get; set; }
        public int Cantidad { get; set; }
    }

    public class ActividadesPorCiudadDTO
    {
        public string Ciudad { get; set; }
        public int Cantidad { get; set; }
    }

    public class ActividadMapaDTO
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
    }

    // Contenedor de resultados
    public class DashboardData
    {
        public List<ActividadesPorHoraDTO> PorHora { get; set; }
        public List<ActividadesPorCiudadDTO> PorCiudad { get; set; }
        public List<ActividadMapaDTO> Activas { get; set; }
    }

    // Servicio principal
    public class ServicioDashboard
    {
        private readonly AppDbContext _context;

        public ServicioDashboard(AppDbContext context)
        {
            _context = context;
        }

        public DashboardData ObtenerMetrics()
        {
            DateTime ahora = DateTime.UtcNow;

            // 1. Agrupamiento real de actividades por hora (últimas 24 hs)
            List<ActividadesPorHoraDTO> datosAgrupados = _context.Actividades
                .Where(a => a.FechaCreacion >= ahora.AddHours(-24))
                .GroupBy(a => a.FechaCreacion.Hour)
                .Select(g => new ActividadesPorHoraDTO
                {
                    Hora = g.Key,
                    Cantidad = g.Count()
                })
                .ToList();

            // 2. Generar lista completa de 0 a 23 horas, llenando con 0 donde no hay datos
            List<ActividadesPorHoraDTO> actividadesPorHora = Enumerable.Range(0, 24)
                .Select(h => new ActividadesPorHoraDTO
                {
                    Hora = h,
                    Cantidad = datosAgrupados.FirstOrDefault(d => d.Hora == h)?.Cantidad ?? 0
                })
                .ToList();

            // Actividades por ciudad en últimos 7 días
            List<ActividadesPorCiudadDTO> actividadesPorCiudad = _context.Actividades
                .Where(a => a.FechaCreacion >= ahora.AddDays(-7))
                .GroupBy(a => a.Ciudad)
                .Select(g => new ActividadesPorCiudadDTO
                {
                    Ciudad = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(dto => dto.Cantidad)
                .ToList();

            // Actividades activas para el mapa
            List<ActividadMapaDTO> actividadesActivas = _context.Actividades
                .Where(a => a.Activa)
                .Select(a => new ActividadMapaDTO
                {
                    Lat = a.Lat,
                    Lon = a.Lon
                })
                .ToList();


            return new DashboardData
            {
                PorHora = actividadesPorHora,
                PorCiudad = actividadesPorCiudad,
                Activas = actividadesActivas
            };
        }
    }
}
