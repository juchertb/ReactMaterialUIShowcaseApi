using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class SchedulerEventCategoryRepository : GenericRepository<SchedulerEventCategory>, ISchedulerEventCategoryRepository
    {
        public SchedulerEventCategoryRepository(ApplicationDBContext context) : base(context) { }
    }
}