namespace Naitv1.Models
{
    public class RegistroParticipacion
    {
        public int Id { get; set; }
        public int ActividadId { get; set; }
        public Actividad? Actividad { get; set; }
        public int ParticipanteId { get; set; }
        public Usuario? Participante { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
