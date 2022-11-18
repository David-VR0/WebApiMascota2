using System.ComponentModel.DataAnnotations;

namespace WebApiMascota2.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
