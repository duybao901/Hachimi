using Query.Domain.Exceptions;
using System.Diagnostics.Contracts;

namespace Query.Application.Exceptions;
public sealed class ValidationException : DomainException
{
    public ValidationException(IReadOnlyCollection<Contract.Abstractions.Shared.Error> errors)
      : base("Validation Failure", "One or more validation errors occurred")
      => Errors = errors;

    public IReadOnlyCollection<Contract.Abstractions.Shared.Error> Errors { get; }
}