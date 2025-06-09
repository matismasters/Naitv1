using System.ComponentModel;
using System.Text.RegularExpressions;
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

        public IActionResult Crear(string? asunto, string? cuerpo)
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
            PlantillaNotificacion vacia = new PlantillaNotificacion();
            ViewBag.Planilla = vacia;
            ViewBag.Planilla.AsuntoTemplate = asunto;
            ViewBag.Planilla.CuerpoTemplate = cuerpo;
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
        public IActionResult VerPlanillas()
        {
            if (UsuarioLogueado.esAnfitrion(HttpContext.Session) == false)
            {
                ViewBag.Error = "No es Anfitrion";
            }
            else { ViewBag.Error = null; }

            List<PlantillaNotificacion> plantillas = _context.PlantillaNotificacion?.ToList();

            if (plantillas.Count > 0)
            {
                ViewBag.Plantillas = plantillas;
            }
            else
            {
                ViewBag.Plantillas = null;
            }

                return View("Notificaciones/PlantillaNotificacion");
        }

        [HttpPost]
        public IActionResult VerPlanillas(string OpcionSeleccionada, string Titulo, string Asunto, string Cuerpo, string accion)
        {
            PlantillaNotificacion nueva = new PlantillaNotificacion
            {
                NombrePlanilla = Titulo,
                AsuntoTemplate = Asunto,
                CuerpoTemplate = Cuerpo
            };

            ViewBag.Planilla = nueva;

            if (accion == "crear")
            {
                _context.PlantillaNotificacion.Add(nueva);
                _context.SaveChanges();
                return RedirectToAction("Crear");
            }
            else
            {
                return RedirectToAction("Crear");
            }
        }
    }
}
