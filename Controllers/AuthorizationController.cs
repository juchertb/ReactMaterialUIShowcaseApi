using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Enumerations;
using ReactMaterialUIShowcaseApi.Helpers;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Mappers;
using ReactMaterialUIShowcaseApi.Models;
using ReactMaterialUIShowcaseApi.Repositories;
using ReactMaterialUIShowcaseApi.Resources;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;

[ApiController]
[SetUserCulture] // controls localization based on language selected at logon
[Route("authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly AuthorizationRepository _authorizationRepository;
    private readonly ILogger<AuthorizationController> _logger;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public AuthorizationController(ITokenService tokenService, AuthorizationRepository authorizationRepository, ILogger<AuthorizationController> logger, IStringLocalizer<ErrorMessages> localizer)
    {
        _tokenService = tokenService;
        _authorizationRepository = authorizationRepository;
        _logger = logger;
        _localizer = localizer;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            // Set culture based on loginDto.Language before validation
            var culture = loginDto.Language switch
            {
                LanguageEnum.iFrench => "fr",
                LanguageEnum.iEnglish => "en",
                _ => "en"
            };
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            // Conduct model validation. This cannot be done before the action because the language
            // isn't known. The selected language is part of the JWT token which is set during authentication.
            var validationContext = new ValidationContext(loginDto, HttpContext.RequestServices, null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(loginDto, validationContext, validationResults, true))
            {
                return BadRequest(validationResults.Select(r => r.ErrorMessage));
            }

            AppUser? user = await _authorizationRepository.CheckUserWebAsync(loginDto.Username, loginDto.Password, ((loginDto.Language == LanguageEnum.iNone || loginDto.Language == LanguageEnum.iBoth) ? LanguageEnum.iEnglish : loginDto.Language));
            if (user == null) return Unauthorized();

            var token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(3); // valid for 3 days

            // Save refresh token to database
            await _authorizationRepository.SaveRefreshTokenAsync(user.UserId, user.FullName, user.Language, user.RefreshToken, user.RefreshTokenExpiryDate);

            var userDto = user.ToDto();
            return Ok(new { token, refreshToken, user = userDto });
        }
        catch (SqlValidationException ex)
        {
            _logger.LogError(ex, $"OracleValidationException error (id = {ex.ErrorRecordId})" + " Data: {@ExceptionData}", ex.Data);
            return BadRequest(new
            {
                error = ex.GetType().ToString(),
                parameter = ex.Data["dbErrorField"],
                errorCode = ex.Data["dbErrorCodeMMCC"],
                message = ex.Message,
                errorRecordId = ex.ErrorRecordId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logon.");
            return StatusCode(500, _localizer["GenericProcessingError"].Value);
        }
    }


    /// <summary>
    /// The refresh token is called by the application on 401 unauthorized errors
    /// 
    /// Security Considerations
    ///   •	Always store refresh tokens securely.
    ///   •	Invalidate refresh tokens on logout or password change.
    ///   •	Use HTTPS for all endpoints.
    ///   ---
    /// Summary of changes:
    ///   •	Update ITokenService and its implementation.
    ///   •	Store refresh tokens with users.
    ///   •	Return refresh tokens on login.
    ///   •	Add a /refresh endpoint.
    ///   •	Validate and rotate refresh tokens.
    /// 
    /// 
    /// 1. Purpose and Structure
    ///•	Access Token(JWT):
    ///     •	Contains user claims and metadata.
    ///     •	Is cryptographically signed(and sometimes encrypted).
    ///     •	Used for authentication/authorization on each API call.
    ///     •	Short-lived(e.g., 15–60 minutes).
    ///•	Refresh Token:
    ///     •	Is a random, unguessable string (not a JWT).
    ///     •	Used only to obtain new access tokens.
    ///     •	Long-lived (e.g., days or weeks).
    ///     •   	Not sent with every API call—only to the token endpoint.
    ///---
    ///2. Security Model
    ///     •	Strength:
    ///         The security of a refresh token comes from its randomness and secrecy.It should be:
    ///     •	At least 256 bits (32 bytes) of cryptographically secure random data(as in your implementation).
    ///     •	Stored securely on the client(e.g., HTTP-only cookie or secure storage).
    ///     •	Stored securely on the server, associated with the user.
    ///     •	Attack Surface:
    ///     •	Refresh tokens are only exposed during token refresh, not on every API call.
    ///     •	If a refresh token is leaked, it can be revoked (by removing it from the server or DB).
    ///---
    ///3. Best Practices
    ///     •	Rotate refresh tokens on every use(issue a new one and invalidate the old).
    ///     •	Set an expiration for refresh tokens.
    ///     •	Revoke on logout or password change.
    ///     •	Store securely (never expose in URLs or client-side JS).
    ///
    ///
    /// </summary>
    /// <param name="tokenApiModel"></param>
    /// <returns></returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
    {
        try
        {
            if (tokenApiModel is null)
                return BadRequest(_localizer["InvalidClientRequest"].Value);

            string? accessToken = tokenApiModel.AccessToken;
            string? refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
                return BadRequest(_localizer["InvalidAccessOrRefreshToken"].Value);

            string? userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return BadRequest(_localizer["InvalidAccessOrRefreshToken"].Value);

            // Get the user's selected language from the Claims
            LanguageEnum userLanguage;
            var userLanguageString = principal.Claims.FirstOrDefault(c => c.Type == "Language")?.Value;
            if (!Enum.TryParse<LanguageEnum>(userLanguageString, true, out userLanguage))
            {
                userLanguage = LanguageEnum.iEnglish;
            }

            // Retrieve the user from the database
            var user = await _authorizationRepository.GetUserByIdAsync(userId, userLanguage);
            if (user == null)
                return BadRequest(_localizer["UserNotFound"].Value);

            // Validate the refresh token and expiry
            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
                return BadRequest(_localizer["InvalidOrExpiredRefreshToken"].Value);

            var newAccessToken = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(3);

            // Save refresh token to database
            await _authorizationRepository.SaveRefreshTokenAsync(user.UserId, user.FullName, user.Language, user.RefreshToken, user.RefreshTokenExpiryDate);

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
        catch (SqlValidationException ex)
        {
            _logger.LogError(ex, $"OracleValidationException error (id = {ex.ErrorRecordId})" + " Data: {@ExceptionData}", ex.Data);
            return BadRequest(new
            {
                error = ex.GetType().ToString(),
                parameter = ex.Data["dbErrorField"],
                errorCode = ex.Data["dbErrorCodeMMCC"],
                message = ex.Message,
                errorRecordId = ex.ErrorRecordId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logon.");
            return StatusCode(500, _localizer["GenericProcessingError"].Value);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string? refreshToken)
    {
        try
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(_localizer["RefreshTokenRequired"].Value);

            await _authorizationRepository.InvalidateRefreshTokenAsync(refreshToken);

            return Ok(new { message = _localizer["LoggedOutAndTokenInvalidated"].Value });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logon.");
            return StatusCode(500, _localizer["GenericProcessingError"].Value);
        }
    }
}