using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMascota2.DTOs;
using WebApiMascota2.Entidades;
namespace WebApiMascota2.Controllers
{
    [ApiController]
    [Route("veterinarias")]
    public class VeterinariaController:ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public VeterinariaController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("/listadoClase")]
        public async Task<ActionResult<List<Veterinaria>>> GetAll()
        {
            return await dbContext.Veterinaria.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerVeterinaria")]
        public async Task<ActionResult<VeterinariaDTOConMascotas>> GetById(int id)
        {
            var veterinaria = await dbContext.Veterinaria
                .Include(mascotaDB => mascotaDB.MascotaVeterinaria)
                .ThenInclude(mascotaVeterinariaDB => mascotaVeterinariaDB.Mascota)
                .Include(servicioDB => servicioDB.Servicio)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (veterinaria == null)
            {
                return NotFound();
            }

            veterinaria.MascotaVeterinaria = veterinaria.MascotaVeterinaria.OrderBy(x => x.Orden).ToList();

            return mapper.Map<VeterinariaDTOConMascotas>(veterinaria);
        }

        [HttpPost]
        public async Task<ActionResult> Post(VeterinariaCreacionDTO veterinariaCreacionDTO)
        {

            if (veterinariaCreacionDTO.MascotasIds == null)
            {
                return BadRequest("No se puede crear una veterinaria sin mascotas.");
            }

            var mascotasIds = await dbContext.Mascota
                .Where(mascotaBD => veterinariaCreacionDTO.MascotasIds.Contains(mascotaBD.Id)).Select(x => x.Id).ToListAsync();

            if (veterinariaCreacionDTO.MascotasIds.Count != mascotasIds.Count)
            {
                return BadRequest("No existe uno de los alumnos enviados");
            }

            var veterinaria = mapper.Map<Veterinaria>(veterinariaCreacionDTO);

            OrdenarPorAlumnos(veterinaria);

            dbContext.Add(veterinaria);
            await dbContext.SaveChangesAsync();

            var veterinariaDTO = mapper.Map<VeterinariaDTO>(veterinaria);

            return CreatedAtRoute("obtenerVeterinaria", new { id = veterinaria.Id }, veterinariaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, VeterinariaCreacionDTO veterinariaCreacionDTO)
        {
            var veterinariaDB = await dbContext.Veterinaria
                .Include(x => x.MascotaVeterinaria)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (veterinariaDB == null)
            {
                return NotFound();
            }

            veterinariaDB = mapper.Map(veterinariaCreacionDTO, veterinariaDB);

            OrdenarPorMascotas(veterinariaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Veterinaria.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Veterinaria { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        private void OrdenarPorMascotas(Veterinaria veterinaria)
        {
            if (veterinaria.MascotaVeterinaria != null)
            {
                for (int i = 0; i < veterinaria.MascotaVeterinaria.Count; i++)
                {
                    veterinaria.MascotaVeterinaria[i].Orden = i;
                }
            }
        }
    }
}
