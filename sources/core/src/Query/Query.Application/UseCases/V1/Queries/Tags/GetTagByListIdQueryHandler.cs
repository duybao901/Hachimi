using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Tags;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;


namespace Query.Application.UseCases.V1.Queries.Tags;
public sealed class GetTagByListIdQueryHandler : IQueryHandler<Contract.Services.V1.Tags.Query.GetTagByListId, List<Response.TagResponse>>
{
    private readonly IMongoRepository<Tag> _tagRepository;

    public GetTagByListIdQueryHandler(IMongoRepository<Tag> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<List<Response.TagResponse>>> Handle(Contract.Services.V1.Tags.Query.GetTagByListId request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.FindAll(tag => request.TagIds.Contains(tag.DocumentId));

        var tagResponses = new List<Response.TagResponse>();

        foreach (var tag in tags)
        {
            tagResponses.Add(new Response.TagResponse(
                tag.DocumentId, tag.Name, tag.Description, tag.Color));
        }

        return Result.Success(tagResponses);
    }
}
