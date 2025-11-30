using Command.Domain.Abstractions.Aggregates;
using Command.Domain.Abstractions.Entities;

namespace Command.Domain.Entities;
public class Post : AggregateRoot<Guid>, IAuditTableEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    // Tag, Background...

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public Post(Guid id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
    }

    public static Post CreatePost(Guid id, string title, string content)
    {
        var post = new Post(id, title, content);

        // You can add domain events or other initialization logic here if needed
        post.RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostCreatedEvent(Guid.NewGuid(), id, title, content));

        return post;
    }

    public void Update(string title, string content)
    {
        // check business rule here before update product
        Title = title;
        Content = content;

        // this -> tham chiếu đến đối tượng hiện tại (Product.Update)
        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostUpdatedEvent(Guid.NewGuid(),
            Id,
            Title,
            Content));
    }

    public void Delete()
    {
        // check business rule here before delete product
        // this -> tham chiếu đến đối tượng hiện tại (Product.Delete)
        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostDeletedEvent(Guid.NewGuid(), Id));
    }
}
