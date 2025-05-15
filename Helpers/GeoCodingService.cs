using Newtonsoft.Json;
namespace Naitv1.Helpers
{
    public interface IGeocodingService
    {
        Task<string> ObtenerCiudadDesdeCoordenadasAsync(float lat, float lon);
    }

    public class GeocodingService : IGeocodingService
    {
        public async Task<string> ObtenerCiudadDesdeCoordenadasAsync(float lat, float lon)
        {
            string url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}";

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("nait-app/1.0");

            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return "Desconocido";

            string json = await response.Content.ReadAsStringAsync();
            dynamic resultado = JsonConvert.DeserializeObject(json);

            return resultado?.address?.city ??
                   resultado?.address?.town ??
                   resultado?.address?.village ??
                   "Desconocido";
        }
    }

}
