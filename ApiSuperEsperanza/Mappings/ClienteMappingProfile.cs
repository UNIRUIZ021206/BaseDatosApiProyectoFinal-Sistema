using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class ClienteMappingProfile : Profile
    {
        public ClienteMappingProfile()
        {
            CreateMap<Cliente, ClienteDto>();

            CreateMap<ClienteCreateDto, Cliente>()
                .ForMember(dest => dest.Id_Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.PuntosCompra, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UltimaCompra, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());

            CreateMap<ClienteUpdateDto, Cliente>()
                .ForMember(dest => dest.Id_Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.PuntosCompra, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore())
                .ForMember(dest => dest.UltimaCompra, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());
        }
    }
}

