using System.ComponentModel.DataAnnotations;
using WebApiMascota2.Validaciones;
namespace WebApiMascota2.DTOs
{
    public class VeterinariaCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<int> MascotasIds { get; set; }
    }
}
