using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identitys;

namespace AuthorizationAPI.UseCases.V1.Commands;

public class LogoutCommandHandler : ICommandHandler<Command.Logout>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutCommandHandler(IJwtTokenService jwtTokenService, ICacheService cacheService, IHttpContextAccessor httpContextAccessor)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(Command.Logout request, CancellationToken cancellationToken)
    {
        var refreshToken = request.RefreshToken;
        var hashRefreshToken = _jwtTokenService.HashToken(refreshToken);
        var email = request.Email;

        //var authenticated = await _cacheService.GetAsync<Response.Authenticated>(email + ":" + hashRefreshToken);
        var authenticated = await _cacheService.GetAsync<Response.Authenticated>("refresh:" + email);

        if (authenticated == null)
        {
            throw new IdentityException.TokenException("Request Token Invalid");
        }

        await _cacheService.RemoveAsync("refresh:" + email);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,                 // HTTP
            SameSite = SameSiteMode.Lax,    // same-origin
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"                     
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken", cookieOptions);

        return Result.Success("Logout success");
    }
}
