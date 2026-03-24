using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext context) : base(context) { }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Collection)
                .Include(p => p.Color)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Collection)
                .Include(p => p.Color)
                .Include(p => p.Tags)
                .ToListAsync();
        }
    }
}