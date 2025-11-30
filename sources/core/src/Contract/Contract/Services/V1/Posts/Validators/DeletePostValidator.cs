using FluentValidation;

namespace Contract.Services.V1.Posts.Validators;
public class DeletePostValidator : AbstractValidator<Command.DeletePostCommand>
{
    public DeletePostValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}
