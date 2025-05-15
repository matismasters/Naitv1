using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult Index(string nombre, string? direccion, string logoUrl, string? descripcion, int telefono, string email, string ciudad, string pais, string departamento)
        {
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            Partner partner = new Partner();

            partner.Nombre = nombre;
            partner.Direccion = direccion;
            partner.LogoUrl = logoUrl;
            partner.Descripcion = descripcion;
            partner.Telefono = telefono;
            partner.Email = email;
            partner.Estado = EstadoPartner.Pendiente;
            partner.Ciudad = ciudad;
            partner.Pais = ciudad;
            partner.Departamento = departamento;
            partner.EsVerificado = false;
            partner.CreadorId = usuario.Id;
            _context.Partners.Add(partner);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
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

            partner.Estado = EstadoPartner.Aceptado;
            partner.EsVerificado = true;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}