using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiMascota2.Entidades;

namespace WebApiMascota2
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MascotaVeterinaria>()
                .HasKey(al => new { al.MascotaId, al.VeterinariaId });
        }

        public DbSet<Mascota> Mascota { get; set; }
        public DbSet<Veterinaria> Veterinaria { get; set; }
        public DbSet<Servicio> Servicio { get; set; }

        public DbSet<MascotaVeterinaria> MascotaVeterinaria { get; set; }
    }
}
