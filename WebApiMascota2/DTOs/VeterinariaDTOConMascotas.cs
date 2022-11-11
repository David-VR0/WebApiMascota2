namespace WebApiMascota2.DTOs
{
    public class VeterinariaDTOConMascotas: VeterinariaDTO
    {
        public List<GetMascotaDTO> Mascotas { get; set; }
    }
}
