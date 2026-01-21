using FluentValidation;

namespace Contract.Services.V1.Posts.Validators;
public class SaveDraftValidator : AbstractValidator<Command.SaveDraftPostCommand>
{
    public SaveDraftValidator()
    {
        RuleFor(p => p.Title)
            .MaximumLength(250).WithMessage("Title maximum length is 250")
            .NotEmpty();
        RuleFor(p => p.Content).NotEmpty();
        RuleFor(p => p.TagIds).NotEmpty();
    }
}
