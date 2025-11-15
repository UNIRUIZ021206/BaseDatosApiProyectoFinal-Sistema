using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class ProductoMappingProfile : Profile
    {
        public ProductoMappingProfile()
        {
            CreateMap<Producto, ProductoDto>();

            CreateMap<ProductoCreateDto, Producto>()
                .ForMember(dest => dest.Id_Producto, opt => opt.Ignore())
                .ForMember(dest => dest.NombreCategoria, opt => opt.Ignore())
                .ForMember(dest => dest.CostoPromedio, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());

            CreateMap<ProductoUpdateDto, Producto>()
                .ForMember(dest => dest.Id_Producto, opt => opt.Ignore())
                .ForMember(dest => dest.NombreCategoria, opt => opt.Ignore())
                .ForMember(dest => dest.CostoPromedio, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());
        }
    }
}

