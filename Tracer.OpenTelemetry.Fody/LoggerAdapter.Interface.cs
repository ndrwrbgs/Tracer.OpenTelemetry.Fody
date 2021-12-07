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
        /// <summary>
        ///     Called by Tracer.Fody, not the end user. Called at the start of the method. Woven into the `{` on the method.
        /// </summary>
        public partial void TraceEnter(
            // Using conservative nullability for first-pass. TODO [simplify] confirm nullability contracts from Tracer.Fody
            string? methodInfo,
            Tuple<string, string>?[]? configParameters,
            string?[]? paramNames,
            object?[]? paramValues);

        /// <summary>
        ///     Called by Tracer.Fody, not the end user. Called at the start of the method. Woven into the `}` on the method.
        /// </summary>
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