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
            string tipoUsuarioString = sesionActual.GetString("tipoUsuario") ?? "false";
            bool esAnfitrion = tipoUsuarioString == "anfitrion";

            return esAnfitrion;
        }

        public static bool esPartner(ISession sesionActual)
        {

            string tipoUsuarioString = sesionActual.GetString("tipoUsuario") ?? "false";
            bool esPartner = tipoUsuarioString == "partner";

            return esPartner;
        }
        public static string nombreUsuario(ISession sessionActual)
        {
            string nombreUsuario = sessionActual.GetString("nombreUsuario") ?? "";
            return nombreUsuario;
        }

        public static bool esSuperAdmin(ISession sessionActual) //MODIFICADO EL FALSE PARA QUE ENTRE EN EL IF.
        {
            string tipoUsuarioString = sessionActual.GetString("tipoUsuario") ?? "false";
            bool esSuperAdmin = tipoUsuarioString == "superadmin";
            return esSuperAdmin;
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
    }
}