using Microsoft.AspNetCore.Mvc;
using Naitv1.Helpers;
using Naitv1.Models;
using Naitv1.Data;

namespace Naitv1.Controllers
{
    public class SesionController : Controller
    {
        private readonly AppDbContext _context;

        public SesionController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            string estaLogueadoString = HttpContext.Session.GetString("estaLogueado") ?? "false";
            bool estaLogueado = estaLogueadoString == "true";

            ViewBag.estaLogueado = estaLogueado;
            return View();
        }

        [HttpPost]
        public IActionResult Iniciar(string email, string password)
        {
            HttpContext.Session.SetString("estaLogueado", "true");
            HttpContext.Session.SetString("nombreUsuario", email);
            return Redirect("/");
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearUsuario(string nombre, string email, string password, string passwordConfirmation)
        {
            Usuario usuario = new Usuario();
            usuario.Email = email;
            usuario.Nombre = nombre;
            usuario.Password = MD5Libreria.Encriptar(password);

            _context.Usuarios.Add(usuario);
            HttpContext.Session.SetString("estaLogueado", "true");
            HttpContext.Session.SetString("nombreUsuario", nombre);
            HttpContext.Session.SetString("emailUsuario", email);

            _context.SaveChanges();

            return Redirect("/");
        }
    }
}
