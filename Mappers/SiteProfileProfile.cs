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
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                        src.Tags == null
                            ? new List<SiteProfileTag>()
                            : src.Tags
                                .Where(t => !string.IsNullOrWhiteSpace(t)) // Remove empty strings
                                .Distinct(StringComparer.OrdinalIgnoreCase) // Remove duplicates in Source
                                .Select(tagName => new SiteProfileTag
                                {
                                    Tag = tagName
                                })
                                .ToList()
                    ));
        }
    }
}