namespace Naitv1.Models
{
    public class Ciudad
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public List<Actividad>? Actividades { get; set; }
    }
}
