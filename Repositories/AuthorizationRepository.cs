using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Enumerations;
using ReactMaterialUIShowcaseApi.Models;
using System.Data;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class AuthorizationRepository : BaseRepository
    {
        public AuthorizationRepository(ApplicationDBContext dbContext, IOptions<AppSettings> appSettings)
            : base(dbContext, appSettings) { }

        public async Task<AppUser?> CheckUserWebAsync(string userName, string userPassword, LanguageEnum userLanguage)
        {
            // Find the user by username
            var user = await _dbContext.Users
                .OfType<AppUser>()
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null;

            // Get the roles for the user
            var roles = await (from userRole in _dbContext.UserRoles
                               join role in _dbContext.Roles on userRole.RoleId equals role.Id
                               where userRole.UserId == user.Id
                               select new
                               {
                                   Name = role.Name,
                                   NormalizedName = role.NormalizedName
                               }).ToListAsync();

            user.BusinessRoleName = roles.FirstOrDefault()?.Name;

            // Use ASP.NET Identity's password hasher to verify the password
            var passwordHasher = new PasswordHasher<AppUser>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, userPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            // Set language and any other properties as needed
            user.Language = userLanguage;

            return user;
        }

        public async Task SaveRefreshTokenAsync(long userId, string userFullName, LanguageEnum userLanguage, string refreshToken, DateTime expiryDate)
        {
            var user = await _dbContext.Users.OfType<AppUser>().FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDate = expiryDate;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            var user = await GetUserByRefreshTokenAsync(refreshToken, LanguageEnum.iEnglish);
            if (user != null)
            {
                await SaveRefreshTokenAsync(user.UserId, user.FullName, user.Language, string.Empty, DateTime.UtcNow);
            }
        }

        public async Task<AppUser?> GetUserByIdAsync(long userId, LanguageEnum userLanguage)
        { 
            return await GetUserAsync(userId, userLanguage, 2, string.Empty);
        }

        public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken, LanguageEnum userLanguage)
        {
            return await _dbContext.Users.OfType<AppUser>().FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<AppUser?> GetUserAsync(long userId, LanguageEnum userLanguage, short selectType, string refreshToken)
        {
            return await _dbContext.Users.OfType<AppUser>().FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}