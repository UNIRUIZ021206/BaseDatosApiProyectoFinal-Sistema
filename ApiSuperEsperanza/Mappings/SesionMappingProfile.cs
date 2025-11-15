using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class SesionMappingProfile : Profile
    {
        public SesionMappingProfile()
        {
            CreateMap<Sesion, SesionDto>();

            CreateMap<SesionCreateDto, Sesion>()
                .ForMember(dest => dest.Id_Sesion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioNombre, opt => opt.Ignore())
                .ForMember(dest => dest.FechaApertura, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCierre, opt => opt.Ignore())
                .ForMember(dest => dest.MontoFinal, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());
        }
    }
}

