using System.Security.Claims;
using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using AuthorizationAPI.Entities.Identity;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationApi.UseCases.V1.Queries;
public class GetLoginQueryHandler : IQueryHandler<Contract.Services.V1.Identity.Query.Login, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly UserManager<AppUser> _userManager;

    public GetLoginQueryHandler(IJwtTokenService jwtTokenService, UserManager<AppUser> userManager, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
    }

    public async Task<Result<Response.Authenticated>> Handle(Contract.Services.V1.Identity.Query.Login request, CancellationToken cancellationToken)
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

        var response = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = refrestToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        // Key is unique, ex: email
        await _cacheService.SetAsync(request.Email, response);

        return Result.Success(response);
    }
}
