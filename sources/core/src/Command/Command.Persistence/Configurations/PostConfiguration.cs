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
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.Content).IsRequired(true);
    }
}