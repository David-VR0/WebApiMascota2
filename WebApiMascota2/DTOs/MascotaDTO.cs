using System.ComponentModel.DataAnnotations;
using WebApiMascota2.Validaciones;
namespace WebApiMascota2.DTOs
{
    public class MascotaDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
