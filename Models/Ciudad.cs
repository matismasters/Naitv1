namespace Naitv1.Models
{
    public class Ciudad
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } 

        public float lat { get; set; }
        public float lon { get; set; }


        public List<Actividad> ListActividades { get; set; } = new List<Actividad>();

    }

}
