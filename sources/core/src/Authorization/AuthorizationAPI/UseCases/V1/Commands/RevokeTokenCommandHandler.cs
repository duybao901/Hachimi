using System.Security.Claims;
using AuthorizationApi.Abstractions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;

namespace AuthorizationApi.UseCases.V1.Commands;

public class RevokeTokenCommandHandler : ICommandHandler<Contract.Services.V1.Identity.Command.RevokeToken>
{
    private readonly ICacheService _cacheService;
    private readonly IJwtTokenService _jwtTokenService;

    public RevokeTokenCommandHandler(ICacheService cacheService, IJwtTokenService jwtTokenService)
    {
        _cacheService = cacheService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> Handle(Contract.Services.V1.Identity.Command.RevokeToken request, CancellationToken cancellationToken)
    {
        var AccessToken = request.AccessToken;
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(AccessToken);
        var userNameKey = principal.FindFirstValue(ClaimTypes.Email).ToString();

        var authenticated = await _cacheService.GetAsync<Response.Authenticated>(userNameKey) ?? throw new Exception("Can not get value from Redis");

        await _cacheService.RemoveAsync(userNameKey, cancellationToken);

        return Result.Success();
    }
}
