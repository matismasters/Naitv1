namespace Naitv1.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ConfiguracionReporte
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un día.")]
        public string DiaObjetivo { get; set; }

        [Range(0, 23, ErrorMessage = "La hora debe estar entre 0 y 23.")]
        public int Hora { get; set; }

        [Range(0, 59, ErrorMessage = "El minuto debe estar entre 0 y 59.")]
        public int Minuto { get; set; }

        [Required(ErrorMessage = "El destinatario es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Destinatario { get; set; }

        [Required(ErrorMessage = "Debe ingresar un asunto.")]
        public string Asunto { get; set; }
    }


}
