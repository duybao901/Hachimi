using Command.Domain.Abstractions.Repositories;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using static Contract.Services.V1.Tags.Command;

namespace Command.Application.UseCases.V1.Commands.Tag;
public sealed class UpdateTagCommandHandler : ICommandHandler<UpdateTagCommand>
{
    private readonly IRepositoryBase<Tags, Guid> _tagReposityBase;

    public UpdateTagCommandHandler(IRepositoryBase<Tags, Guid> tagReposityBase)
    {
        _tagReposityBase = tagReposityBase;
    }

    public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagReposityBase.FindByIdAsync(request.Id)
            ?? throw new TagException.TagNotFoundException(request.Id);

        tag.UpdateTag(request.Name, request.Description, request.Color);
        _tagReposityBase.Update(tag);

        return Result.Success();
    }
}
