using Microsoft.AspNetCore.Http;
using ReactMaterialUIShowcaseApi.Enumerations;
using ReactMaterialUIShowcaseApi.Interfaces;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReactMaterialUIShowcaseApi.Services
{
    /// <summary>
    /// Provides access to the current authenticated user's claims and context during a request.
    /// Is a runtime helper for extracting user info from the current request's claims, making 
    /// it easier to access user context throughout your application.
    /// 
    /// Used at runtime to access information about the currently logged-in user(e.g., user ID, roles, etc.) via claims.
    ///Does not represent or persist user data in the database.
    /// </summary>
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long UserId
        {
            get
            {
                var claimValue = GetClaim(ClaimTypes.NameIdentifier);
                return long.TryParse(claimValue, out var userId) ? userId : 0;
            }
        }
        public string UserName => GetClaim("UserName") ?? string.Empty;
        public string FullName => GetClaim(ClaimTypes.Name) ?? string.Empty;
        public BusinessRoleConstants BusinessRoleCode
        {
            get
            {
                var claimValue = GetClaim("BusinessRoleCode");
                return Enum.TryParse<BusinessRoleConstants>(claimValue, out var roleCode) ? roleCode : 0;
            }   
        }
        public string? BusinessRoleName => GetClaim("BusinessRoleName");
        public long OrganizationId
        {
            get
            {
                var claimValue = GetClaim("OrganizationId");
                return long.TryParse(claimValue, out var orgId) ? orgId : 0;
            }
        }
        public string? OrganizationName => GetClaim("OrganizationName");
        public LanguageEnum Language
        {
            get
            {
                var claimValue = GetClaim("Language");
                return Enum.TryParse<LanguageEnum>(claimValue, out var lang) ? lang : LanguageEnum.iEnglish;
            }
        }

        public string? GetClaim(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
        }
    }
}
