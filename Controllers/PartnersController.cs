using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Index(string nombre, string? direccion, string logoUrl, string? descripcion,int telefono, string email)
        {
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            Partner partner = new Partner();

            partner.Nombre = nombre;
            partner.Direccion = direccion;
            partner.LogoUrl = logoUrl; // jijiijij
            partner.Descripcion = descripcion;
            partner.Telefono = telefono;
            partner.Email = email;
            partner.Estado = EstadoPartner.Pendiente;
            _context.Partners.Add(partner);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var partner=_context.Partners.FirstOrDefault(p => p.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

            _context.Partners.Remove(partner);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
       
    }
}
