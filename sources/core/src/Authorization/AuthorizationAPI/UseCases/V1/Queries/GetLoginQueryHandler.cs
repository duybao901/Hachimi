using System.Security.Claims;
using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using AuthorizationAPI.Abstractions.Repositories;
using AuthorizationAPI.Entities;
using AuthorizationAPI.Entities.Identity;
using AuthorizationAPI.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identitys;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationApi.UseCases.V1.Queries;
public class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.LoginTokenResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositoryBase<UserProfile, Guid> _userProfileRepositoryBase;

    public GetLoginQueryHandler(
        IJwtTokenService jwtTokenService, 
        UserManager<AppUser> userManager, 
        ICacheService cacheService, 
        IHttpContextAccessor httpContextAccessor, 
        IRepositoryBase<UserProfile, Guid> userProfileRepositoryBase)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _userProfileRepositoryBase = userProfileRepositoryBase;
    }

    public async Task<Result<Response.LoginTokenResponse>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        // Check user
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) { 
            throw new IdentityException.UserNotExistsException(request.Email);
        }

        // Check password
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            throw new IdentityException.WrongPassWordException();
        }

        // Generate JWT Token
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Email", user.Email),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var hashRefreshToken = _jwtTokenService.HashToken(refreshToken);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,                 // HTTP
            SameSite = SameSiteMode.Lax,    // same-origin
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"                     
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

        // Key is unique, ex: email
        var authenticated = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
        };

        // key is refreshToken
        await _cacheService.SetAsync(hashRefreshToken, authenticated, cancellationToken);

        var userProfile = await _userProfileRepositoryBase.FindSingleAsync(u => u.UserId == user.Id);
        if (userProfile == null)
        {
            throw new UserProfileException.UserProfileNotFoundWithUserIdException(user.Id);
        }

        var response = new Response.LoginTokenResponse()
        {
            AccessToken = accessToken,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
            CurrentUser = new Response.CurrentUser()
            {
                Id = user.Id,
                Email = user.Email,
                Name = userProfile.Name,
                UserName = userProfile.UserName,
                AvatarUrl = userProfile.AvatarUrl
            }
        };

        return Result.Success(response);
    }
}
