using Naitv1.Migrations;
using System.Collections.Generic;
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

        public string chequeoUsuario(Usuario usuario)
        {

            if (usuario.TipoUsuario == null)
            {
                return usuario.TipoUsuario = "usuarioComun"; ;
            }
            else
            {
                return usuario.TipoUsuario;
            }

        }

    }
}


