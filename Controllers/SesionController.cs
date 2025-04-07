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
            List<Usuario> resultado = _context.Usuarios.Where(
                usuario => usuario.Email == email && usuario.Password == MD5Libreria.Encriptar(password)
            ).ToList();

            if (resultado.Count > 0)
            {
                Usuario usuario = resultado.First();

                HttpContext.Session.SetString("estaLogueado", "true");
                HttpContext.Session.SetString("nombreUsuario", usuario.Nombre);
                HttpContext.Session.SetString("emailUsuario", usuario.Email);

                return Redirect("/");
            } else
            {
                return Redirect("/Sesion/ErrorDeInicio");
            }
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        public IActionResult ErrorDeRegistro()
        {
            return View();
        }

        public IActionResult ErrorDeInicio()
        {
            return View();
        }

        public IActionResult CuentaCreadaConExito()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";

            return View();
        }
        
        public IActionResult Salir()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult CrearUsuario(string nombre, string email, string password, string passwordConfirmation)
        {
            if ( password == passwordConfirmation)
            {
                Usuario usuario = new Usuario();
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Password = MD5Libreria.Encriptar(password);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                HttpContext.Session.SetString("estaLogueado", "true");
                HttpContext.Session.SetString("nombreUsuario", nombre);
                HttpContext.Session.SetString("emailUsuario", email);

                return Redirect("/Sesion/CuentaCreadaConExito");
            } else
            {
                return Redirect("/Sesion/ErrorDeRegistro");
            }
        }
    }
}
