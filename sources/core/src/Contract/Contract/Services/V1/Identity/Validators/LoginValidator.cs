using FluentValidation;

namespace Contract.Services.V1.Identity.Validators;
public class LoginValidator : AbstractValidator<Query.Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email is required");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Email is required");
    }
}
