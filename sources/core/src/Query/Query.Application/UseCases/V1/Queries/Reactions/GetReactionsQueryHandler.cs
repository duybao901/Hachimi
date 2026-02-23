using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Reaction;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using static Contract.Services.V1.Reaction.Query;

namespace Query.Application.UseCases.V1.Queries.Reactions;
public sealed class GetReactionsQueryHandler : IQueryHandler<GetReactionsQuery, List<Response.Reaction>>
{
    private readonly IMongoRepository<Reaction> _reactionMongoRepository;

    public GetReactionsQueryHandler(IMongoRepository<Reaction> reactionMongoRepository)
    {
        _reactionMongoRepository = reactionMongoRepository;
    }

    public async Task<Result<List<Response.Reaction>>> Handle(GetReactionsQuery request, CancellationToken cancellationToken)
    {
        var reactions = await _reactionMongoRepository.FindAll();

        var response = reactions.Select(r => new Response.Reaction(r.DocumentId, r.Name, r.Icon)).ToList();

        return Result.Success(response);
    }
}
