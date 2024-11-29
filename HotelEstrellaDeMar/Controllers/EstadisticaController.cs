using HotelEstrellaDeMar.Data;
using HotelEstrellaDeMar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelEstrellaDeMar.Controllers
{
    public class EstadisticaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadisticaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Historial = HistorialReservasPorTipoHabitacion();
            return View();
        }

        public Dictionary<string, int> HistorialReservasPorTipoHabitacion()
        {
            List<Reserva> reservas = _context.Reservas
                .Include(reserva => reserva.Habitacion)
                .ToList(); //Esto carga todas las reservas y sus habitaciones relacionadas desde la base de datos en una lista en memoria.


            Dictionary<string, int> historial = reservas
                .GroupBy(reserva => reserva.Habitacion.TipoHabitacion) //Agrupa las reservas por el tipo de habitación (propiedad TipoHabitacion de la entidad Habitacion).
                .ToDictionary(
                    tipo => tipo.Key,       //Convierte cada grupo en una entrada del diccionario: Tipo de habitación
                    tipo => tipo.Count()   // Cantidad de reservas
                );

            return historial;
        }
    }
}
