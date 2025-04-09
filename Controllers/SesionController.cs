﻿using Microsoft.AspNetCore.Mvc;
using Naitv1.Helpers;
using Naitv1.Models;
using Naitv1.Data;

namespace Naitv1.Controllers
{
    public class SesionController : Controller
    {
        private readonly AppDbContext _context;

        public SesionController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.estaLogueado = UsuarioLogueado.estaLogueado(HttpContext.Session);
            return View();
        }

        [HttpPost]
        public IActionResult Iniciar(string email, string password)
        {
            List<Usuario> resultado = _context.Usuarios.Where(
                usuario => usuario.Email == email && usuario.Password == MD5Libreria.Encriptar(password)
            ).ToList();

            if (resultado.Count > 0)
            {
                Usuario usuario = resultado.First();

                HttpContext.Session.SetString("estaLogueado", "true");
                HttpContext.Session.SetString("nombreUsuario", usuario.Nombre);
                HttpContext.Session.SetString("emailUsuario", usuario.Email);

                return Redirect("/");
            } else
            {
                return Redirect("/Sesion/ErrorDeInicio");
            }
        }

        public IActionResult Registrarse()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult ErrorDeRegistro()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult ErrorDeInicio()
        {
           if(UsuarioLogueado.estaLogueado(HttpContext.Session)) {
                return RedirectToAction("Index", "Home");
           }

           return View();
        }

        public IActionResult CuentaCreadaConExito()
        {
           if(UsuarioLogueado.estaLogueado(HttpContext.Session) == false) {
                return RedirectToAction("Index", "Home");
           }

           ViewBag.NombreUsuario = HttpContext.Session.GetString("nombreUsuario") ?? "";

           return View();
        }
        
        public IActionResult Salir()
        {
            if (UsuarioLogueado.estaLogueado(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult CrearUsuario(string nombre, string email, string password, string passwordConfirmation)
        {
            if(UsuarioLogueado.estaLogueado(HttpContext.Session)) {
                return RedirectToAction("Index", "Home");
            }

            if ( password == passwordConfirmation)
            {
                Usuario usuario = new Usuario();
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Password = MD5Libreria.Encriptar(password);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                HttpContext.Session.SetString("estaLogueado", "true");
                HttpContext.Session.SetString("nombreUsuario", nombre);
                HttpContext.Session.SetString("emailUsuario", email);

                return Redirect("/Sesion/CuentaCreadaConExito");
            } else
            {
                return Redirect("/Sesion/ErrorDeRegistro");
            }
        }
    }
}
