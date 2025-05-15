using System.Globalization;
using System.Text.Json;

namespace Naitv1.Models
{
    public class Notificaciones
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Mensaje { get; set; }

        public DateTime FechaHoraProgramada { get; set; }

        public string CriterioSegmento { get; set; }

        public string EstadoNotificacion { get; set; }

        public static string CrearCriterio(List<int> idCiudad, List<string> rolUsuario)
        {
            var criterio = new
            {
                ciudad = idCiudad,
                tipoUsuario = rolUsuario
            };

            string CriterioSegmento = JsonSerializer.Serialize(criterio);
            return CriterioSegmento;
        }

        public static (List<int> ciudad, List<string> tipoUsuario) VerCriterio( string CriterioSegmento)
        {
            if (string.IsNullOrWhiteSpace(CriterioSegmento))
                return (new List<int>(), new List<string>());

            using var doc = JsonDocument.Parse(CriterioSegmento);

            var ciudad = doc.RootElement.GetProperty("ciudad")
                .EnumerateArray()
                .Select(e => e.GetInt32())
                .ToList();

            var tipoUsuario = doc.RootElement.GetProperty("tipoUsuario")
                .EnumerateArray()
                .Select(e => e.GetString())
                .ToList();

            return (ciudad, tipoUsuario);
        }
    }
    
}
