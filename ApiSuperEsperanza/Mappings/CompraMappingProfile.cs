using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class CompraMappingProfile : Profile
    {
        public CompraMappingProfile()
        {
            CreateMap<Compra, CompraDto>();
            CreateMap<DetalleCompra, DetalleCompraDto>();

            CreateMap<CompraCreateDto, Compra>()
                .ForMember(dest => dest.Id_Compra, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProveedor, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Detalles, opt => opt.Ignore());

            CreateMap<DetalleCompraCreateDto, DetalleCompra>()
                .ForMember(dest => dest.Id_DetalleCompra, opt => opt.Ignore())
                .ForMember(dest => dest.Id_Compra, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProducto, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore());

            CreateMap<DetalleCompraUpdateDto, DetalleCompra>()
                .ForMember(dest => dest.Id_DetalleCompra, opt => opt.Ignore())
                .ForMember(dest => dest.Id_Compra, opt => opt.Ignore())
                .ForMember(dest => dest.Id_Producto, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProducto, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());
        }
    }
}

