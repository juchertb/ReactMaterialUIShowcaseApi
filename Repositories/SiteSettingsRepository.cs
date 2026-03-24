using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class SiteSettingsRepository : GenericRepository<SiteSettings>, ISiteSettingsRepository
    {
        public SiteSettingsRepository(ApplicationDBContext context) : base(context) { }

        public async Task<SiteSettings?> GetSettingsAsync()
        {
            return await _dbSet.FirstOrDefaultAsync();
        }
    }
}