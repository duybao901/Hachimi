using System.Security.Claims;
using AuthorizationApi.Abstractions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;

namespace AuthorizationApi.UseCases.V1.Queries;
public class GetLoginQueryHandler : IQueryHandler<Contract.Services.V1.Identity.Query.Login, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;

    public GetLoginQueryHandler(IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<Response.Authenticated>> Handle(Contract.Services.V1.Identity.Query.Login request, CancellationToken cancellationToken)
    {
        // Check User

        // Generate JWT Token
        var claims = new List<Claim>
        {
            new Claim("UserName", request.UserName),
            new Claim(ClaimTypes.Role, "Senior .NET")
        };

        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refrestToken = _jwtTokenService.GenerateRefreshToken();

        var response = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = refrestToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        // Key is unique, ex: email
        await _cacheService.SetAsync(request.UserName, response);

        return Result.Success(response);
    }
}
