using Microsoft.AspNetCore.Identity;
using ReactMaterialUIShowcaseApi.Enumerations;
using System;

namespace ReactMaterialUIShowcaseApi.Models
{
    /// <summary>
    /// Persistent user model required for Identity operations and user data storage.
    /// 
    /// Represents the user entity in your database and is used by ASP.NET Core Identity for user management (registration, 
    /// login, password management, roles, etc.).
    /// 
    ///	Used by Entity Framework Core to map user data to your database.
    ///	Used when creating, updating, or querying user records.
    ///	Source of user data when generating authentication tokens (e.g., in your TokenService).
    /// </summary>
    public class AppUser : IdentityUser
    {
        public long UserId { get; set; }
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string FullName {
            get
            {
                return $"{GivenName} {Surname}";
            }
        }
        public int BusinessRoleCode { get; set; }
        public string? BusinessRoleName { get; set; } = string.Empty;
        public bool PasswordExpiredInd { get; set; }
        public long OrganizationId { get; set; }
        public string? OrganizationName { get; set; } = string.Empty;
        public LanguageEnum Language { get; set; } = LanguageEnum.iEnglish;

        /*
        * Refresh token related properties
        */
        public long RefreshTokenId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryDate { get; set; }
        public DateTime RefreshTokenCreatedDate { get; set; }
        public string RefreshTokenCreatedBy { get; set; } = string.Empty;
    }
}