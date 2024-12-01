using HotelEstrellaDeMar.Data;
using HotelEstrellaDeMar.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configuracion de la conexion a SQLServer
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Precarga de datos directamente en Program
using (var scope = app.Services.CreateScope())
{
    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if ((_context.Reservas.Count() == 0 || _context.Usuarios.Count() == 0 || _context.Habitaciones.Count() == 0))
    {
        _context.Habitaciones.Add(new Habitacion(101, "Simple", 2));
        _context.Habitaciones.Add(new Habitacion(102, "Simple", 2));
        _context.Habitaciones.Add(new Habitacion(202, "Doble", 4));
        _context.Habitaciones.Add(new Habitacion(203, "Doble", 4));
        _context.Habitaciones.Add(new Habitacion(204, "Doble", 4));
        _context.Habitaciones.Add(new Habitacion(205, "Doble", 4));
        _context.Habitaciones.Add(new Habitacion(206, "Doble", 4));
        _context.Habitaciones.Add(new Habitacion(207, "Doble", 4));
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

        int HabitacionIdTres = _context.Habitaciones.Skip(2).FirstOrDefault().Id;
        int usuarioIdTres = _context.Usuarios.Skip(1).FirstOrDefault().Id; // Seleccionando el usuario con id = 2                
        _context.Reservas.Add(new Reserva(new DateTime(2025, 08, 01), new DateTime(2025, 08, 09), HabitacionIdTres, usuarioIdTres));

        _context.SaveChanges();
    }
}

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // La app inicia en LoginController, acción Index

app.Run();
