namespace Naitv1.Models
{
    public static class FactoryMarcadoresMapa
    {
        public static IMarcadorDeMapa CrearMarcador(Actividad actividad)
        {
            if (actividad.TipoActividad == "Asado")
            {
                return new ActividadMarcadorAsado(actividad);
            }
            else if (actividad.TipoActividad == "Filosofar")
            {
                return new ActividadMarcadorFilosofar(actividad);
            }
            else
            {
                return actividad; 
            }
        }
    }
}
