using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class SchedulerEventProfile : Profile
    {
        public SchedulerEventProfile()
        {
            CreateMap<SchedulerEvent, SchedulerEventDto>().ReverseMap();
        }
    }
}