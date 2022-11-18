using AutoMapper;
using WebApiMascota2.DTOs;
using WebApiMascota2.Entidades;
namespace WebApiMascota2.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<MascotaDTO, Mascota>();
            CreateMap<Mascota, GetMascotaDTO>();
            CreateMap<Mascota, MascotaDTOConVeterinaria>()
                .ForMember(mascotaDTO => mascotaDTO.Veterinarias, opciones => opciones.MapFrom(MapMascotaDTOVeterinarias));
            CreateMap<VeterinariaCreacionDTO, Veterinaria>()
                .ForMember(veterinaria => veterinaria.MascotaVeterinaria, opciones => opciones.MapFrom(MapMascotaVeterinaria));
            CreateMap<Veterinaria, VeterinariaDTO>();
            CreateMap<Veterinaria, VeterinariaDTOConMascotas>()
                .ForMember(veterinariaDTO => veterinariaDTO.Mascotas, opciones => opciones.MapFrom(MapVeterinariasDTOMascotas));
            CreateMap<VeterinariaPatchDTO, Veterinaria>().ReverseMap();
            CreateMap<ServicioCreacionDTO, Servicio>();
            CreateMap<Servicio, ServicioDTO>();
        }

        private List<VeterinariaDTO> MapMascotaDTOVeterinarias(Mascota mascota, GetMascotaDTO getMascotaDTO)
        {
            var result = new List<VeterinariaDTO>();

            if (mascota.MascotaVeterinaria == null) { return result; }

            foreach (var mascotaVeterinaria in mascota.MascotaVeterinaria)
            {
                result.Add(new VeterinariaDTO()
                {
                    Id = mascotaVeterinaria.VeterinariaId,
                    Nombre = mascotaVeterinaria.Veterinaria.Nombre
                });
            }

            return result;
        }

        private List<GetMascotaDTO> MapVeterinariasDTOMascotas(Veterinaria veterinaria, VeterinariaDTO veterinariaDTO)
        {
            var result = new List<GetMascotaDTO>();

            if (veterinaria.MascotaVeterinaria == null)
            {
                return result;
            }

            foreach (var mascotaVeterinaria in veterinaria.MascotaVeterinaria)
            {
                result.Add(new GetMascotaDTO()
                {
                    Id = mascotaVeterinaria.MascotaId,
                    Nombre = mascotaVeterinaria.Mascota.Nombre
                });
            }

            return result;
        }

        private List<MascotaVeterinaria> MapMascotaVeterinaria(VeterinariaCreacionDTO veterinariaCreacionDTO, Veterinaria veterinaria)
        {
            var resultado = new List<MascotaVeterinaria>();

            if (veterinariaCreacionDTO.MascotasIds == null) { return resultado; }
            foreach (var mascotaId in veterinariaCreacionDTO.MascotasIds)
            {
                resultado.Add(new MascotaVeterinaria() { MascotaId = mascotaId });
            }
            return resultado;
        }
    }
}

