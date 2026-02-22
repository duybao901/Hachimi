using Command.Persistence.Contants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Command.Domain.Entities;

internal class PostReactionConfiguration : IEntityTypeConfiguration<PostReaction>
{
    public void Configure(EntityTypeBuilder<PostReaction> builder)
    {
        builder.ToTable(TableNames.PostReactions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PostId)
               .IsRequired();

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.ReactionTypeId)
               .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
               .IsRequired();

        builder.HasOne(x => x.ReactionType)
               .WithMany()
               .HasForeignKey(x => x.ReactionTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Posts>()
               .WithMany()
               .HasForeignKey(x => x.PostId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.PostId, x.UserId })
               .IsUnique();


        builder.HasIndex(x => x.PostId);
    }
}