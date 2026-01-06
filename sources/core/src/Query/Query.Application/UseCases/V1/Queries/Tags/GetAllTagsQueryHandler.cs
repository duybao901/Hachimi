using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Tags;
using MongoDB.Driver.Linq;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Queries.Tags;
public sealed class GetAllTagsQueryHandler : IQueryHandler<Contract.Services.V1.Tags.Query.SearchTags, List<Response.TagResponse>>
{
    private readonly IMongoRepository<Tag> _tagRepository;

    public GetAllTagsQueryHandler(IMongoRepository<Tag> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result<List<Response.TagResponse>>> Handle(Contract.Services.V1.Tags.Query.SearchTags request, CancellationToken cancellationToken)
    {
        var queryable = _tagRepository
           .AsQueryable(null);

        // Search
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));
        }

        var tagsSearch = await queryable.ToListAsync();
        var tagResponses = new List<Response.TagResponse>();

        foreach(var tag in tagsSearch)
        {
            tagResponses.Add(new Response.TagResponse(
                tag.DocumentId, tag.Name, tag.Description, tag.Color));
        }

        return Result.Success(tagResponses);
    }
}
