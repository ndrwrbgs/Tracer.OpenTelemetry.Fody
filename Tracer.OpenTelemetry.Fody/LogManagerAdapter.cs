namespace Tracer.OpenTelemetry
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Used by Tracer.Fody to create a Logger for each Type
    /// </summary>
    [PublicAPI]
    public static class LogManagerAdapter
    {
        /// <summary>
        ///     Defined and used by Tracer.Fody
        /// </summary>
        [PublicAPI]
        public static LoggerAdapter GetLogger(Type type)
        {
            return new LoggerAdapter(type);
        }
    }
}