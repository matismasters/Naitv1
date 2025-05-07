namespace Naitv1.Models
{
    public class RegistroEmail
    {
        public int Id { get; set; } 
        public string Destinatario {  get; set; }   
        public string Asunto { get; set; }
        public string CuerpoHtml { get; set; }  
        public DateTime FechaProgramada { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    }
}
