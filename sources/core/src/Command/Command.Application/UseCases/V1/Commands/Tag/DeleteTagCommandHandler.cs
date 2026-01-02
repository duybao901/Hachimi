using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Microsoft.EntityFrameworkCore;

namespace Command.Application.UserCases.V1.Commands.Tag;
public sealed class DeleteTagCommandHandler : ICommandHandler<Contract.Services.V1.Tags.Command.DeleteTagCommand>
{
    private readonly IRepositoryBase<Tags, Guid> _tagRepositoryBase;
    private readonly IRepositoryBase<Posts, Guid> _postRepositoryBase;

    public DeleteTagCommandHandler(IRepositoryBase<Tags, Guid> tagRepositoryBase, IRepositoryBase<Posts, Guid> postRepositoryBase)
    {
        _tagRepositoryBase = tagRepositoryBase;
        _postRepositoryBase = postRepositoryBase;
    }

    public async Task<Result> Handle(Contract.Services.V1.Tags.Command.DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepositoryBase.FindByIdAsync(request.Id)
            ?? throw new TagException.TagNotFoundException(request.Id);

        var isUsed = await _postRepositoryBase
        .FindAll(p => p.PostTags.Any(pt => pt.TagId == request.Id))
        .AnyAsync();

        if(isUsed)
        {
            throw new TagException.TagAlreadyExitsInAnotherPost();
        }

        _tagRepositoryBase.Remove(tag);
        tag.DeleteTag();

        return Result.Success("Delete Tag Susccess");
    }
}
