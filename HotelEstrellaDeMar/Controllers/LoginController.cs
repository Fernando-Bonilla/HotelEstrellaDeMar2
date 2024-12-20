﻿using Microsoft.AspNetCore.Mvc;
using HotelEstrellaDeMar.Data;
using HotelEstrellaDeMar.Models;

namespace HotelEstrellaDeMar.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            // Buscar el usuario en la base de datos
            Usuario? usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email && usuario.Password == password);
            //Console.WriteLine("Id en el index de login controller" + usuario.Id);

            if (usuario != null)
            {
                // Redirigir al listado de reservas después del login exitoso
                Console.WriteLine(ViewBag.IdUsuario = usuario.Id);
                return RedirectToAction("Index", "Home", new { id = usuario.Id });
            }

            // Si las credenciales son incorrectas
            TempData["Error"] = "Correo o contraseña incorrectos.";
            return View();
        }
    }
}

