using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

﻿namespace Naitv1.Models
{
    public class Actividad : IMarcadorDeMapa
    {
        public int Id { get; set; }
        public string MensajeDelAnfitrion { get; set; }
        public int AnfitrionId { get; set; }
        public Usuario? Anfitrion { get; set; }
        public string? TipoActividad { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }

        [JsonIgnore]
        public Point Ubicacion { get; set; } = new Point(0, 0) { SRID = 4326 };
        public bool Activa { get; set; } = true;
        public List<RegistroParticipacion> RegistrosParticipacion { get; set; } = new List<RegistroParticipacion>();

        public static List<string> TiposActividad = new List<string>
        {
            "Tomar una",
            "Matear",
            "Fumar algo",
            "Bajonear algo",
            "Musica en vivo",
            "Jugar a algo",
            "Filosofar",
            "Asado",
            "Trabajar"
        };

        // IMarcadorDeMapa 

        public float lat()
        {
            return Lat;
        }

        public float lon()
        {
            return Lon;
        }

        public string mensajeDelAnfitrion()
        {
            return MensajeDelAnfitrion;
        }

        public string urlImagenMarcador()
        {
            return "/assets/marcadorNormal.png";
        }

        public string tipoActividad()
        {
            return TipoActividad ?? "Actividad sin tipo definido";
        }

        public int idActividad()
        {
            return Id;
        }
    }
}