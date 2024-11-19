using System.ComponentModel.DataAnnotations;

namespace HotelEstrellaDeMar.Models
{

    public class Habitacion
    {
        #region Propiedades
        [Key]
        public int NumHabitacion { get; set; }
        public string? TipoHabitacion { get; set; }
        public int CapacidadHabitacion { get; set; }

        #endregion Propiedades

        public ICollection<Reserva> ?Reservas { get; set; }
    }
}
