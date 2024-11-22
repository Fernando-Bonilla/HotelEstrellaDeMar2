using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelEstrellaDeMar.Models
{

    public class Habitacion
    {
        #region Propiedades
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NumHabitacion { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El tipo de Habitacion no puede exceder los 50 caracteres")]
        public string? TipoHabitacion { get; set; }
        [Required]
        [Range(1, 6)]
        public int CapacidadHabitacion { get; set; }

        #endregion Propiedades

        //Referencias
        public ICollection<Reserva> ?Reservas { get; set; }

        #region Constructores
        public Habitacion()
        {
            
        }
        public Habitacion(int numHab, string tipoHab, int capacidad)
        {
            NumHabitacion = numHab;
            TipoHabitacion = tipoHab;
            CapacidadHabitacion = capacidad;
        }
        #endregion Constructores
    }
}
