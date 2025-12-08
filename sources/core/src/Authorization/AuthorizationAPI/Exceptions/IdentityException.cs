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

    public class UserNotExistsException : BadRequestException
    {
        public UserNotExistsException(string email)
            : base($"A user with the email {email} not exists.")
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

    public class confirmPassWordDoNotMatchException : BadRequestException
    {
        public confirmPassWordDoNotMatchException()
            : base("Confirm password do not match")
        {
        }
    }

    public class WrongPassWordException : BadRequestException
    {
        public WrongPassWordException()
            : base("The password is incorrect")
        {
        }
    }
}
