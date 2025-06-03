using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Naitv1.Helpers
{
    public class Geolocalizacion
    {
        public static List<Actividad> ActividadesCercanas(
            float latUsuario,
            float lonUsuario,
            int radioEnMetros,
            AppDbContext context
        ) {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(
                srid: 4326
            );

            var centro = geometryFactory.CreatePoint(
                new Coordinate(lonUsuario, latUsuario)
            );

            List<Actividad> lugaresCercanos = context.Actividades
                .Where(actividad => actividad.Ubicacion.IsWithinDistance(centro, radioEnMetros))
                .ToList();

            return lugaresCercanos;
        }
    }
}
