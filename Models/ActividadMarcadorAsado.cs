namespace Naitv1.Models
{
    public class ActividadMarcadorAsado : IMarcadorDeMapa
    {
        private readonly Actividad _actividad;

        public ActividadMarcadorAsado(Actividad actividad)
        {
            _actividad = actividad;
        }

        public float lat()
        {
            return _actividad.Lat;
        }

        public float lon()
        {
            return _actividad.Lon;
        }

        public string mensajeDelAnfitrion()
        {
            return "EL TAL ASADO: " + _actividad.MensajeDelAnfitrion;
        }

        public string urlImagenMarcador()
        {
            return "/assets/marcadorNormal.png";
        }

        public string tipoActividad()
        {
            return "Asado!";
        }

        public int idActividad()
        {
            return _actividad.Id;
        }
    }
}
