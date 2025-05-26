using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Naitv1.Data;
using Naitv1.Models;
using Naitv1.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Naitv1.Controllers
{
    public class PanelAdminController : Controller
    {        
        private readonly AppDbContext _context;
        private readonly ServicioDashboard _servicioDashboard;

        public PanelAdminController(AppDbContext context, ServicioDashboard servicioDashboard)
        {            
            _context = context;
            _servicioDashboard = servicioDashboard;
        }

        /*[Authorize(Roles = "Admin")]*/
        public async Task<ActionResult> Index()
        {
            var actividadesPorHora = await _servicioDashboard.ObtenerMetrics();
            ViewBag.ActividadesPorHora = actividadesPorHora;

            var datos = await _servicioDashboard.ObtenerMetrics();

            ViewBag.ActividadesPorHora = datos.ActividadesPorHora;
            ViewBag.ActividadesPorCiudad = datos.ActividadesPorCiudad;


            return View();
        }

        /*[HttpGet]
        public async Task<IActionResult> ActividadesActivas()
        {
            var actividades = await _servicioDashboard.ActividadesActivas();
            return Json(actividades);
        }*/
    }
}
