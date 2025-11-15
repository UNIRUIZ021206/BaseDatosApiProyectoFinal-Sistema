using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class LoteMappingProfile : Profile
    {
        public LoteMappingProfile()
        {
            CreateMap<Lote, LoteDto>();

            CreateMap<LoteCreateDto, Lote>()
                .ForMember(dest => dest.Id_Lote, opt => opt.Ignore())
                .ForMember(dest => dest.NombreProducto, opt => opt.Ignore())
                .ForMember(dest => dest.FechaIngreso, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}

