using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => (src.FirstName)))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => (src.LastName)))
                .ForMember(dest => dest.home_phone, opt => opt.MapFrom(src => (src.HomePhone)))
                .ForMember(dest => dest.mobile_phone, opt => opt.MapFrom(src => (src.MobilePhone)))
                .ForMember(dest => dest.twitter_url, opt => opt.MapFrom(src => (src.TwitterUrl)))
                .ForMember(dest => dest.facebook_url, opt => opt.MapFrom(src => (src.FacebookUrl)))
                .ForMember(dest => dest.instagram_url, opt => opt.MapFrom(src => (src.InstagramUrl)))
                .ForMember(dest => dest.linkedin_url, opt => opt.MapFrom(src => (src.LinkedInUrl)))
                .ReverseMap();
        }
    }
}