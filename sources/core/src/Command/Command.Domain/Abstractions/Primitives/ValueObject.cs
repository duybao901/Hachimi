namespace Command.Domain.Abstractions.Primitives;
public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetAtomicValue();

    public bool Equals(ValueObject? orther)
    {
        return orther is not null && ValuesAreEqual(orther);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }

    private bool ValuesAreEqual(ValueObject orther)
    {
        return GetAtomicValue().SequenceEqual(orther.GetAtomicValue());
    }

    public override int GetHashCode()
    {
        return GetAtomicValue().Aggregate(default(int), HashCode.Combine);
    }
}
