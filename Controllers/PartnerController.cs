using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    public class PartnerController : Controller
    {

        private readonly AppDbContext _context;
        public PartnerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Index(string nombre, string? direccion, string logoUrl, string? descripcion)
        {
            Usuario usuario = UsuarioLogueado.Usuario(HttpContext.Session);
            Partner partner = new Partner();

            partner.Nombre = nombre;
            partner.Direccion = direccion;
            partner.LogoUrl = logoUrl;
            partner.Descripcion = descripcion;
            partner.EstadoPartner = true;

            _context.Partners.Add(partner);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
       
    }
}
