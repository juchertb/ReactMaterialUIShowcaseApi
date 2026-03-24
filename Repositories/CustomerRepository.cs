using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDBContext context) : base(context) { }

        public async Task<Customer?> GetCustomerWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> SaveChangesAsync(int customerId)
        {
            bool result = false;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int maxId = customerId;
                if (maxId == 0)
                {
                    result = await _context.SaveChangesAsync() > 0;
                    maxId = await GetMaxId();
                }
                Customer? newCustomer = null;
                if (maxId > 0)
                {
                    newCustomer = await _dbSet.FindAsync(maxId);
                    if (newCustomer != null)
                    {
                        // Add fake order record
                        Order newOrder = new Order
                        {
                            CustomerId = newCustomer.Id,
                            Date = DateTime.UtcNow,
                            TotalExTaxes = 100.0f,
                            DeliveryFees = 10.0f,
                            TaxRate = 0.2f,
                            Taxes = 20.0f,
                            Total = 130.0f
                        };
                        _context.Orders.Add(newOrder);
                        result = await _context.SaveChangesAsync() > 0;

                        // Add fake invoice record
                        Invoice newInvoice = new Invoice
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            OrderId = newOrder.Id,
                            Status = "Ordered",
                            Date = DateTime.UtcNow,
                            TotalExTaxes = 100.0f,
                            DeliveryFees = 10.0f,
                            TaxRate = 0.2f,
                            Taxes = 20.0f,
                            Total = 130.0f
                        };
                        _context.Invoices.Add(newInvoice);
                        result = await _context.SaveChangesAsync() > 0;
                    }
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<int> GetMaxId()
        {
            return await _dbSet.MaxAsync(c => (int?)c.Id) ?? 0;
        }

        public async Task<int> FindCustomer(string firstName, string lastName)
        {
            var customer = await _dbSet.FirstOrDefaultAsync(p =>
                p.FirstName.ToLower() == firstName.Trim().ToLower() &&
                p.LastName.ToLower() == lastName.Trim().ToLower());

            if (customer == null) return 0;
            return customer.Id;
        }
    }
}