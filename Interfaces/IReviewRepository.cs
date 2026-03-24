using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsForCustomerAsync(int customerId);
        Task<IEnumerable<Review>> GetReviewsForOrderAsync(int orderId);
        Task<Review?> GetReviewWithDetailsAsync(int id);
    }
}