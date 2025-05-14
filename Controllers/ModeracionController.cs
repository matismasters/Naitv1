using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    public class ModeracionController : Controller
    {
        private readonly AppDbContext _context;

        public ModeracionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
			if (UsuarioLogueado.esModerador(HttpContext.Session))
			{
				return View();
			} else {
				return RedirectToAction("Index", "Home");
			}
		}

        public IActionResult Notificaciones()
        {
            if (UsuarioLogueado.esModerador(HttpContext.Session))
			{
				List<RegistroNotificacion> notificaciones = _context.
					RegistroNotificaciones.
					ToList();

				return View(notificaciones);
			} else {
				return RedirectToAction("Index", "Home");
			}
        }

		public IActionResult Reportes(int id)
			{
			if (UsuarioLogueado.esModerador(HttpContext.Session))
				{
				var actividadReportada = _context.
					RegistroNotificaciones.
					Find(id);

				return View(actividadReportada);
				}
			else
				{
				return RedirectToAction("Index", "Home");
				}
			}

		public IActionResult Estadisticas()
			{
			if (UsuarioLogueado.esModerador(HttpContext.Session))
				{
				return View();
				}
			else
				{
				return RedirectToAction("Index", "Home");
				}
			}

		public IActionResult Mapa()
			{
			if (UsuarioLogueado.esModerador(HttpContext.Session))
				{
				return View();
				}
			else
				{
				return RedirectToAction("Index", "Home");
				}
			}
		}
}
