using Microsoft.AspNetCore.Identity;

namespace AuthorizationApi.Exceptions;

public static class IdentityException
{
    public class TokenException : DomainException
    {
        public TokenException(string message) : base("Token Exception", message)
        {
        }
    }

    public class UserAlreadyExistsException : BadRequestException
    {
        public UserAlreadyExistsException(string email)
            : base($"A user with the email {email} already exists.")
        {
        }
    }

    public class UserCreationFailedException : Exception
    {
        public IReadOnlyList<IdentityError> Errors { get; }

        public UserCreationFailedException(IEnumerable<IdentityError> errors)
            : base("User creation failed")
        {
            Errors = errors.ToList();
        }
    }

    public class PasswordsDoNotMatchException : BadRequestException
    {
        public PasswordsDoNotMatchException()
            : base("The provided passwords do not match.")
        {
        }
    }
}
