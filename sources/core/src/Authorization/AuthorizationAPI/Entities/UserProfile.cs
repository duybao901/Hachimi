using AuthorizationAPI.Abstractions.Aggregates;
using Contract.Services.V1.UserProfiles;

namespace AuthorizationAPI.Entities;
public class UserProfile : AggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public string UserName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Website { get; set; }
    public string? Location { get; set; }

    private UserProfile() { }

    public static UserProfile Create(Guid userId, string name, string userName, string email)
    {
        UserProfile userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserName = userName,
            Name = name,
            Bio = "",
            AvatarUrl = "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/capybara_vwpmqp.png"
        };

        userProfile.RaiseDomainEvent(new DomainEvent.UserRegisterEvent(Guid.NewGuid(), userProfile.Id, userId, name, userName, email, userProfile.AvatarUrl));

        return userProfile;
    }

    public void UpdateProfile(string name, string? bio, string? avatarUrl)
    {
        Name = name;
        Bio = bio;
        AvatarUrl = avatarUrl;
    }
}
