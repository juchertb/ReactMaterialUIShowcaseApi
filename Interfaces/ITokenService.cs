using ReactMaterialUIShowcaseApi.Models;
using System.Security.Claims;

namespace ReactMaterialUIShowcaseApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);

        /*
         * Refresh token related methods
         */
        string CreateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}