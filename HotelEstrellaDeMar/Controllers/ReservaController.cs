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


        public IActionResult Index(int id)
        {
            if (id == null || id == 0)
            {
                // Redirigir al login si no se pasa el ID
                return RedirectToAction("Index", "Login");
            }
            //List<Reserva> Reservas = new List<Reserva>();
            //Usuario ?usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email);

            // Guardar el ID en el ViewBag y listar las reservas
            ViewBag.IdUsuario = id;
            return View(ListarReservas(id));            
        }

        public IActionResult Create(int id)
        {            

            ViewBag.IdUsuario = id;
            Console.WriteLine("id en create" + id);
            ViewBag.Usuarios = _context.Usuarios;
            ViewBag.Hoy = DateTime.Today.ToString("yyyy-MM-dd"); // Tambien le paso el valor de la fecha de hoy para setearle como min al input CheckIn
            ViewBag.Manana = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"); // Lo mismo para la fecha de CheckOut
            return View();
        }

        public IActionResult Edit(int id, int idUsuario)
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
            ViewBag.IdUsuario = idUsuario;
            return View();
        }

/*        public IActionResult Remove()
        {
            return View();
        }*/

        public IActionResult CrearReserva(DateTime fechaCheckIn, DateTime fechaCheckOut, string tipoHab, int idUsuario)
        {
            Console.WriteLine($"idUsuario recibido: {idUsuario}");

            if (fechaCheckOut < fechaCheckIn)
            {
                TempData["ErrorFecha"] = "La fecha de Check Out debe ser posterior a la fecha de Check In";
                return RedirectToAction("Create", new { id = idUsuario });
            }
            if ((fechaCheckOut - fechaCheckIn).TotalDays > 30)
            {
                TempData["ErrorDuracion"] = "La reserva no puede ser mayor a 30 dias";
                return RedirectToAction("Create", new { id = idUsuario });
            }


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

                Reserva? reserva = _context.Reservas
                    .Include(reserva => reserva.Habitacion)
                    .Include(reserva => reserva.Usuario)
                    .FirstOrDefault(reserva => reserva.IDReserva == newReserva.IDReserva);


                ViewBag.Reserva = reserva;
                ViewBag.IdUsuario = idUsuario;
                return View("ReservaCreadaOModificadaExitosa", new { id = idUsuario });
            }
            else
            {
                TempData["Error"] = $"No hay habitaciones del tipo {tipoHab} disponibles para el rango de fechas seleccionado"; // Usando metodo TempData me permite mantener esta info por mas que se recargue la view
                //ViewBag.Error = TempData["Error"];
                //return View(ListarReservas(idUsuario));
                return RedirectToAction("Create", new { id = idUsuario });
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
            return View("Index", new { id = idUsuario });
        }

        public IActionResult EditarReserva(int idReserva, DateTime fechaCheckIn, DateTime fechaCheckOut, string tipoHab, int idUsuario)
        {
            Console.WriteLine($"ID Usuario recibido en EditarReserva: {idUsuario}");
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
                ViewBag.IdUsuario = idUsuario;
                return View(ListarReservas(idUsuario)); //** recordar pasarle id por lo que venga de la web, por ahora lo estoy pasando a mano desde el metodo Index de acá 
            }

            reserva.FechaCheckIn = fechaCheckIn;
            reserva.FechaCheckOut = fechaCheckOut;
            reserva.HabitacionId = habitacionSeleccionada.Id;

            _context.SaveChanges();

            ViewBag.IdUsuario = idUsuario;
            ViewBag.Reserva = reserva;
            return View("ReservaCreadaOModificadaExitosa"/*, new { id = idUsuario }*/);
        }

        public IActionResult EliminarReserva(int id, int idUsuario)
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

            ViewBag.idUsuario = idUsuario;
            return View("BorradoExitoso");
        }
    }
}
