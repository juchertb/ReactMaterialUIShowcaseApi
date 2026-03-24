using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetCustomerWithDetailsAsync(int id);
        Task<int> GetMaxId();
        Task<int> FindCustomer(string firstName, string lastName);
        Task<bool> SaveChangesAsync(int customerId);
    }
}