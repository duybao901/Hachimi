using Microsoft.AspNetCore.Identity;

namespace AuthorizationAPI.Identity;
public class AppUser : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> UserClaims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> UserLogins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> UserTokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}
