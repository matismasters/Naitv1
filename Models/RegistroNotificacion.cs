using Microsoft.EntityFrameworkCore;

namespace Naitv1.Models
    {
    public class RegistroNotificacion
        {
        public int Id { get; set; }


        public int ActividadId { get; set; }
        public Actividad? Actividad { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public string Tipo { get; set; } = "Actividad";

        public string Motivo { get; set; }

        public string Descripcion { get; set; }

        public string EstadoNotificacion { get; set; }

        public DateTime FechaNotificacion { get; set; } = DateTime.Now;

        public static List<string> Motivos = new List<string>
            {
            "Peligroso", "Fraude", "Actividad Politica", "Engañoso", "Otros"
            };
        }
    }