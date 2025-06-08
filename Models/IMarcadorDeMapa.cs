namespace Naitv1.Models
{
    public interface IMarcadorDeMapa
    {
        public float lat();
        public float lon();
        public string mensajeDelAnfitrion();
        public string urlImagenMarcador();
        public string tipoActividad();
        public int idActividad();
    }
}
