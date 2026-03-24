using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class SiteProfileProfile : Profile
    {
        public SiteProfileProfile()
        {
            CreateMap<SiteProfile, SiteProfileDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)));

            CreateMap<SiteProfileDto, SiteProfile>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}