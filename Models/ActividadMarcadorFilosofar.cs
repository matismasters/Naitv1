namespace Naitv1.Models
{
    public class ActividadMarcadorFilosofar : IMarcadorDeMapa
    {
        private readonly Actividad _actividad;

        public ActividadMarcadorFilosofar(Actividad actividad)
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
            return "Filosofando: " + _actividad.MensajeDelAnfitrion;
        }

        public string urlImagenMarcador()
        {
            return "/assets/marcadorFilosofar.png";
        }

        public string tipoActividad()
        {
            return "Filosofar!";
        }

        public int idActividad()
        {
            return _actividad.Id;
        }
    }
}
