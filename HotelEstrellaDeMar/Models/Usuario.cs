using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelEstrellaDeMar.Models
{
    public class Usuario
    {
        #region Propiedades
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El Apellido no puede superar los 50 caracteres")]
        public string? Apellido { get; set; }

        [Required]
        public string? TipoDocumento { get; set; }
        [Required]

        public int NumeroDocumento { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public int Telefono { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Por favor ingrese un email válido")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        //public string? ConfirmPassword { get; set; } No voy a implementar la creacion de nuevos usuarios 
        #endregion Propiedades

        //Relaciones
        public ICollection<Reserva> ?Reservas { get; set; }

        #region Constructores
        public Usuario()
        {
            
        }

        public Usuario(string nombre, string apellido, string tipoDocumento, int numeroDoc, DateTime fechaNac, int telefono, string email, string password )
        {
            Nombre = nombre;
            Apellido = apellido;
            TipoDocumento = tipoDocumento;
            FechaNacimiento = fechaNac;
            Telefono = telefono;
            Email = email;
            Password = password;
        }
        #endregion Constructores
    }
}
