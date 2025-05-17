using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;
using System.Diagnostics;

namespace Naitv1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            Actividad actividad = new Actividad();

            bool estaLogueado = UsuarioLogueado.estaLogueado(HttpContext.Session);
            ViewBag.estaLogueado = estaLogueado;

            if (estaLogueado)
            {
                ViewBag.nombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";

                List<Actividad> actividades = _context.Actividades
                    .Include(a => a.Anfitrion)
                    .ToList();

                ViewBag.actividades = actividades;
            }

            return View();
        }

        public IActionResult CrearActividad()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Configuracion()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Ayuda()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
