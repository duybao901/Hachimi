using System.Reflection;

namespace ProjectionWorker;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
