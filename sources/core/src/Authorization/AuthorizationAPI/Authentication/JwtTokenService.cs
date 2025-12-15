using AuthorizationApi.Abstractions;
using AuthorizationApi.DependencyInjection.Options;
using AuthorizationApi.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Authentication;
public class JwtTokenService : IJwtTokenService
{

    private readonly JwtOption jwtOption = new JwtOption();

    public JwtTokenService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOption.ExpireMin),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[32]; // 256-bit
        RandomNumberGenerator.Fill(bytes);
        return WebEncoders.Base64UrlEncode(bytes); // URL & cookie safe
    }

    public string HashToken(string token)
    {
        if (token == null)
        {
            throw new IdentityException.TokenException("Request Token Invalid");
        }
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // Ensure jwtOption.SecretKey/Issuer/Audience are populated (throw early if not)
        if (string.IsNullOrWhiteSpace(jwtOption.SecretKey))
            throw new InvalidOperationException("JWT SecretKey is not configured.");
        if (string.IsNullOrWhiteSpace(jwtOption.Issuer))
            throw new InvalidOperationException("JWT Issuer is not configured.");
        if (string.IsNullOrWhiteSpace(jwtOption.Audience))
            throw new InvalidOperationException("JWT Audience is not configured.");

        var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //false -> you might want to validate the audience and issuer depending on your use case
            ValidAudience = jwtOption.Audience,
            ValidateIssuer = false,
            ValidIssuer = jwtOption.Issuer,
            ValidateLifetime = false, //false -> here we are saying that we don't care about the token's expiration date
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException(ex.Message);
        }
    }
}
