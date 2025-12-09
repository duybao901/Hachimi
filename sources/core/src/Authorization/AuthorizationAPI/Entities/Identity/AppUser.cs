using Microsoft.AspNetCore.Identity;

namespace AuthorizationAPI.Entities.Identity;
public class AppUser : IdentityUser<Guid>
{   
    public virtual ICollection<IdentityUserClaim<Guid>> UserClaims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> UserLogins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> UserTokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}
