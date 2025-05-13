using System.ComponentModel.DataAnnotations;

namespace Naitv1.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TipoUsuario { get; set; }

        //public int? Codigo { get; set; }

        public List<string> TiposUsuarios = new List<string>
        {
            "basico", "anfitrion", "superadmin", "moderador"
        };
    }
}
