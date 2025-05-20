using Microsoft.AspNetCore.Mvc;
using Naitv1.Data;
using Naitv1.Helpers;
using Naitv1.Models;


namespace Naitv1.Controllers
{
    public class PanelAdminController : Controller
    {
        private readonly ServicioDashboard _servicioDashboard;
        private readonly AppDbContext _context;



        public PanelAdminController(ServicioDashboard servicioDashboard, AppDbContext context)
        {
            _servicioDashboard = servicioDashboard;
            _context = context;
        }

        public IActionResult Index()
        {
            DashboardData datos = _servicioDashboard.ObtenerMetrics();
            return View("Dashboard", datos); 
        }

        [HttpGet]
        public IActionResult Filtro()
        {
            var ciudades = _servicioDashboard.ObtenerTodasLasCiudades();
            ViewBag.Ciudades = ciudades;

            return View();
        }

        [HttpPost]
        public IActionResult ResultadosFiltro(FiltroDashboard filtro)
        {
            ViewBag.esAdmin = UsuarioLogueado.esAdmin(HttpContext.Session);

            try
            {
                var data = _servicioDashboard.ObtenerMetricsFiltrado(filtro.Ciudad, filtro.FechaInicio, filtro.FechaFin);

                if (data.PorHora.Sum(x => x.Cantidad) == 0 && data.PorCiudad.Sum(x => x.Cantidad) == 0)
                {
                    ViewBag.Mensaje = "No hay datos para este filtro";
                }

                ViewBag.Ciudad = filtro.Ciudad;
                ViewBag.FechaDesde = filtro.FechaInicio.ToString("yyyy-MM-dd");
                ViewBag.FechaHasta = filtro.FechaFin.ToString("yyyy-MM-dd");


                Actividad actividad = new Actividad();

                bool estaLogueado = UsuarioLogueado.estaLogueado(HttpContext.Session);

                ViewBag.estaLogueado = estaLogueado;

                if (estaLogueado)
                {
                    ViewBag.esAdmin = UsuarioLogueado.esAdmin(HttpContext.Session);

                    if (UsuarioLogueado.esAdmin(HttpContext.Session))
                    {
                        List<Actividad> actividadesCiudadFiltro = _context.Actividades
                        .Where(a => a.Ciudad == filtro.Ciudad)
                        .ToList();

                        ViewBag.actividades = actividadesCiudadFiltro;
                    }
                }

                return View("ResultadosFiltro", data); 
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Ciudades = _servicioDashboard.ObtenerTodasLasCiudades(); 
                return View("Filtro");
            }
        }

    }
}