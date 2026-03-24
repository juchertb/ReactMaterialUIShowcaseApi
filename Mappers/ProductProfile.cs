using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)))
                .ForMember(dest => dest.category_id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.category_name, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.collection_id, opt => opt.MapFrom(src => src.CollectionId))
                .ForMember(dest => dest.collection_name, opt => opt.MapFrom(src => src.Collection.Name))
                .ForMember(dest => dest.color_id, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.color_name, opt => opt.MapFrom(src => src.Color.Name));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.category_id))
                //.ForMember(dest => dest.Category.Name, opt => opt.MapFrom(src => src.category_name))
                .ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.collection_id))
                //.ForMember(dest => dest.Collection.Name, opt => opt.MapFrom(src => src.collection_name))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.color_id));
                //.ForMember(dest => dest.Color.Name, opt => opt.MapFrom(src => src.color_name));
        }
    }
}