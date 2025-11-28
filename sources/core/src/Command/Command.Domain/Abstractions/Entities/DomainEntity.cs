namespace Command.Domain.Abstractions.Entities;
public abstract class DomainEntity<Tkey> : IDomainEntity<Tkey>
{
    public virtual Tkey Id { get; set; }

    public bool IsDelete { get; protected set; }
}
