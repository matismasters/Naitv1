using Microsoft.AspNetCore.Mvc;
using Naitv1.Helpers;
using Naitv1.Models;
using System.Diagnostics;

namespace Naitv1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            bool estaLogueado = UsuarioLogueado.estaLogueado(HttpContext.Session);
            ViewBag.estaLogueado = estaLogueado;

            if (estaLogueado)
            {
                ViewBag.nombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";
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
