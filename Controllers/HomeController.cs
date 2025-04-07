using Microsoft.AspNetCore.Mvc;
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
            string estaLogueadoString = HttpContext.Session.GetString("estaLogueado") ?? "false";
            bool estaLogueado = estaLogueadoString == "true";

            ViewBag.estaLogueado = estaLogueado;

            if (estaLogueado)
            {
                ViewBag.nombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";
            }

            return View();
        }

        public IActionResult CrearActividad()
        {
            HttpContext.Session.SetString("estaLogueado", "true");
            HttpContext.Session.SetString("nombreUsuario", "Matias");
            return View();
        }

        public IActionResult Configuracion()
        {
            HttpContext.Session.SetString("estaLogueado", "false");
            HttpContext.Session.SetString("nombreUsuario", "");
            return View();
        }

        public IActionResult Ayuda()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
