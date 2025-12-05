using Command.Domain.Entities;
using Command.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations;
public class PostTagsConfiguration : IEntityTypeConfiguration<PostTags>
{
    public void Configure(EntityTypeBuilder<PostTags> builder)
    {
        builder.ToTable(TableNames.PostTags);

        builder.HasKey(pt => new { pt.PostId, pt.TagId });

        builder.HasOne<Posts>()
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);
        //.OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Tags>()
            .WithMany()
            .HasForeignKey(pt => pt.TagId);
        //.OnDelete(DeleteBehavior.Cascade);
    }
}
