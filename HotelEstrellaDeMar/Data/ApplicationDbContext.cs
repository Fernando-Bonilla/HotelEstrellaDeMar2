using HotelEstrellaDeMar.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaDeMar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }

        //Recordar agregar aca las tablas a mapear en la BD
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Habitacion>()
                .HasKey(h => h.Id); 

            modelBuilder.Entity<Habitacion>()
                .HasMany(h => h.Reservas)
                .WithOne(r => r.Habitacion)
                .HasForeignKey(r => r.HabitacionId);
        }

    }
}
