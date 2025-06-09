using Naitv1.Data;
using Naitv1.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Naitv1.Services
{
    public class ServicioCiudad
    {
        private readonly AppDbContext _context;

        public ServicioCiudad(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ciudad> ObtenerCiudad(float lat, float lon)
        {
            Console.WriteLine($"%%%%%%%%%%%%%%%%%en servicio ciudad %%%%%%%%%%%%%");
            Console.WriteLine($"latitud: {lat}");
            Console.WriteLine($"longi: {lon}");
            Console.WriteLine($"%%%%%%%%%%%%%%%%%en servicio ciudad %%%%%%%%%%%%%");

            string nombreCiudad = await LlamarApiReverseGeocoding(lat, lon);

            Ciudad? ciudadExistente = await _context.Ciudades
                .FirstOrDefaultAsync(c => c.Nombre == nombreCiudad);

            if (ciudadExistente != null)
            {
                return ciudadExistente;
            }

            Ciudad nuevaCiudad = new Ciudad 
            { 
                Nombre = nombreCiudad 
            };

            _context.Ciudades.Add(nuevaCiudad);
            await _context.SaveChangesAsync();

            return nuevaCiudad;
        }


        private async Task<string> LlamarApiReverseGeocoding(float lat, float lon)
        {
            using var http = new HttpClient();

            var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}";
            http.DefaultRequestHeaders.UserAgent.ParseAdd("NaitApp/1.0 (fernandobonilla832@gmail.com)");

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return "Desconocida";
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<NominatimResponse>(json);            
            
            return data?.address?.city ?? "Desconocida";           
        }


        public class NominatimResponse
        {
            public Address address { get; set; }
        }

        public class Address
        {   
            // Aca tambien me sirve para despues si quiero pasarle mas datos de la ciudad como codigo postal etc
            public string city { get; set; }
            
        }
    }
}
