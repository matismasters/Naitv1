using System.ComponentModel.DataAnnotations;

namespace Naitv1.Models
{
    public enum EstadoPartner
    {
        Pendiente,
        Rechazado,
        Aceptado
    }

    public class Partner
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "El campo Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Email no es una dirección de correo válida")]
        public string Email { get; set; }

        public string LogoUrl { get; set; }
        public string? Descripcion { get; set; }
        public EstadoPartner Estado { get; set; } = EstadoPartner.Pendiente;

        public string Ciudad { get; set; }

        public string Pais { get; set; }

        public string Departamento { get; set; } = "Sin departamento";
        public bool EsVerificado { get; set; } = false;

        public Usuario? Creador { get; set; }

        public int CreadorId { get; set; }
    }
}