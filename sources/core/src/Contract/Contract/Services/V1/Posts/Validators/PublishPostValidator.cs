using FluentValidation;

namespace Contract.Services.V1.Posts.Validators;
public class PublishPostValidator : AbstractValidator<Command.PublishPostCommand>
{
    public PublishPostValidator()
    {
        RuleFor(p => p.Title)
            .MaximumLength(250).WithMessage("Title maximum length is 250")
            .NotEmpty();
        RuleFor(p => p.Content).NotEmpty();
        RuleFor(p => p.TagIds).NotEmpty();
    }
}
