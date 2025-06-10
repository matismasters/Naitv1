using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
			} 
			else 
			{
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

				ViewBag.notificaciones = notificaciones;

				return View();
			} 
			else 
			{
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

				ViewBag.actividadReportada = actividadReportada;

				return View();
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		public IActionResult BloquearUsuario()
			{
			if (UsuarioLogueado.esModerador(HttpContext.Session))
				{
				List<Usuario> usuarios = _context.Usuarios
					.Where(usuario => usuario.Estado == "Activo")
					.ToList();

                usuarios.RemoveAll(usuario => usuario.Id == UsuarioLogueado.Usuario(HttpContext.Session).Id);

                ViewBag.usuarios = usuarios;

				return View();
				}
			return RedirectToAction("Index", "Home");
			}

        [HttpGet]
        public IActionResult UsuariosBloqueados()
            {
            if (UsuarioLogueado.esModerador(HttpContext.Session))
                {
				List<Usuario> usuariosBloqueados = _context.Usuarios						
					.Where(usuario => usuario.Estado == "Bloqueado")
					.ToList();    

                ViewBag.usuariosBloqueados = usuariosBloqueados;

                return View();
                }
            return RedirectToAction("Index", "Home");
            }

        [HttpPost]
		public IActionResult BloquearUsuario(int id)
			{
			if (UsuarioLogueado.esModerador(HttpContext.Session))
				{
                Usuario usuario = _context.Usuarios
                .Find(id)!;

                if (usuario != null && id != UsuarioLogueado.Usuario(HttpContext.Session).Id)
                    {
                    usuario.Estado = "Bloqueado";
                    _context.SaveChanges();
                    }

				return RedirectToAction("BloquearUsuario", "Moderacion");
                }
			return RedirectToAction("Index", "Home");
			}
	}
}
