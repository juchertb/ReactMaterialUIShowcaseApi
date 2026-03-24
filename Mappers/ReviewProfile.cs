using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.command, opt => opt.MapFrom(src => src.Order.Invoice))
                .ForMember(dest => dest.command_id, opt => opt.MapFrom(src => src.Order.Invoice == null ? string.Empty : src.Order.Invoice.Id))
                .ReverseMap();
        }
    }
}