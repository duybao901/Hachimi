using Command.Domain.Entities;
using Command.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations;

internal class PostConfiguration : IEntityTypeConfiguration<Posts>
{
    public void Configure(EntityTypeBuilder<Posts> builder)
    {
        builder.ToTable(TableNames.Post);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.PostStatus)
            .IsRequired();

        builder.Property(x => x.ViewCount)
            .HasDefaultValue(0);

        builder.Property(x => x.CommentCount)
            .HasDefaultValue(0);

        builder.Property(x => x.LikeCount)
            .HasDefaultValue(0);

        builder.Property(x => x.FeedScore)
            .HasDefaultValue(0);

        builder.Property(x => x.PublishedAt)
            .HasPrecision(3);

        // Feed indexes
        builder.HasIndex(x => new { x.PostStatus, x.PublishedAt });
        builder.HasIndex(x => new { x.PostStatus, x.FeedScore });
    }

}