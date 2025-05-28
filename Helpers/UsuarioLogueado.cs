using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Models;

namespace Naitv1.Helpers
{
    public class UsuarioLogueado
    {

        public static bool estaLogueado(ISession sesionActual)
        {
            string estaLogueadoString = sesionActual.GetString("estaLogueado") ?? "false";
            bool estaLogueado = estaLogueadoString == "true";

            return estaLogueado;
        }

        public static bool esAnfitrion(ISession sesionActual)
        {
            string tipoUsuarioString = sesionActual.GetString("tipoUsuario") ?? "basico";
            bool esAnfitrion = tipoUsuarioString == "anfitrion";

            return esAnfitrion;
        }

        public static string nombreUsuario(ISession sessionActual)
        {
            string nombreUsuario = sessionActual.GetString("nombreUsuario") ?? "";
            return nombreUsuario;
        }

        public static int idUsuario(ISession sesionActual)
        {
            int idUsuario = sesionActual.GetInt32("idUsuario") ?? 0;
            return idUsuario;
 
        }

        public static bool esSuperAdmin(ISession sessionActual)
        {
            string tipoUsuarioString = sessionActual.GetString("tipoUsuario") ?? "basico";
            bool esSuperAdmin = tipoUsuarioString == "superadmin";
            return esSuperAdmin;
        }

		public static bool esModerador(ISession sessionActual)
		{
		    string tipoUsuarioString = sessionActual.GetString("tipoUsuario") ?? "basico";
		    bool esModerador = tipoUsuarioString == "moderador";
		    return esModerador;
		}

		public static void loguearUsuario(ISession sesionActual, Usuario usuario)
        {
            sesionActual.SetString("estaLogueado", "true");

            sesionActual.SetInt32("idUsuario", usuario.Id);
            sesionActual.SetString("nombreUsuario", usuario.Nombre);
            sesionActual.SetString("emailUsuario", usuario.Email);
            sesionActual.SetString("tipoUsuario", usuario.TipoUsuario);
        }

        public static Usuario Usuario(ISession sesionActual)
        {
            Usuario usuario = new Usuario();
            usuario.Id = sesionActual.GetInt32("idUsuario") ?? 0;
            usuario.Nombre = sesionActual.GetString("nombreUsuario") ?? "";
            usuario.Email = sesionActual.GetString("emailUsuario") ?? "";
            usuario.TipoUsuario = sesionActual.GetString("tipoUsuario") ?? "basico";

            return usuario;
        }

        public static Actividad? Actividad(AppDbContext dbContext, ISession sesionActual)
        {
            int idUsuario = UsuarioLogueado.idUsuario(sesionActual);

            Actividad? actividad = dbContext.Actividades
                .Where(actividad => actividad.AnfitrionId == idUsuario && actividad.Activa == true)
                .FirstOrDefault();

            return actividad;
        }
    }
}
