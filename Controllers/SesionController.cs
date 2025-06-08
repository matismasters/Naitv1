using Microsoft.AspNetCore.Mvc;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    public class SesionController : Controller
    {
        private readonly AppDbContext _context;

        public SesionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.estaLogueado = UsuarioLogueado.estaLogueado(HttpContext.Session);
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
                UsuarioLogueado.loguearUsuario(HttpContext.Session, usuario);

                if (usuario.TipoUsuario == "moderador")
				{
					return RedirectToAction("Index", "Moderacion");
				} else {
					return Redirect("/");
				}

            } else
            {
                return Redirect("/Sesion/ErrorDeInicio");
            }
        }

        public IActionResult Registrarse()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult RegistroAnfitrion()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult RegistroPartner()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

 
        public IActionResult ErrorDeRegistro()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult ErrorDeInicio()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult CuentaCreadaConExito()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.NombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";

            return View();
        }

       

        [HttpPost]
        public IActionResult CrearUsuario(string nombre, string email, string password, string passwordConfirmation)
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            if (password == passwordConfirmation)
            {
                Usuario usuario = new Usuario();
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Password = MD5Libreria.Encriptar(password);
                usuario.TipoUsuario = "basico";

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                UsuarioLogueado.loguearUsuario(HttpContext.Session, usuario);

                return Redirect("/Sesion/CuentaCreadaConExito");
            }
            else
            {
                return Redirect("/Sesion/ErrorDeRegistro");
            }
        }

        public IActionResult CrearUsuarioAnfitrion(string nombre, string email, string password, string passwordConfirmation)
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            if (password == passwordConfirmation)
            {
                Usuario usuario = new Usuario();
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Password = MD5Libreria.Encriptar(password);
                usuario.TipoUsuario = "anfitrion";

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                //

                UsuarioLogueado.loguearUsuario(HttpContext.Session, usuario);

                return Redirect("/Sesion/CuentaCreadaConExito");
            }
            else
            {
                return Redirect("/Sesion/ErrorDeRegistro");
            }
        }

        public IActionResult CrearUsuarioPartner(string nombre, string email, string password, string passwordConfirmation, string nombrePartner, string? direccion, string logoUrl, string? descripcion, int telefono, string ciudad, string pais, string departamento)
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            if (password == passwordConfirmation)
            {
                Usuario usuario = new Usuario();
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Password = MD5Libreria.Encriptar(password);
                usuario.TipoUsuario = "partner";

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                Partner partner = new Partner();
                partner.Email = usuario.Email;
                partner.Nombre = nombrePartner;
                partner.Direccion = direccion;
                partner.LogoUrl = logoUrl;
                partner.Descripcion = descripcion;
                partner.Telefono = telefono;
                partner.Estado = EstadoPartner.Pendiente;
                partner.Ciudad = ciudad;
                partner.Pais = pais;
                partner.Departamento = departamento;
                partner.EsVerificado = false;
                partner.CreadorId = usuario.Id;
                _context.Partners.Add(partner);
                _context.SaveChanges();

                //

                UsuarioLogueado.loguearUsuario(HttpContext.Session, usuario);

                return Redirect("/Sesion/CuentaCreadaConExito");
            }
            else
            {
                return Redirect("/Sesion/ErrorDeRegistro");
            }
        }

        public IActionResult Salir()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                HttpContext.Session.Clear();
            }

            return Redirect("/");
        }
    }

}

