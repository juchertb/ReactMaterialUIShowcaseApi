using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Dtos;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface ISiteProfileRepository : IGenericRepository<SiteProfile>
    {
        void Update(SiteProfile entity, SiteProfileDto dto);
        Task<SiteProfile?> GetSiteProfileWithDetailsAsync(int id);
    }
}