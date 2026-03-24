using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Helpers;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Models;
using System.Data;
using ReactMaterialUIShowcaseApi.Enumerations;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDBContext _dbContext;
        protected readonly string _dbConnection = string.Empty;
        protected readonly string _environment = string.Empty;

        protected BaseRepository(ApplicationDBContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _dbConnection = appSettings.Value.RpmsWebDbConnection;
            _environment = appSettings.Value.RpmsWebEnvironment;
        }

        protected BaseRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
