using System.ComponentModel.DataAnnotations;

namespace Command.Infrastructure.DependencyInjection.Options;
public class MessageBusOption
{
    [Required, Range(1, 10)] public int RetryLimit { get; init; }
    [Required, Timestamp] public TimeSpan InitialInterval { get; init; }
    [Required, Timestamp] public TimeSpan IntervalIncrement { get; init; }
}
