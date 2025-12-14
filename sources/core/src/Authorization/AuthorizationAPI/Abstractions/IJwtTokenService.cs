using System.Security.Claims;

namespace AuthorizationApi.Abstractions;

public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    string HashToken(string token);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
