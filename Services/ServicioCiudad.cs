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


        private async Task<string> LlamarApiReverseGeocoding(double lat, double lon)
        {
            /*using var http = new HttpClient();

            var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}";
            http.DefaultRequestHeaders.UserAgent.ParseAdd("NaitApp/1.0 (fernandobonilla832@gmail.com)");

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return "Desconocida";
            }               

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<NominatimResponse>(json);

            Console.WriteLine(json);

            return data?.address?.city
                ?? data?.address?.town
                ?? data?.address?.village
                ?? "Desconocida";*/

            using var http = new HttpClient();

            // Formatear los floats para asegurar que usan punto decimal, no coma
            string latStr = lat.ToString("F8", CultureInfo.InvariantCulture);
            string lonStr = lon.ToString("F8", CultureInfo.InvariantCulture);
            Console.WriteLine($"===========================================================");
            Console.WriteLine($"lat: {lat}");
            Console.WriteLine($"long: {lat}");

            var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}";
            http.DefaultRequestHeaders.UserAgent.ParseAdd("NaitApp/1.0 (mailto:fernandobonilla832@gmail.com)");

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ HTTP ERROR {(int)response.StatusCode}: {response.ReasonPhrase}");
                Console.WriteLine($"Respuesta: {error}");
                return "Desconocida";
            }

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("✅ JSON Recibido:");
            Console.WriteLine(json);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("address", out var address))
            {
                if (address.TryGetProperty("city", out var city)) return city.GetString();
                if (address.TryGetProperty("town", out var town)) return town.GetString();
                if (address.TryGetProperty("village", out var village)) return village.GetString();
            }

            return "Desconocida";
        }


        public class NominatimResponse
        {
            public Address address { get; set; }
        }

        public class Address
        {
            [JsonPropertyName("city")]
            public string city { get; set; }
            public string town { get; set; }
            public string village { get; set; }
        }
    }
}
