using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class SiteSettingsProfile : Profile
    {
        public SiteSettingsProfile()
        {
            CreateMap<SiteSettings, SiteSettingsDto>().ReverseMap();
        }
    }
}