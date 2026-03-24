using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Interfaces;
using System;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(ApplicationDBContext context) : base(context) { }
    }
}