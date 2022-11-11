using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMascota2.DTOs;
using WebApiMascota2.Entidades;
namespace WebApiMascota2.Controllers
{
    [ApiController]
    [Route("mascotas")]
    public class MascotasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public MascotasController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetMascotaDTO>>> Get()
        {
            var mascota = await dbContext.Mascota.ToListAsync();
            return mapper.Map<List<GetMascotaDTO>>(mascota);
        }


        [HttpGet("{id:int}", Name = "obtenermascota")] 
        public async Task<ActionResult<MascotaDTOConVeterinaria>> Get(int id)
        {
            var mascota = await dbContext.Mascota
                .Include(mascotaDB => mascotaDB.MascotaVeterinaria)
                .ThenInclude(mascotaVeterinariaDB => mascotaVeterinariaDB.Veterinaria)
                .FirstOrDefaultAsync(mascotaDB => mascotaDB.Id == id);

            if (mascota == null)
            {
                return NotFound();
            }

            return mapper.Map<MascotaDTOConVeterinaria>(mascota);

        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetMascotaDTO>>> Get([FromRoute] string nombre)
        {
            var mascotas = await dbContext.Mascota.Where(mascotaDB => mascotaDB.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<GetMascotaDTO>>(mascotas);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MascotaDTO mascotaDto)
        {

            var existeMascotaMismoNombre = await dbContext.Mascota.AnyAsync(x => x.Nombre == mascotaDto.Nombre);

            if (existeMascotaMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {mascotaDto.Nombre}");
            }

            var mascota = mapper.Map<Mascota>(mascotaDto);

            dbContext.Add(mascota);
            await dbContext.SaveChangesAsync();

            var mascotaDTO = mapper.Map<GetMascotaDTO>(mascota);

            return CreatedAtRoute("obtenerMascota", new { id = mascota.Id }, mascotaDTO);
        }

        [HttpPut("{id:int}")] // api/mascotas/1
        public async Task<ActionResult> Put(MascotaDTO mascotaCreacionDTO, int id)
        {
            var exist = await dbContext.Mascota.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var mascota = mapper.Map<Mascota>(mascotaCreacionDTO);
            mascota.Id = id;

            dbContext.Update(mascota);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Mascota.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Mascota()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
