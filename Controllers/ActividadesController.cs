using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Models;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Migrations;

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
            return View();
        }

        [HttpGet]
        public IActionResult Visibles()
        {
            List<Actividad> actividades = _context.Actividades
                .Where(a => a.Activa == true)
                .Include(a => a.Anfitrion)
                .ToList();

            ViewBag.actividades = actividades;

            return View();
        }

        [HttpPost]
        public IActionResult Index(string mensajeDelAnfitrion, string tipoActividad, float lat, float lon, float? latSuperAdmin, float? lonSuperAdmin)
        {
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            Actividad actividad = new Actividad();

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
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult AgregarParticipante(int IdActividad)
        {
            ActividadesUsuarios reunion = new ActividadesUsuarios();
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);

            if (usuario == null)
            {
                Console.WriteLine("anda  a la concha de tu madre");
            }

            Actividad? actividad = _context.Actividades.Find(IdActividad);
            if (actividad == null)
            {
                Console.WriteLine("anda  a la concha de tu madre");
            }

            reunion.IdActividad = actividad.Id;
            reunion.IdUsuario = usuario.Id;

            _context.ActividadesUsuarios.Add(reunion);
            _context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }
    }

   


}
