using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class FacturaMappingProfile : Profile
    {
        public FacturaMappingProfile()
        {
            CreateMap<Factura, FacturaDto>();
            CreateMap<DetalleFactura, DetalleFacturaDto>();

            CreateMap<FacturaCreateDto, Factura>()
                .ForMember(dest => dest.Id_Factura, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteNombre, opt => opt.Ignore())
                .ForMember(dest => dest.CodigoSesion, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoFactura, opt => opt.Ignore())
                .ForMember(dest => dest.FechaAnulacion, opt => opt.Ignore())
                .ForMember(dest => dest.MotivoAnulacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Detalles, opt => opt.Ignore());

            CreateMap<DetalleFacturaCreateDto, DetalleFactura>()
                .ForMember(dest => dest.Id_DetalleFactura, opt => opt.Ignore())
                .ForMember(dest => dest.Id_Factura, opt => opt.Ignore())
                .ForMember(dest => dest.CodigoLote, opt => opt.Ignore())
                .ForMember(dest => dest.Id_Producto, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProducto, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore());
        }
    }
}

