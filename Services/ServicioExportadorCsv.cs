using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using Naitv1.Models;

namespace Naitv1.Services
{
    public class ServicioExportadorCsv
    {
        private readonly AppDbContext _context; 

        public ServicioExportadorCsv(AppDbContext context)
        {
            _context = context;

        }

        public async Task<string> GenerarCsv(FiltroDashboard filtro)
        {
            IQueryable<Actividad> consulta = _context.Actividades
               .Include(a => a.Ciudad)
               .AsQueryable();

            if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue)
            {
                consulta = consulta
                    .Where(a => a.FechCreado >= filtro.FechaInicio.Value
                                            && a.FechCreado <= filtro.FechaFin.Value);
            }

            if (filtro.CiudadId.HasValue)
            {
                consulta = consulta
                    .Where(a => a.CiudadId == filtro.CiudadId.Value);
            }

            var datos = await consulta
                .GroupBy(a => new { Fecha = a.FechCreado.Date, Ciudad = a.Ciudad.Nombre })
                .Select(g => new
                {
                    Fecha = g.Key.Fecha,
                    Ciudad = g.Key.Ciudad,
                    TotalActividades = g.Count()
                })
                .ToListAsync();            

            // Ahora genero el CSV            
            StringWriter writer = new StringWriter();
            CsvWriter csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.WriteRecords(datos);

            // esto devuelve el string resultante
            string resultadoCsv = writer.ToString();

            return resultadoCsv;
            
        }
    }

    
}
