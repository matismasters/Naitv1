using Naitv1.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naitv1.Helpers
{
    // Estadisicas del dashboard
    public class DashboardData
    {
        public List<ActividadesPorHoraDTO> PorHora { get; set; }
        public List<ActividadesPorCiudadDTO> PorCiudad { get; set; }

    }
    // DTOs tablas que muestra chart.js
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

    public class ServicioDashboard
    {
        private readonly AppDbContext _context;

        public ServicioDashboard(AppDbContext context)
        {
            _context = context;
        }

        public List<string> ObtenerTodasLasCiudades()
        {
            List<string> resultadoCiudades = _context.Actividades
                .Where(a => a.Ciudad != null)
                .Select(a => a.Ciudad)
                .Distinct()
                .OrderBy(c => c)
                .ToList() !;
            return resultadoCiudades ;
        }
        public DashboardData ObtenerMetrics()
        {
            DateTime ahora = DateTime.UtcNow;

            // actividades por hora (últimas 24 hs)
            List<ActividadesPorHoraDTO> datosAgrupados = _context.Actividades
                .Where(a => a.FechaCreacion >= ahora.AddHours(-24))
                .GroupBy(a => a.FechaCreacion.Hour)
                .Select(g => new ActividadesPorHoraDTO
                {
                    Hora = g.Key,
                    Cantidad = g.Count()
                })
                .ToList();

            // aca pasamos las estadisticas y ponemos que vaya de a una hora la tabla, que cuando no hay actividad ponga 0
            List<ActividadesPorHoraDTO> actividadesPorHora = Enumerable.Range(0, 24)
                .Select(h => new ActividadesPorHoraDTO
                {
                    Hora = h,
                    Cantidad = datosAgrupados.FirstOrDefault(d => d.Hora == h)?.Cantidad ?? 0
                })
                .ToList();

            // actividades por ciudad en últimos 7 días
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

            return new DashboardData
            {
                PorHora = actividadesPorHora,
                PorCiudad = actividadesPorCiudad,
            };
        }

        public DashboardData ObtenerMetricsFiltrado(string ciudad, DateTime fechaInicio, DateTime fechaFin)
        {
            if ((fechaFin - fechaInicio).TotalDays > 90)
            {
                throw new Exception("Rango máximo 90 días");
            }

            var query = _context.Actividades
                .Where(a => a.FechaCreacion >= fechaInicio && a.FechaCreacion <= fechaFin);

            if (!string.IsNullOrEmpty(ciudad))
            {
                query = query.Where(a => a.Ciudad == ciudad);
            }

            // Agrupar por hora
            var agrupadoPorHora = query
                .GroupBy(a => a.FechaCreacion.Hour)
                .Select(g => new ActividadesPorHoraDTO
                {
                    Hora = g.Key,
                    Cantidad = g.Count()
                })
                .ToList();

            var actividadesPorHora = Enumerable.Range(0, 24)
                .Select(h => new ActividadesPorHoraDTO
                {
                    Hora = h,
                    Cantidad = agrupadoPorHora.FirstOrDefault(d => d.Hora == h)?.Cantidad ?? 0
                }).ToList();

            // Agrupar por ciudad
            var actividadesPorCiudad = query
                .GroupBy(a => a.Ciudad)
                .Select(g => new ActividadesPorCiudadDTO
                {
                    Ciudad = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .ToList();

            return new DashboardData
            {
                PorHora = actividadesPorHora,
                PorCiudad = actividadesPorCiudad
            };
        }

    }
}
