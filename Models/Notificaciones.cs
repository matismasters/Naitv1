using System.Globalization;

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

    }
}
