namespace Naitv1.Models
{
    public class RegistroNotificacion
    {
        public int RegistroId { get; set; }

        public int ReferenciaId { get; set; }

        public string Tipo { get; set; }

        public string Motivo { get; set; }

        public string EstadoNotificacion { get; set; }

        public DateTime FechaNotificacion { get; set; } = DateTime.Now;

    }
}
