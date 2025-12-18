using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using AuthorizationAPI.Abstractions.Repositories;
using AuthorizationAPI.Entities;
using AuthorizationAPI.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthorizationApi.UseCases.V1.Queries;

public class GetRefreshTokenQueryHandler : IQueryHandler<Contract.Services.V1.Identity.Query.Refresh, Response.RefreshTokenResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AuthorizationAPI.Entities.Identity.AppUser> _userManager;
    private readonly IRepositoryBase<UserProfile, Guid> _userProfileRepositoryBase;

    public GetRefreshTokenQueryHandler(IJwtTokenService jwtTokenService, ICacheService cacheService, IHttpContextAccessor httpContextAccessor, 
        UserManager<AuthorizationAPI.Entities.Identity.AppUser> userManager, 
        IRepositoryBase<UserProfile, Guid> userProfileRepositoryBase)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _userProfileRepositoryBase = userProfileRepositoryBase;
    }

    public async Task<Result<Response.RefreshTokenResponse>> Handle(Contract.Services.V1.Identity.Query.Refresh request, CancellationToken cancellationToken)
    {
        var refreshToken = request.RefreshToken;
        var hashRefreshToken = _jwtTokenService.HashToken(refreshToken);
        if (refreshToken is null)
        {
            throw new IdentityException.TokenException("Request Token Invalid");
        }

        var authenticated = await _cacheService.GetAsync<Response.Authenticated>(hashRefreshToken);
        /*
            1. Tìm trong bộ nhớ cache (_cacheService.GetAsync) xem có thông tin xác thực của người dùng không.
            2. So sánh Refresh Token từ request với token trong cache: Nếu không khớp, token có thể bị giả mạo.
            3. Kiểm tra thời gian hết hạn của Refresh Token: Nếu hết hạn, yêu cầu bị từ chối và người dùng phải đăng nhập lại
         */
        if (authenticated is null || authenticated.RefreshToken != refreshToken || authenticated.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new IdentityException.TokenException("Request Token Invalid");
        }

        // Rotate refresh token
        await _cacheService.RemoveAsync(hashRefreshToken, cancellationToken);

        // Refresh token is valid -> Get old access token info -> get principal
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(authenticated.AccessToken);
        var newAccessToken = _jwtTokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var hashNewRefreshToken = _jwtTokenService.HashToken(newRefreshToken);

        // Set new refresh token in cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,                 // HTTP
            SameSite = SameSiteMode.Lax,    // same-origin
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"                     
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

        // Key is unique, ex: email
        var newAuthenticated = new Response.Authenticated()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
        };
        await _cacheService.SetAsync(hashNewRefreshToken, newAuthenticated, cancellationToken).ConfigureAwait(true);

        var email = principal.FindFirstValue(ClaimTypes.Email).ToString();
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new IdentityException.UserNotExistsException(email);
        }

        var userProfile = await _userProfileRepositoryBase.FindSingleAsync(u => u.UserId == user.Id);
        if (userProfile == null)
        {
            throw new UserProfileException.UserProfileNotFoundWithUserIdException(user.Id);
        }

        var refreshTokenResponse = new Response.RefreshTokenResponse
        {
            AccessToken = newAccessToken,
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

        return Result.Success(refreshTokenResponse);
    }
}
