using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.UserProfiles;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Commands.UserProfiles;
public class ProjectUserProfileDetailsWhenUserProfileChangeEventHandler :
    ICommandHandler<DomainEvent.UserRegisterEvent>
{
    private readonly IMongoRepository<AuthorProjection> _authorRepository;

    public ProjectUserProfileDetailsWhenUserProfileChangeEventHandler(IMongoRepository<AuthorProjection> authorRepository)
    {
        _authorRepository = authorRepository;
    }


    public async Task<Result> Handle(DomainEvent.UserRegisterEvent request, CancellationToken cancellationToken)
    {
        var newAuthor = new AuthorProjection
        {
            DocumentId = request.Id,
            Name = request?.Name,
            UserName = request.UserName,
            Email = request.Email,
        };

        await _authorRepository.InsertOneAsync(newAuthor);

        return Result.Success();
    }
}
