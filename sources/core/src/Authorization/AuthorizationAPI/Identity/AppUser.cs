using Microsoft.AspNetCore.Identity;

namespace AuthorizationAPI.Identity;
public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public DateTime DateOfBirth { get; set; }

    public string Address { get; set; } = null!;

    public bool? IsDirector { get; set; }

    public bool? IsHeadOfDepartment { get; set; }

    public Guid ManagerId { get; set; }

    public Guid UserId { get; set; }

    public int IsReceipient { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> UserClaims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> UserLogins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> UserTokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}
