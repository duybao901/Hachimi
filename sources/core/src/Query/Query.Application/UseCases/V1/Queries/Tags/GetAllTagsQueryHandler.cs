using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Tags;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Queries.Tags;
public sealed class GetAllTagsQueryHandler : IQueryHandler<Contract.Services.V1.Tags.Query.GetAllTags, List<Response.TagResponse>>
{
    private readonly IMongoRepository<Tag> _tagRepository;

    public GetAllTagsQueryHandler(IMongoRepository<Tag> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<List<Response.TagResponse>>> Handle(Contract.Services.V1.Tags.Query.GetAllTags request, CancellationToken cancellationToken)
    {

        var tags = await _tagRepository.FindAll();
        var tagResponses = new List<Response.TagResponse>();

        foreach(var tag in tags)
        {
            tagResponses.Add(new Response.TagResponse(
                tag.DocumentId, tag.Name, tag.Description, tag.Color));
        }

        return Result.Success(tagResponses);
    }
}
