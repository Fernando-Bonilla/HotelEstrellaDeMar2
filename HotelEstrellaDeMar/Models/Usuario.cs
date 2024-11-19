using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelEstrellaDeMar.Models
{
    public class Usuario
    {
        #region Propiedades
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? TipoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        #endregion Propiedades

        public ICollection<Reserva> ?Reservas { get; set; }
    }
}
