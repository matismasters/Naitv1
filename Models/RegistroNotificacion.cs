namespace Naitv1.Models
    {
    public class RegistroNotificacion
        {
        public int RegistroId { get; set; }

        public int ReferenciaId { get; set; }

        public int UsuarioId { get; set; }

		public Actividad? ActividadRef { get; set; }

        public string Tipo { get; set; } = "Actividad";

        public string Motivo { get; set; }

        public string Descripcion { get; set; }

		public string EstadoNotificacion { get; set; }

        public DateTime FechaNotificacion { get; set; } = DateTime.Now;

        public List<string> Motivos = new List<string>
        {
            "Peligroso", "Fraude", "Actividad Politica", "Engañoso", "Otros"
        };
        }
    }