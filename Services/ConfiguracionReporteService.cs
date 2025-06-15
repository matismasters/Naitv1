using Naitv1.Data;
using Naitv1.Models;
using System.Text.Json;

namespace Naitv1.Services
{
    public class ConfiguracionReporteService
    {
        private readonly string _rutaArchivo;
        private readonly AppDbContext _appDbContext;

        public ConfiguracionReporteService(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _rutaArchivo = Path.Combine(env.ContentRootPath, "appConfig", "configuracionReporte.json");
            _appDbContext = dbContext;
        }

        public ConfiguracionReporte Obtener()
        {
            if (!File.Exists(_rutaArchivo))
                return new ConfiguracionReporte();

            string json = File.ReadAllText(_rutaArchivo);
            return JsonSerializer.Deserialize<ConfiguracionReporte>(json);
        }

        public void Guardar(ConfiguracionReporte config)
        {
            ConfiguracionReporte reporte = new ConfiguracionReporte
            {
                DiaObjetivo = config.DiaObjetivo,
                Hora = config.Hora,
                Minuto = config.Minuto,
                Destinatario = config.Destinatario,
                Asunto = config.Asunto
            };
            _appDbContext.ConfiguracionReportes.Add(reporte);
            _appDbContext.SaveChanges();


            var dir = Path.GetDirectoryName(_rutaArchivo);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_rutaArchivo, json);
        }
    }

}
