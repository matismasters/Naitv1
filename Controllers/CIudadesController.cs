using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Naitv1.Data;
using Naitv1.Models;

namespace Naitv1.Controllers
{
    [Route ("Ciudades/[action]")]
    public class CiudadesController : Controller
    {
        private readonly AppDbContext _context;

        public CiudadesController (AppDbContext context)
        {
            _context = context;
        }
       public IActionResult InsercionManual()
        {
            var ciudades = new[]
                {
                new Ciudades { Nombre = "Colonia del Sacramento", Departamento = "Colonia", Pais = "Uruguay", CodigoPostal  = 70000 },
                new Ciudades { Nombre = "Nueva Helvecia", Departamento = "Colonia", Pais = "Uruguay", CodigoPostal = 70200 },
                new Ciudades { Nombre = "Carmelo", Departamento = "Colonia", Pais = "Uruguay", CodigoPostal = 70100 },
                new Ciudades { Nombre = "Rosario", Departamento = "Colonia", Pais = "Uruguay", CodigoPostal = 70200 },
                new Ciudades { Nombre = "Juan Lacaze", Departamento = "Colonia", Pais = "Uruguay", CodigoPostal = 70001 },
                new Ciudades { Nombre = "Montevideo", Departamento = "Montevideo", Pais = "Uruguay", CodigoPostal = 11000 },
                new Ciudades { Nombre = "Punta del Este", Departamento = "Maldonado", Pais = "Uruguay", CodigoPostal = 20100 },
                new Ciudades { Nombre = "Paysandú", Departamento = "Paysandú", Pais = "Uruguay", CodigoPostal = 60000 },
                new Ciudades { Nombre = "Salto", Departamento = "Salto", Pais = "Uruguay", CodigoPostal = 50000 },
                new Ciudades { Nombre = "Durazno", Departamento = "Durazno", Pais = "Uruguay", CodigoPostal = 97000 }
                };

            _context.Ciudades.AddRange(ciudades);
            _context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }
    }
}
