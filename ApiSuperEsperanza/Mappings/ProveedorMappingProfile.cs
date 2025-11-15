using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class ProveedorMappingProfile : Profile
    {
        public ProveedorMappingProfile()
        {
            CreateMap<Proveedor, ProveedorDto>();

            CreateMap<ProveedorCreateDto, Proveedor>()
                .ForMember(dest => dest.Id_Proveedor, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());

            CreateMap<ProveedorUpdateDto, Proveedor>()
                .ForMember(dest => dest.Id_Proveedor, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());
        }
    }
}

