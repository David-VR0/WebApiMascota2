namespace WebApiMascota2.DTOs
{
    public class MascotaDTOConVeterinaria: GetMascotaDTO
    {
        public List<VeterinariaDTO> Veterinarias { get; set; }
    }
}
