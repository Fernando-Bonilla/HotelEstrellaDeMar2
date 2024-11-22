using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelEstrellaDeMar.Models
{
    public class Reserva
    {
        
        #region Propiedades
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDReserva { get; set; }

      /*  [Required]
        public int NumHabitacion { get; set; }*/

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaCheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaCheckOut { get; set; }

        [DataType(DataType.Date)]        
        public DateTime FechaReserva { get; set; } = DateTime.Today;

        #endregion

        //Relacion con la tabla Habitacion
        [ForeignKey("Habitacion")]
        public int HabitacionId {  get; set; }     
        public Habitacion? Habitacion { get; set; }

        //Relacion con la tabla Usuario
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        #region Constructores
        public Reserva()
        {
            
        }
        public Reserva(DateTime fechaCheckIn, DateTime fechaCheckOut, int idHabitacion, int idUsuario)
        {
            FechaCheckIn = fechaCheckIn;
            FechaCheckOut = fechaCheckOut;
            HabitacionId = idHabitacion;
            IdUsuario = idUsuario;            
        }
        #endregion Constructores

    }
}
