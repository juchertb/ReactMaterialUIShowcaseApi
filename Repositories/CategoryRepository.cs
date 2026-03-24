using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Data;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDBContext context) : base(context) { }
    }
}