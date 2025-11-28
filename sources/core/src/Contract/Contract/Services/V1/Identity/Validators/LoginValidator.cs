using FluentValidation;

namespace Contract.Services.V1.Identity.Validators;
public class LoginValidator : AbstractValidator<Query.Login>
{
    public LoginValidator()
    {
        RuleFor(p => p.UserName).NotEmpty();
        RuleFor(p => p.Password).NotEmpty();
    }
}
