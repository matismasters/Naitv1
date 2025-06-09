using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Models;
using Naitv1.Data;
using Naitv1.Helpers;
using System.Net.Http.Json;

namespace Naitv1.Controllers
{
    public class ActividadesController : Controller
    {
        private readonly AppDbContext _context;

        public ActividadesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Actividad? actividad = UsuarioLogueado.Actividad(_context, HttpContext.Session);
            ViewBag.Actividad = actividad ?? new Actividad();

            return View();
        }

        [HttpGet]
        public IActionResult Visibles()
        {
            List<Actividad> actividades = _context.Actividades
                .Where(a => a.Activa == true)
                .Include(a => a.Anfitrion)
                .ToList();

            return Json(actividades);
        }

        private double CalcularDistancia(float lat1, float lon1, float lat2, float lon2)
        {
            var R = 6371e3; // Radio de la Tierra en metros
            var phi1 = lat1 * Math.PI / 180;
            var phi2 = lat2 * Math.PI / 180;
            var deltaPhi = (lat2 - lat1) * Math.PI / 180;
            var deltaLambda = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = R * c;
            return d;
        }

        



    [HttpPost]
        public IActionResult Index(int idActividad, string mensajeDelAnfitrion, string tipoActividad, float lat, float lon, float? latSuperAdmin, float? lonSuperAdmin, string? submit)
        {
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            Actividad actividad;

			ViewBag.Actividad = new Actividad(); // Prevenís el null


			Ciudad? ciudadMasCercana = _context.Ciudades
            .AsEnumerable()
            .OrderBy(c => CalcularDistancia(c.lat, c.lon, lat, lon))
            .FirstOrDefault();


            Console.WriteLine("KEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEHHHHHHHHHHHHHH");

            if (idActividad != 0 )
            {
                actividad = _context.Actividades.Find(idActividad) ?? new Actividad();
                actividad.MensajeDelAnfitrion = mensajeDelAnfitrion;
                actividad.TipoActividad = tipoActividad;

                _context.Actividades.Update(actividad);
            } else
            {
                actividad = new Actividad();
                actividad.MensajeDelAnfitrion = mensajeDelAnfitrion;
                actividad.TipoActividad = tipoActividad;
 
                if (latSuperAdmin != null && lonSuperAdmin != null && UsuarioLogueado.esSuperAdmin(HttpContext.Session))
                {
                    actividad.Lat = (float) latSuperAdmin;
                    actividad.Lon = (float) lonSuperAdmin;
                }
                else
                {
                    actividad.Lat = lat;
                    actividad.Lon = lon;
                }
                if (ciudadMasCercana != null)
                {
                    actividad.CiudadId = ciudadMasCercana.Id;
                    actividad.AnfitrionId = usuario.Id;
                    _context.Actividades.Add(actividad);
                }
                else
                {
                    actividad.CiudadId = 3;
					actividad.AnfitrionId = usuario.Id;
					_context.Actividades.Add(actividad);
				}
            }

            _context.SaveChanges();

			return RedirectToAction("Index", "Home");
        }
    }
}
