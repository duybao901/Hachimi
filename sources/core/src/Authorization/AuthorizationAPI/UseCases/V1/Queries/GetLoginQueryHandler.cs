using System.Security.Claims;
using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using AuthorizationAPI.Entities.Identity;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationApi.UseCases.V1.Queries;
public class GetLoginQueryHandler : IQueryHandler<Contract.Services.V1.Identity.Query.Login, Response.LoginTokenResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetLoginQueryHandler(IJwtTokenService jwtTokenService, UserManager<AppUser> userManager, ICacheService cacheService, IHttpContextAccessor httpContextAccessor)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<Response.LoginTokenResponse>> Handle(Contract.Services.V1.Identity.Query.Login request, CancellationToken cancellationToken)
    {
        // Check user
        var userExists = await _userManager.FindByEmailAsync(request.Email);

        if (userExists == null) { 
            throw new IdentityException.UserNotExistsException(request.Email);
        }

        // Check password
        var passwordValid = await _userManager.CheckPasswordAsync(userExists, request.Password);
        if (!passwordValid)
        {
            throw new IdentityException.WrongPassWordException();
        }

        // Generate JWT Token
        var roles = await _userManager.GetRolesAsync(userExists);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, userExists.Email),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refrestToken = _jwtTokenService.GenerateRefreshToken();
        var hashRefreshToken = _jwtTokenService.HashToken(refrestToken);

        var response = new Response.LoginTokenResponse()
        {
            AccessToken = accessToken,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
        };

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,                 // HTTP
            SameSite = SameSiteMode.Lax,    // same-origin
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"                      // QUAN TRỌNG
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refrestToken, cookieOptions);

        // Key is unique, ex: email
        var authenticated = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = refrestToken,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
        };

        // key is refreshToken
        await _cacheService.SetAsync(hashRefreshToken, authenticated, cancellationToken).ConfigureAwait(true);

        return Result.Success(response);
    }
}
