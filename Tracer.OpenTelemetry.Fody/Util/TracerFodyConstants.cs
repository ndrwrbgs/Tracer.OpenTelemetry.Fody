namespace Tracer.OpenTelemetry
{
    internal static class TracerFodyConstants
    {
        /// <summary>
        /// From https://github.com/csnemes/tracer/blob/master/Tracer.Fody/Weavers/MethodWeaverBase.cs
        /// </summary>
        internal const string ExceptionMarker = "$exception";

        /// <summary>
        /// From https://github.com/csnemes/tracer/blob/master/Tracer.Fody/Weavers/MethodWeaverBase.cs
        /// </summary>
        internal const string? ReturnValueMarker = null;
    }
}