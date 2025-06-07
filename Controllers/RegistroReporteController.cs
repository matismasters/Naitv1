using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    public class RegistroReporteController : Controller
    {
        private readonly AppDbContext _context;

        public RegistroReporteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ReportarActividad(int idActividad, string motivo, string descripcion)
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            RegistroNotificacion notificacion = _context.RegistroNotificaciones
                .Where(a => a.UsuarioId == usuario.Id)
                .Where(a => a.ActividadId == idActividad)
                .FirstOrDefault();

            if (notificacion != null)
            {
                return Json(new { error = "Ya hay un reporte creado" });
            }

            RegistroNotificacion nuevoRegistro = new RegistroNotificacion
            {
                ActividadId = idActividad,
                UsuarioId = usuario.Id,
                Motivo = motivo,
                Descripcion = descripcion
            };

            _context.RegistroNotificaciones.Add(nuevoRegistro);
            _context.SaveChanges();

            return Json(nuevoRegistro.Id);
        }
    }
}
