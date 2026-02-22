using Command.Domain.Entities;
using Command.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations;
internal sealed class ReactionTypeConfiguration : IEntityTypeConfiguration<ReactionType>
{
    public void Configure(EntityTypeBuilder<ReactionType> builder)
    {
        builder.ToTable(TableNames.ReactionTypes);

        builder.HasData(
            new ReactionType("Like", "heart"),
            new ReactionType("Unicorn", "unicorn"),
            new ReactionType("ExplodingHead", "exploding"),
            new ReactionType("RaisedHands", "hands"),
            new ReactionType("Fire", "fire")
        );
    }
}
