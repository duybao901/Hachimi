using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Persistence.Seed;
public sealed class ReactionTypeMongoSeeder : IDataSeeder
{
    private readonly IMongoRepository<Reaction> _reactionRepository;

    public ReactionTypeMongoSeeder(IMongoRepository<Reaction> reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var exists = await _reactionRepository.FindOneAsync(
        x => x.DocumentId == Guid.Parse("72B5C96B-4859-40E7-B87F-210440E629FD"));

        if (exists != null)
            return;

        await _reactionRepository.InsertManyAsync(GetSeedData());
    }

    private static List<Reaction> GetSeedData()
        => new()
        {
            new() { DocumentId = Guid.Parse("72B5C96B-4859-40E7-B87F-210440E629FD"), Name = "Like", Icon = "heart" },
            new() { DocumentId = Guid.Parse("EA58565B-1456-4169-BB47-37102C635710"), Name = "Unicorn", Icon = "unicorn" },
            new() { DocumentId = Guid.Parse("904D8BED-C0B6-4D28-90BB-B539D3074BC7"), Name = "ExplodingHead", Icon = "exploding" },
            new() { DocumentId = Guid.Parse("97DE8548-8026-4A6A-A1C7-C4B25A4E6E2E"), Name = "RaisedHands", Icon = "hands" },
            new() { DocumentId = Guid.Parse("31574891-5B59-49BD-A697-ECA5B6B5DC94"), Name = "Fire", Icon = "fire" }
        };
}
