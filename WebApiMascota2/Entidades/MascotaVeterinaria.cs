namespace WebApiMascota2.Entidades
{
    public class MascotaVeterinaria
    {
        public int MascotaId { get; set; }
        public int VeterinariaId { get; set; }
        public int Orden { get; set; }
        public Mascota Mascota { get; set; }
        public Veterinaria Veterinaria { get; set; }
    }
}
