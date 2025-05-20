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

        [HttpPost]
        public IActionResult Index(int idActividad, string mensajeDelAnfitrion, string tipoActividad, float lat, float lon, float? latSuperAdmin, float? lonSuperAdmin, string? submit)
        {
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            Actividad actividad;

            if (idActividad != 0)
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
                actividad.AnfitrionId = usuario.Id;
                _context.Actividades.Add(actividad);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
