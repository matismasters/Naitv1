using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace Naitv1.Models
{
    public class ActividadesUsuarios
    {
        public int Id { get; set; } 
        public int IdActividad { get; set; }
        public int IdUsuario { get; set; }
    
    }
}
