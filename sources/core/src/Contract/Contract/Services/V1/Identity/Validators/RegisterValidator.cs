using FluentValidation;

namespace Contract.Services.V1.Identity.Validators;
public class RegisterValidator : AbstractValidator<Command.RegisterUser>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email is required");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            //.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            //.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            //.Matches("[0-9]").WithMessage("Password must contain at least one number")
            //.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
}
