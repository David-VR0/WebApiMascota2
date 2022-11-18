using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using WebApiMascota2.DTOs;
using WebApiMascota2.Entidades;
namespace WebApiMascota2.Controllers
{
    [ApiController]
    [Route("veterinarias/{veterinariaId:int}/servicio")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        public ServiciosController(ApplicationDbContext dbContext, IMapper mapper,UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServicioDTO>>> Get(int veterinariaId)
        {
            var existeVeterinaria = await dbContext.Veterinaria.AnyAsync(veterinariaDB => veterinariaDB.Id == veterinariaId);
            if (!existeVeterinaria)
            {
                return NotFound();
            }

            var servicio = await dbContext.Servicio.Where(servicioDB => servicioDB.VeterinariaId == veterinariaId).ToListAsync();

            return mapper.Map<List<ServicioDTO>>(servicio);
        }

        [HttpGet("{id:int}", Name = "obtenerServicio")]
        public async Task<ActionResult<ServicioDTO>> GetById(int id)
        {
            var servicio = await dbContext.Servicio.FirstOrDefaultAsync(servicioDB => servicioDB.Id == id);

            if (servicio == null)
            {
                return NotFound();
            }

            return mapper.Map<ServicioDTO>(servicio);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(int veterinariaId, ServicioCreacionDTO servicioCreacionDTO)
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var existeVet = await dbContext.Veterinaria.AnyAsync(vetDB => vetDB.Id == veterinariaId);
            if (!existeVet)
            {
                return NotFound();
            }
            
            var servicio = mapper.Map<Servicio>(servicioCreacionDTO);
            
            servicio.VeterinariaId = veterinariaId;
            servicio.UsuarioId = usuarioId;
            dbContext.Add(servicio);
            await dbContext.SaveChangesAsync();

            var servicioDTO = mapper.Map<ServicioDTO>(servicio);

            return CreatedAtRoute("obtenerServicio", new { id = servicio.Id, veterinariaId = veterinariaId }, servicioDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int veterinariaId, int id, ServicioCreacionDTO servicioCreacionDTO)
        {
            var existeVet = await dbContext.Veterinaria.AnyAsync(vetDB => vetDB.Id == veterinariaId);
            if (!existeVet)
            {
                return NotFound();
            }

            var existeServ = await dbContext.Servicio.AnyAsync(servicioDB => servicioDB.Id == id);
            if (!existeServ)
            {
                return NotFound();
            }
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var servicio = mapper.Map<Servicio>(servicioCreacionDTO);
            servicio.Id = id;
            servicio.VeterinariaId = veterinariaId;
            servicio.UsuarioId = usuarioId;
            dbContext.Update(servicio);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
