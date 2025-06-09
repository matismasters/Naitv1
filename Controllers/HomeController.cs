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

            //PrecargarDatos();
            Actividad actividad = new Actividad();

            bool estaLogueado = UsuarioLogueado.estaLogueado(HttpContext.Session);
            ViewBag.estaLogueado = estaLogueado;

            if (estaLogueado)
            {
                ViewBag.nombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";
                ViewBag.actividad = UsuarioLogueado.Actividad(_context, HttpContext.Session);

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

      /*  public void PrecargarDatos()
        {
            *//*if (_context.Ciudades.Count() == 0)
            {*//*
                _context.Add(new Ciudad { Nombre = "Artigas", lat = -30.4011f, lon = -56.4667f });
                _context.Add(new Ciudad { Nombre = "Canelones", lat = -34.5167f, lon = -56.2833f });
                _context.Add(new Ciudad { Nombre = "Melo", lat = -32.3703f, lon = -54.1675f });
                _context.Add(new Ciudad { Nombre = "Colonia del Sacramento", lat = -34.4710f, lon = -57.8441f });
                _context.Add(new Ciudad { Nombre = "Durazno", lat = -33.4131f, lon = -56.4992f });
                _context.Add(new Ciudad { Nombre = "Trinidad", lat = -33.5167f, lon = -56.9000f });
                _context.Add(new Ciudad { Nombre = "Florida", lat = -34.0956f, lon = -56.2142f });
                _context.Add(new Ciudad { Nombre = "Minas", lat = -34.3759f, lon = -55.2376f });
                _context.Add(new Ciudad { Nombre = "Maldonado", lat = -34.9106f, lon = -54.9589f });
                _context.Add(new Ciudad { Nombre = "Montevideo", lat = -34.9033f, lon = -56.1882f });
                _context.Add(new Ciudad { Nombre = "Paysandú", lat = -32.3214f, lon = -58.0756f });
                _context.Add(new Ciudad { Nombre = "Fray Bentos", lat = -33.1333f, lon = -58.3000f });
                _context.Add(new Ciudad { Nombre = "Rivera", lat = -30.9053f, lon = -55.5508f });
                _context.Add(new Ciudad { Nombre = "Rocha", lat = -34.4828f, lon = -54.3336f });
                _context.Add(new Ciudad { Nombre = "Salto", lat = -31.3833f, lon = -57.9667f });
                _context.Add(new Ciudad { Nombre = "San José de Mayo", lat = -34.3375f, lon = -56.7136f });
                _context.Add(new Ciudad { Nombre = "Mercedes", lat = -33.2531f, lon = -58.0309f });
                _context.Add(new Ciudad { Nombre = "Tacuarembó", lat = -31.7167f, lon = -55.9833f });
                _context.Add(new Ciudad { Nombre = "Treinta y Tres", lat = -33.2333f, lon = -54.3833f });
                _context.SaveChanges();
            *//*}*//*

        }*/
    }
}
