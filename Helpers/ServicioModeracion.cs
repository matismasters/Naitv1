using Naitv1.Models;

namespace Naitv1.Helpers
{
    public class ServicioModeracion
    {                               
        public static void ReportarActividad(int actividadId, int usuarioId, string motivo, string descripcion)
        {
            RegistroNotificacion registro = new RegistroNotificacion();
            registro.ActividadId = actividadId;
            registro.UsuarioId = usuarioId;
		    registro.Motivo = motivo;
            registro.Descripcion = descripcion;
            registro.EstadoNotificacion = "Pendiente";
	    }
    }
}
