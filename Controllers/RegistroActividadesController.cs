using Microsoft.AspNetCore.Mvc;
using Naitv1.Helpers;
using Naitv1.Models;
using Naitv1.Data;
using Microsoft.EntityFrameworkCore;

namespace Naitv1.Controllers
{
    public class RegistroActividadesController : Controller
    {
        private readonly AppDbContext _context;

        public RegistroActividadesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Participar(int idActividad)
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return RedirectToAction("Index", "Sesion");
            }

            Actividad? actividad = _context.Actividades.Find(idActividad);
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            ViewBag.Usuario = usuario;
            ViewBag.Actividad = actividad;

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada.");
            }

            RegistroParticipacion registro = new RegistroParticipacion
            {
                ParticipanteId = usuario.Id,
                ActividadId = actividad.Id,
                FechaRegistro = DateTime.Now
            };

            _context.RegistrosParticipacion.Add(registro);
            _context.SaveChanges();

            ViewBag.RegistrosParticipacion = _context.RegistrosParticipacion
                .Where(r => r.ActividadId == actividad.Id)
                .Where(r => r.ParticipanteId == usuario.Id)
                .Include(r => r.Actividad)
                .Include(r => r.Participante)
                .ToList();

            return View();
        }
    }
}
