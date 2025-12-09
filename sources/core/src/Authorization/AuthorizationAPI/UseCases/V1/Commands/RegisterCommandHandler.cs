using AuthorizationApi.Abstractions;
using AuthorizationApi.Exceptions;
using AuthorizationAPI.Abstractions.Repositories;
using AuthorizationAPI.Entities;
using AuthorizationAPI.Entities.Identity;
using AuthorizationAPI.Extensions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationAPI.UseCases.V1.Commands;

public class RegisterCommandHandler : ICommandHandler<Command.RegisterUserCommand>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IRepositoryBase<UserProfile, Guid> _userProfileRepositoryBase;

    public RegisterCommandHandler(IJwtTokenService jwtTokenService, ICacheService cacheService, UserManager<AppUser> userManager, IRepositoryBase<UserProfile, Guid> userProfileRepositoryBase)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
        _userProfileRepositoryBase = userProfileRepositoryBase;
    }
    public async Task<Result> Handle(Command.RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            throw new IdentityException.UserAlreadyExistsException(request.Email);
        }

        // Create new user
        var newUser = new AppUser
        {
            UserName = UsernameGenerator.GenerateUsername(request.Name),
            Email = request.Email,
        };

        if (request.Password != request.ConfirmPassword)
        {
            throw new IdentityException.confirmPassWordDoNotMatchException();
        }

        var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

        if (!createUserResult.Succeeded)
            throw new IdentityException.UserCreationFailedException(createUserResult.Errors);

        await _userManager.AddToRoleAsync(newUser, "Author");

        var userProfile = UserProfile.Create(newUser.Id, request.Name, newUser.NormalizedUserName, request.Email);
        _userProfileRepositoryBase.Add(userProfile);

        return Result.Success("Register susscess!");
    }
}
