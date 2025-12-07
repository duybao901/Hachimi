using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using AuthorizationAPI.Extensions;
using AuthorizationAPI.Identity;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthorizationAPI.UseCases.V1.Commands;

public class RegisterCommandHandler : ICommandHandler<Command.RegisterUser>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly UserManager<AppUser> _userManager;

    public RegisterCommandHandler(IJwtTokenService jwtTokenService, ICacheService cacheService, UserManager<AppUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
    }
    public async Task<Result> Handle(Command.RegisterUser request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            throw new IdentityException.UserAlreadyExistsException(request.Email);
        }

        // Create new user
        var newUser = new AppUser
        {
            Name = request.Name,
            UserName = UsernameGenerator.GenerateUsername(request.Name),
            Email = request.Email,
            AvatarUrl = null
        };

        if(request.Password != request.ConfirmPassword)
        {
            throw new IdentityException.PasswordsDoNotMatchException();
        }

        var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

        if (!createUserResult.Succeeded)
            throw new IdentityException.UserCreationFailedException(createUserResult.Errors);

        await _userManager.AddToRoleAsync(newUser, "Author");


        // Raise domain event UserRegisteredEvent
        var roles = await _userManager.GetRolesAsync(newUser);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, newUser.Email),
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
