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
        var post = new Post(id, title, content)
        {
            CreatedOnUtc = DateTimeOffset.UtcNow
        };

        // You can add domain events or other initialization logic here if needed

        return post;
    }

    public void Update(string title, string content)
    {
        // check business rule here before update product
        Title = title;
        Content = content;

        // this -> tham chiếu đến đối tượng hiện tại (Product.Update)
        //RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductUpdatedEvent(Guid.NewGuid(),
        //    Id,
        //    name,
        //    price,
        //    description));
    }

    public void Delete()
    {
        // check business rule here before delete product
        // this -> tham chiếu đến đối tượng hiện tại (Product.Delete)
        //RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductDeletedEvent(Guid.NewGuid(), Id));
    }
}
