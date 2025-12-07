namespace AuthorizationAPI.Abstractions.Entities;
public interface IAuditTableEntity
{
    DateTimeOffset CreatedOnUtc { get; set; }

    DateTimeOffset? ModifiedOnUtc { get; set; }
}
