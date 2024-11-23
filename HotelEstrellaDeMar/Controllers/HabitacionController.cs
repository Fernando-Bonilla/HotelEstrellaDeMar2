using HotelEstrellaDeMar.Data;
using Microsoft.AspNetCore.Mvc;
using HotelEstrellaDeMar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaDeMar.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HabitacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.HabitacionesDisponibles = new List<Habitacion>();
            return View();
        }

        [HttpPost]
        public IActionResult MostrarHabDisponibles(DateTime fechaCheckIn, DateTime fechaCheckOut, string tipoHab)
        {
            List<Habitacion> HabitacionesDisponibles = new List<Habitacion>();
            List<Habitacion> HabitacionesPorTipo;
            //List<Reserva> Reservas;

            //Selecciono todas las habitaciones que tengan el mismo tipo de habitacion que ingresa el usuario
            HabitacionesPorTipo = _context.Habitaciones
                .Where(habitacion => habitacion.TipoHabitacion == tipoHab)
                .Include(habitacion => habitacion.Reservas)
                .ToList();

            foreach(Habitacion habitacion in HabitacionesPorTipo)
            {
                if(habitacion.EstaDisponible(fechaCheckIn, fechaCheckOut))
                {
                    HabitacionesDisponibles.Add(habitacion);
                }
            }
            /*
                        Reservas = _context.Reservas.ToList();

                        HabitacionesDisponibles = HabitacionesPorTipo.Where(habitacion => 
                            !Reservas.Any(reserva => reserva.HabitacionId == habitacion.Id 
                            && fechaCheckIn <= reserva.FechaCheckOut
                            || reserva.HabitacionId == habitacion.Id && fechaCheckOut <= reserva.FechaCheckOut 
                            && reserva.HabitacionId == habitacion.Id && fechaCheckOut >= reserva.FechaCheckIn 
                            || reserva.HabitacionId == habitacion.Id && fechaCheckIn <= reserva.FechaCheckOut 
                            && fechaCheckOut >= reserva.FechaCheckIn)).ToList();*/


            /*var reservaEspecifica = _context.Reservas.FirstOrDefault(reserva => reserva.IDReserva == 1);
            Console.WriteLine(reservaEspecifica.FechaCheckIn);
            Console.WriteLine("Hola");
            Console.WriteLine($"Fecha CheckIn: {fechaCheckIn}, Fecha CheckOut: {fechaCheckOut}, Tipo de Habitación: {tipoHab}");
            Console.WriteLine(HabitacionesDisponibles.Count());*/

            //Console.WriteLine(HabitacionesDisponibles.Count());

            ViewBag.HabitacionesDisponibles = HabitacionesDisponibles;
            
            return View("Index");

        }

    }
}
