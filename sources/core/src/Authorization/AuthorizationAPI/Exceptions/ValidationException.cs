namespace AuthorizationApi.Exceptions;

public class ValidationException : DomainException
{
    public ValidationException(List<ValidationError> errors)
        : base("Validation Failure", "One or more validation errors occurred")
    {
        Errors = errors;
    }

    public List<ValidationError> Errors { get;  }
}

public record ValidationError(string Code, string Message);