using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Data;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class CollectionRepository : GenericRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(ApplicationDBContext context) : base(context) { }
    }
}