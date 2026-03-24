using ReactMaterialUIShowcaseApi.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int id);
    }
}