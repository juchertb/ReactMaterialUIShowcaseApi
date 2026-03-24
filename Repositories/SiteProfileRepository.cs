using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class SiteProfileRepository : GenericRepository<SiteProfile>, ISiteProfileRepository
    {
        public SiteProfileRepository(ApplicationDBContext context) : base(context) { }
    }
}