using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Data;
using System;
using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Services.Query;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class SchedulerEventRepository : GenericRepository<SchedulerEvent>, ISchedulerEventRepository
    {
        public SchedulerEventRepository(ApplicationDBContext context) : base(context) { }

        public new async virtual Task<(IEnumerable<SchedulerEvent> Items, int Total)> QueryAsync(ParsedListQuery parsed)
        {
            if (parsed.EmbededTables is null) 
                parsed.EmbededTables = new List<string>();
            parsed.EmbededTables.Add("Category");

            return await _dbSet.ApplyListQueryAsync(parsed);
        }
    }
}
