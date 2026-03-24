using Microsoft.IdentityModel.Tokens;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Models;
using ReactMaterialUIShowcaseApi.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReactMaterialUIShowcaseApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"] ?? string.Empty));
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Surname, user.Surname ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Name, (user.GivenName + " " + user.Surname) ?? string.Empty), 
                new Claim("BusinessRoleCode", user.BusinessRoleCode.ToString()), 
                new Claim("BusinessRoleName", user.BusinessRoleName ?? string.Empty), 
                new Claim("OrganizationId", user.OrganizationId.ToString()), 
                new Claim("OrganizationName", user.OrganizationName ?? string.Empty),
                new Claim("UserName", user.UserName ?? string.Empty),
                new Claim("Language", user.Language.ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        ///  Generate a secure refresh token to be a random string with a cryptographically secure RNG. 
        ///  Stored securely, and managed according to best practices.
        ///  The access token is more complex because it needs to carry claims and be verifiable by the API.
        /// </summary>
        /// <returns></returns>
        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            /*
             * Todo:: The tokenValidationParameters need to be reviewed.
             */
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],

                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,

                ValidateLifetime = false, // Ignore expiration for refresh. It is handled in the authorization controller.

                ClockSkew = TimeSpan.Zero // Optional: no clock skew
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null)
                return null;
            return principal;
        }
    }
}