using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    public class PartnersController : Controller
    {
        private readonly AppDbContext _context;

        public PartnersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Crear() //modificado
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("idUsuario");

            if (usuarioId != null)
            {
                var partners = _context.Partners.ToList();
                return View(partners);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

     
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var partner = _context.Partners.FirstOrDefault(p => p.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

            _context.Partners.Remove(partner);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Aprobar(int id)
        {

           var partner = _context.Partners.FirstOrDefault(p => p.Id == id);
            if (partner == null)
            {
                return NotFound(); 
            }
            var usuarioAmodificar = _context.Usuarios.FirstOrDefault(u => u.Id == partner.CreadorId);
            if (usuarioAmodificar != null)
            {
                usuarioAmodificar.TipoUsuario = "partner";
            }
            partner.Estado = EstadoPartner.Aceptado;
            partner.EsVerificado = true;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditarMiPerfil()
        {
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");
            var partnerEditar = _context.Partners.FirstOrDefault(p => p.CreadorId == idUsuario);
            if (partnerEditar == null)
            {
                return RedirectToAction("Index", "Home"); // O mostrar error que no existe
            } 

            return View("Editar", partnerEditar);
        }


        [HttpPost]
        public IActionResult Editar(int id, string nombre, string? direccion, string logoUrl, string? descripcion, int telefono, string email, string ciudad, string pais, string departamento)
        {
            var partner = _context.Partners.FirstOrDefault(p => p.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

                       partner.Nombre = nombre;
            partner.Direccion = direccion;
            partner.LogoUrl = logoUrl;
            partner.Descripcion = descripcion;
            partner.Telefono = telefono;
            partner.Email = email;
            partner.Ciudad = ciudad;
            partner.Pais = pais;
            partner.Departamento = departamento;

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}