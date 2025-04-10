using System.ComponentModel;

namespace Naitv1.Models
{
    public class Actividad
    {
        public int Id { get; set; }
        public string MensajeDelAnfitrion { get; set; }
        public int AnfitrionId { get; set; }
        public Usuario? Anfitrion { get; set; }
        public string? TipoActividad { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public bool Activa { get; set; } = true;

        public static List<string> TiposActividad = new List<string>
        {
            "Tomar una",
            "Matear",
            "Fumar algo",
            "Bajonear algo",
            "Musica en vivo",
            "Jugar a algo",
            "Filosofar",
            "Asado",
            "Trabajar"
        };
    }
}
