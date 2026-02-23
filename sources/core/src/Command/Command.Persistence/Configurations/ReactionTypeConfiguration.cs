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
                new ReactionType(
                    Guid.Parse("72B5C96B-4859-40E7-B87F-210440E629FD"),
                    "Like",
                    "heart"),

                new ReactionType(
                    Guid.Parse("EA58565B-1456-4169-BB47-37102C635710"),
                    "Unicorn",
                    "unicorn"),

                new ReactionType(
                    Guid.Parse("904D8BED-C0B6-4D28-90BB-B539D3074BC7"),
                    "ExplodingHead",
                    "exploding"),

                new ReactionType(
                    Guid.Parse("97DE8548-8026-4A6A-A1C7-C4B25A4E6E2E"),
                    "RaisedHands",
                    "hands"),

                new ReactionType(
                    Guid.Parse("31574891-5B59-49BD-A697-ECA5B6B5DC94"),
                    "Fire",
                    "fire")
            );
    }
}
