using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mappers
{
    public class SchedulerEventCategoryProfile : Profile
    {
        public SchedulerEventCategoryProfile()
        {
            CreateMap<SchedulerEventCategory, SchedulerEventCategoryDto>().ReverseMap();
        }
    }
}
