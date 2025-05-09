using Microsoft.AspNetCore.Mvc;
using Naitv1.Data;

namespace Naitv1.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notificaciones()
        {
            return View();
        }
    }
}
