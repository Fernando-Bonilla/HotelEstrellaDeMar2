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
            return View(ListarReservas(6)); //** recordar pasarle id por lo que venga de la web, por ahora lo estoy pasando a mano desde el metodo Index de acá
        }

        public IActionResult Create()
        {
            ViewBag.Usuarios = _context.Usuarios;
            return View();
        }

        public IActionResult Edit(int id)
        {
            Reserva reserva = _context.Reservas                
                .Include(reserva => reserva.Habitacion)
                .FirstOrDefault(reserva => reserva.IDReserva == id);

            if (reserva == null)
            {
                return NotFound();
            }

            Console.WriteLine("Edit metodo Holaa");            
            ViewBag.Reserva = reserva;  
            return View();
        }

/*        public IActionResult Remove()
        {
            return View();
        }*/

        public IActionResult CrearReserva(DateTime fechaCheckIn, DateTime fechaCheckOut, string tipoHab, int idUsuario)
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
                Reserva newReserva = new Reserva(fechaCheckIn, fechaCheckOut, HabitacionesDisponibles[0].Id, idUsuario);                                                                                                               
                _context.Reservas.Add(newReserva);
                _context.SaveChanges();
                //return RedirectToAction("Index");

                Reserva? reservita = _context.Reservas
                    .Include(reserva => reserva.Habitacion)
                    .Include(reserva => reserva.Usuario)
                    .FirstOrDefault(reserva => reserva.IDReserva == newReserva.IDReserva);


                ViewBag.Reserva = reservita;                
                return View("ReservaCreadaOModificadaExitosa");
            }
            else
            {
                TempData["Error"] = $"No hay habitaciones del tipo {tipoHab} disponibles para el rango de fechas seleccionado"; // Usando metodo TempData me permite mantener esta info por mas que se recargue la view
                //ViewBag.Error = TempData["Error"];
                //return View(ListarReservas(idUsuario));
                return RedirectToAction("Create");
            }       
            
        }

        public IActionResult ListarReservas(int idUsuario)
        {
            List<Reserva> Reservas = _context.Reservas
                .Where(reserva => reserva.IdUsuario == idUsuario && reserva.FechaCheckIn > DateTime.Today)
                .Include(reserva => reserva.Habitacion)
                .Include(reserva => reserva.Usuario)
                .ToList(); 

            ViewBag.Reservas = Reservas;
            return View("Index");
        }

        public IActionResult EditarReserva(int idReserva, DateTime fechaCheckIn, DateTime fechaCheckOut, string tipoHab)
        {
            Console.WriteLine(tipoHab);
            Console.WriteLine(idReserva);
            Reserva reserva = _context.Reservas
                .Include(reserva => reserva.Habitacion)
                .Include(reserva =>reserva.Usuario)
                .FirstOrDefault(reserva => reserva.IDReserva == idReserva);

            if (reserva == null)
            {
                Console.WriteLine("Entra a reserva == null");
                return NotFound();
            }
           
            List <Habitacion> HabitacionesPorTipo = _context.Habitaciones
                .Where(habitacion => habitacion.TipoHabitacion == tipoHab)
                .Include(habitacion => habitacion.Reservas)
                .ToList();

            List<Habitacion> HabitacionesDisponibles = new List<Habitacion>();
            Habitacion habitacionSeleccionada = new Habitacion();

            if (HabitacionesPorTipo.Count() > 0)
            {
                foreach (Habitacion habitacion in HabitacionesPorTipo)
                {
                    if (habitacion.EstaDisponible(fechaCheckIn, fechaCheckOut))
                    {
                        HabitacionesDisponibles.Add(habitacion);
                    }
                }
            }

            if (HabitacionesDisponibles.Count > 0)
            {
                habitacionSeleccionada = HabitacionesDisponibles[0];
            }
            else
            {
                Console.WriteLine("Entra a HabitacionesDisponibles == 0");
                ViewBag.Message = "No hay habitaciones disponibles para el rango de fechas";
                return View(ListarReservas(6)); //** recordar pasarle id por lo que venga de la web, por ahora lo estoy pasando a mano desde el metodo Index de acá 
            }

            reserva.FechaCheckIn = fechaCheckIn;
            reserva.FechaCheckOut = fechaCheckOut;
            reserva.HabitacionId = habitacionSeleccionada.Id;

            _context.SaveChanges();

            ViewBag.Reserva = reserva;
            return View("ReservaCreadaOModificadaExitosa");
        }

        public IActionResult EliminarReserva(int id)
        {
            Console.WriteLine("id Reserva: " + id);
            Reserva reserva = _context.Reservas
                .Include(reserva => reserva.Usuario)
                .Include(reserva => reserva.Habitacion)
                .FirstOrDefault(reserva => reserva.IDReserva == id);

            if (reserva == null) 
            {
                Console.WriteLine("la reserva es null gil");  
            }

            if (reserva != null)
            {
                ViewBag.Reserva = reserva;

                _context.Reservas.Remove(reserva);
                _context.SaveChanges();
            }

            
            return View("BorradoExitoso");
        }
    }
}
