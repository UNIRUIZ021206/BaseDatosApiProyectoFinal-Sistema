using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.NombreRol, opt => opt.MapFrom(src => src.Rol));

            CreateMap<UsuarioCreateDto, Usuario>()
                .ForMember(dest => dest.Id_Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.CodigoUsuario))
                .ForMember(dest => dest.Clave, opt => opt.Ignore())
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.Id_Rol))
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioModificacion, opt => opt.Ignore());

            CreateMap<UsuarioUpdateDto, Usuario>()
                .ForMember(dest => dest.Id_Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.CodigoUsuario))
                .ForMember(dest => dest.Clave, opt => opt.Ignore())
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.Id_Rol))
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioModificacion, opt => opt.Ignore());
        }
    }
}

