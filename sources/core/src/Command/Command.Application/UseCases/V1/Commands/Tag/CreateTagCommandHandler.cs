using Command.Domain.Abstractions.Repositories;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;

namespace Command.Application.UseCases.V1.Commands.Tag;
public sealed class CreateTagCommandHandler : ICommandHandler<Contract.Services.V1.Tags.Command.CreateTagCommand>
{
    private readonly IRepositoryBase<Tags, Guid> _tagRepository;

    public CreateTagCommandHandler(IRepositoryBase<Tags, Guid> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result> Handle(Contract.Services.V1.Tags.Command.CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = Tags.CreateTag(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.Color
            );

        _tagRepository.Add(tag);

        return Result.Success("Create Tag Success!");
    }
}
