using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface ISiteSettingsRepository : IGenericRepository<SiteSettings>
    {
        Task<SiteSettings?> GetSettingsAsync();
    }
}