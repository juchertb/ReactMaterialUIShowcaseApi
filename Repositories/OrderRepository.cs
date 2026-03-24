using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using ReactMaterialUIShowcaseApi.Services.Query;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDBContext context) : base(context) { }

        public async Task<Order?> GetOrderWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Customer)
                //.Include(o => o.Invoice)
                //.Include(o => o.Reviews)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async override Task<(IEnumerable<Order> Items, int Total)> QueryAsync(ParsedListQuery parsed)
        {
            var query = _dbSet
                //.Include(o => o.Customer)   // ⭐ Load related customer
                .AsQueryable();

            return await query.ApplyListQueryAsync(parsed);
        }

    }
}