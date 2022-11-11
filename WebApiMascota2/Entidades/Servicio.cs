using Microsoft.AspNetCore.Identity;

namespace WebApiMascota2.Entidades
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Consulta { get; set; }

        public int VeterinariaId { get; set; }

        public Veterinaria Veterinaria { get; set; }

    }
}
