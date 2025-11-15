using AutoMapper;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi.Mappings
{
    public class CategoriaMappingProfile : Profile
    {
        public CategoriaMappingProfile()
        {
            // Mapeo de Categoria a CategoriaDto
            CreateMap<Categoria, CategoriaDto>();

            // Mapeo de CategoriaCreateDto a Categoria
            CreateMap<CategoriaCreateDto, Categoria>()
                .ForMember(dest => dest.Id_Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());

            // Mapeo de CategoriaUpdateDto a Categoria (para actualizaciones)
            CreateMap<CategoriaUpdateDto, Categoria>()
                .ForMember(dest => dest.Id_Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id_UsuarioModificacion, opt => opt.Ignore());
        }
    }
}

