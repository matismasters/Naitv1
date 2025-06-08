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
        public IActionResult Participar(float latUsuario, float lonUsuario)
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session) == false)
            {
                return Json(new { error = "No esta logueado" });
            }

            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);

            List<Actividad> actividadesCercanas = Geolocalizacion.ActividadesCercanas(
                latUsuario: latUsuario,
                lonUsuario: lonUsuario,
                radioEnMetros: 50,
                context: _context
            );

            if (actividadesCercanas.Count == 0)
            {
                return Json(new { error = "No hay actividades cercanas" });
            }

            RegistroParticipacion registro = new RegistroParticipacion
            {
                ParticipanteId = usuario.Id,
                ActividadId = actividadesCercanas.First().Id,
                FechaRegistro = DateTime.Now
            };

            _context.RegistrosParticipacion.Add(registro);
            _context.SaveChanges();

            return Json(registro.Id);
        }
    }
}
