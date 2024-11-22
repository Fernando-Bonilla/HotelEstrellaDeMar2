using HotelEstrellaDeMar.Data;
using HotelEstrellaDeMar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelEstrellaDeMar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Metodo para agregar datos a la base de datos para tener para testear
        public IActionResult PreCargaDatos() 
        {
            if(_context.Reservas.Count() == 0 || _context.Usuarios.Count() == 0 || _context.Habitaciones.Count() == 0)
            {
                _context.Habitaciones.Add(new Habitacion(101, "Simple", 2));
                _context.Habitaciones.Add(new Habitacion(102, "Simple", 2));
                _context.Habitaciones.Add(new Habitacion(202, "Doble", 4));
                _context.Habitaciones.Add(new Habitacion(203, "Doble", 4));
                _context.Habitaciones.Add(new Habitacion(301, "Suite", 6));

                _context.Usuarios.Add(new Usuario("Rodrigo", "Santana", "CI", 45268136, new DateTime(1987, 05, 01), 099679788, "asd@gmail.com", "123"));
                _context.Usuarios.Add(new Usuario("Juan", "Perez", "CI", 45268139, new DateTime(1991, 08, 11), 099679844, "bebo@gmail.com", "456"));

                _context.SaveChanges();

                int HabitacionId = _context.Habitaciones.FirstOrDefault().Id;
                int usuarioId = _context.Usuarios.FirstOrDefault().Id; // Seleccionando el usuario con id = 1           
                _context.Reservas.Add(new Reserva(new DateTime(2025, 08, 11), new DateTime(2025, 08, 13), HabitacionId, usuarioId));

                int HabitacionIdDos = _context.Habitaciones.Skip(1).FirstOrDefault().Id;
                int usuarioIdDos = _context.Usuarios.Skip(1).FirstOrDefault().Id; // Seleccionando el usuario con id = 2                
                _context.Reservas.Add(new Reserva(new DateTime(2025, 01, 07), new DateTime(2025, 01, 09), HabitacionIdDos, usuarioId));

                /*int usuarioIdTres = _context.Usuarios.Skip(1).FirstOrDefault().Id; // Seleccionando el usuario con id = 2
                _context.Reservas.Add(new Reserva(new DateTime(2025, 01, 07), new DateTime(2025, 01, 09), 202, usuarioIdTres));*/

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
