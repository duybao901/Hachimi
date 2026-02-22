using Command.Domain.Abstractions.Entities;

namespace Command.Domain.Entities;

public sealed class ReactionType: DomainEntity<Guid>
{
    public Guid Id { get; private set; }

    public string Name { get; set; } = default!;

    public string Icon { get; set; } = default!;

    private ReactionType() { }

    public ReactionType(string name, string icon)
    {
        Id = Guid.NewGuid();
        Name = name;
        Icon = icon;
    }
}