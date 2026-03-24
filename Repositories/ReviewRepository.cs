using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;
using ReactMaterialUIShowcaseApi.Services.Query;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDBContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetReviewsForCustomerAsync(int customerId)
        {
            return await _dbSet
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsForOrderAsync(int orderId)
        {
            return await _dbSet
                .Where(r => r.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Customer)
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async override Task<(IEnumerable<Review> Items, int Total)> QueryAsync(ParsedListQuery parsed)
        {
            var query = _dbSet
                //.Include(o => o.Customer)   // ⭐ Load related customer
                .Include(r => r.Order)
                .ThenInclude(i => i.Invoice)
                .AsQueryable();

            return await query.ApplyListQueryAsync(parsed);
        }
    }
}