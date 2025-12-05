using AuthorizationAPI.Contants;
using AuthorizationAPI.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationAPI.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.UserName).IsUnique();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(256);

        // Each User can have many UserClaims
        builder.HasMany(AppUsers => AppUsers.UserClaims)
            .WithOne()
            .HasForeignKey(UserClaims => UserClaims.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(AppUsers => AppUsers.UserLogins)
            .WithOne()
            .HasForeignKey(UserLogins => UserLogins.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(AppUsers => AppUsers.UserTokens)
            .WithOne()
            .HasForeignKey(UserTokens => UserTokens.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(AppUsers => AppUsers.UserRoles)
            .WithOne()
            .HasForeignKey(UserRoles => UserRoles.UserId)
            .IsRequired();
    }
}