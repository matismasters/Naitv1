using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    [Route("Admin/[action]")]

    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotificacionesIndex()
        {
            if (UsuarioLogueado.esAnfitrion(HttpContext.Session) == false)
            {
                ViewBag.Error = "Debes estar logeado como super admin para acceder a esta configuracion";
            }
            else { ViewBag.Error = null; }

            List<Notificaciones> notificaciones = _context.Notificaciones.ToList();

            ViewBag.Notificaciones = notificaciones;

            return View("Notificaciones/Index");
        }

        public IActionResult Crear()
        {
            if (UsuarioLogueado.esAnfitrion(HttpContext.Session) == false)
            {
                ViewBag.Error = "No es superAdmin";
            }
            else { ViewBag.Error = null; }

            List<Ciudades> Ciudades = _context.Ciudades.ToList();

            if (Ciudades.Count > 0)
            {
                ViewBag.Ciudades = Ciudades;
            }
            return View("Notificaciones/Crear");
        }

        [HttpPost]
        public IActionResult Crear (string tituloNotificacion, string mensajeNotificacion, List<int> idsCiudades, List<string> roles, DateTime fechaProgramadaNotificacion) 
        {
            string criteriosSegmentados = Notificaciones.CrearCriterio(idsCiudades, roles);
            
            Notificaciones notificacion = new Notificaciones();
            
            notificacion.Titulo = tituloNotificacion;
            notificacion.Mensaje = mensajeNotificacion;
            notificacion.CriterioSegmento = criteriosSegmentados;
            notificacion.FechaHoraProgramada = fechaProgramadaNotificacion;
            notificacion.EstadoNotificacion = "pendiente";

            _context.Notificaciones.Add(notificacion);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
