using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetProductWithDetailsAsync(int id);
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();
    }
}