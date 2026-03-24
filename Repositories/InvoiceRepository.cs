using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDBContext context) : base(context) { }

        public async Task<Invoice?> GetInvoiceWithDetailsAsync(string id)
        {
            return await _dbSet
                .Include(i => i.Order)
                .ThenInclude(c => c.Customer)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}