using HotelEstrellaDeMar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelEstrellaDeMar.Models;
using System.Security.Cryptography.X509Certificates;

namespace HotelEstrellaDeMar.Controllers
{    
    public class ReservaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            //List<Reserva> Reservas = new List<Reserva>();
            return View(ListarReservas(2));
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Remove()
        {
            return View();
        }

        public IActionResult CrearReserva(DateTime fechaCheckIn, DateTime fechaCheckOut, string tipoHab, int idUsuario)/*int idUsuario*/
        {
            List<Reserva> Reservas = new List<Reserva>();
            List<Habitacion> HabitacionesPorTipo = _context.Habitaciones
                .Where(habitacion => habitacion.TipoHabitacion == tipoHab)
                .Include(habitacion => habitacion.Reservas)
                .ToList();

            List<Habitacion> HabitacionesDisponibles = new List<Habitacion>();


            if (HabitacionesPorTipo.Count() > 0)
            {
                foreach(Habitacion habitacion in HabitacionesPorTipo)
                {
                    if (habitacion.EstaDisponible(fechaCheckIn, fechaCheckOut))
                    {
                        HabitacionesDisponibles.Add(habitacion);                        
                    }
                }            
            }

            if(HabitacionesDisponibles.Count > 0)
            {
                Reserva newReserva = new Reserva(fechaCheckIn, fechaCheckOut, HabitacionesDisponibles[0].Id, idUsuario); //Recordar pasarle el id por usuario                                                                                                                 
                _context.Reservas.Add(newReserva);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }else
            {
                ViewBag.Message = "No hay habitaciones disponibles para el rango de fechas";
                return View(ListarReservas(idUsuario));
            }       
            
        }

        public IActionResult ListarReservas(int idUsuario)
        {
            List<Reserva> Reservas = _context.Reservas
                .Where(reserva => reserva.IdUsuario == idUsuario && reserva.FechaCheckIn > DateTime.Today)
                .Include(reserva => reserva.Habitacion)
                .Include(reserva => reserva.Usuario)
                .ToList(); // recordar pasarle id por lo que venga de la web

            ViewBag.Reservas = Reservas;
            return View("Index");
        }

    }
}
