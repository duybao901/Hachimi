namespace Command.Persistence.Contants;

public static class TableNames
{
    // For Outbox Pattern
    internal const string OutboxMessages = nameof(OutboxMessages);

    // *********** Singular Nouns ***********
    internal const string Post = nameof(Post);
    internal const string PostTags = nameof(PostTags);
    internal const string Tags = nameof(Tags);
}
