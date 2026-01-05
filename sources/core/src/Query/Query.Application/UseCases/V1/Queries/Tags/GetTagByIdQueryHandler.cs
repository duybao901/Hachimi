using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Tags;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Domain.Exceptions;

namespace Query.Application.UseCases.V1.Queries.Tags;
public sealed class GetTagByIdQueryHandler : IQueryHandler<Contract.Services.V1.Tags.Query.GetTagById, Response.TagResponse>
{
    private readonly IMongoRepository<Tag> _tagRepository;

    public GetTagByIdQueryHandler(IMongoRepository<Tag> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<Response.TagResponse>> Handle(Contract.Services.V1.Tags.Query.GetTagById request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.FindOneAsync(t => t.DocumentId == request.Id)
            ?? throw new TagException.TagNotFoundException(request.Id);

        var TagResponse = new Response.TagResponse(tag.DocumentId, tag.Name, tag.Description, tag.Color);

        return Result.Success(TagResponse);
    }
}
