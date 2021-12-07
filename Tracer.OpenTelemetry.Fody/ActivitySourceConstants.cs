namespace Tracer.OpenTelemetry
{
    /// <summary>
    ///     Values that will be used for creating the <see cref="System.Diagnostics.ActivitySource"/>,
    ///     subscribers must explicitly subscribe to events with these values, so they are exposed.
    /// </summary>
    public static class ActivitySourceConstants
    {
        /// <summary>
        ///     <see cref="System.Diagnostics.ActivitySource.Name"/>
        /// </summary>
        public static readonly string Name = "Tracer.OpenTelemetry.Fody";

        /// <summary>
        /// Sync with version information in the csproj
        /// </summary>
        public static readonly string Version = "3.3.1";
    }
}