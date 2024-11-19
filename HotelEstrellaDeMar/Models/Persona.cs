using System.ComponentModel.DataAnnotations.Schema;

namespace HotelEstrellaDeMar.Models
{
    public class Persona
    {
        #region Propiedades
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? TipoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public string? Email { get; set; }
        #endregion Propiedades
    }
}
