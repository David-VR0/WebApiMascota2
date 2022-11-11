namespace WebApiMascota2.DTOs
{
    public class VeterinariaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }
        public List<ServicioDTO> Servicios { get; set; }
    }
}
