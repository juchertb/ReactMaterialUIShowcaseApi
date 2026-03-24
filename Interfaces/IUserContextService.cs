using ReactMaterialUIShowcaseApi.Enumerations;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface IUserContextService
    {
        long UserId { get; }
        string UserName { get; }
        string FullName { get; }
        BusinessRoleConstants BusinessRoleCode { get; }
        string? BusinessRoleName { get; }
        long OrganizationId { get; }
        string? OrganizationName { get; }
        LanguageEnum Language { get; }
        string? GetClaim(string claimType);
    }
}