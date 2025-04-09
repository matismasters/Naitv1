using Microsoft.AspNetCore.Mvc;

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
    }
}
