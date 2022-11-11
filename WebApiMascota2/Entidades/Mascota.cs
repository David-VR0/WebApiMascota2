using System.ComponentModel.DataAnnotations;
using WebApiMascota2.Validaciones;

namespace WebApiMascota2.Entidades
{
    public class Mascota
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public List<MascotaVeterinaria> MascotaVeterinaria { get; set; }
    }
}
