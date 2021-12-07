namespace Tracer.OpenTelemetry
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Interfaces defined by Tracer.Fody
    /// </summary>
    [PublicAPI]
    public partial class LoggerAdapter
    {
        public partial void TraceEnter(
            // Using conservative nullability for first-pass. TODO [simplify] confirm nullability contracts from Tracer.Fody
            string? methodInfo,
            Tuple<string, string>?[]? configParameters,
            string?[]? paramNames,
            object?[]? paramValues);

        public partial void TraceLeave(
            // Using conservative nullability for first-pass. TODO [simplify] confirm nullability contracts from Tracer.Fody
            string? methodInfo,
            Tuple<string, string>?[]? configParameters,
            long startTicks,
            long endTicks,
            string?[]? paramNames,
            object?[]? paramValues);
    }
}