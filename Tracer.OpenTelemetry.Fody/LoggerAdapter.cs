namespace Tracer.OpenTelemetry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using global::OpenTelemetry.Trace;
    using JetBrains.Annotations;
    using TracerAttributes;
    using Util;

    public partial class LoggerAdapter
    {
        private static readonly ActivitySource ActivityTracer = new(ActivitySourceConstants.Name, ActivitySourceConstants.Version);
        private readonly string? name;

        /// <summary>
        ///     Called by Tracer.Fody, not the end user. A <see cref="LoggerAdapter"/> is created for every class/type.
        /// </summary>
        [PublicAPI /* defined by Tracer.Fody */]
        public LoggerAdapter(Type? containingType)
        {
            this.name = containingType != null ? (containingType.Name + ".") : null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member - https://github.com/dotnet/roslyn/issues/54103
        public partial void TraceEnter(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            string? methodInfo,
            Tuple<string, string>?[]? configParameters,
            string?[]? paramNames,
            object?[]? paramValues)
        {
            if (!ActivityTracer.HasListeners()) return;

            var activity = ActivityTracer.StartActivity($"{this.name}{methodInfo}");
            if (activity == null) return;

            if (ShouldIncludeArguments(configParameters))
            {
                IncludeArgumentsAsTags(activity, paramNames, paramValues);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member - https://github.com/dotnet/roslyn/issues/54103
        public partial void TraceLeave(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            string? methodInfo,
            Tuple<string, string>?[]? configParameters,
            long startTicks,
            long endTicks,
            string?[]? paramNames,
            object?[]? paramValues)
        {
            // TODO [enhance] .NET team would rather we store the activeScope from TraceEnter as a variable, than use Activity.Current. Postponing that change for now due to complexity, can revisit if we see performance issues.
            var activity = Activity.Current;
            if (activity == null) return;
            
            FindAndRecordAnyUncaughtException(activity, paramNames, paramValues);
            
            if (ShouldIncludeReturnValue(configParameters))
            {
                IncludeReturnValueAsTag(activity, paramNames, paramValues);
            }

            activity.Dispose();
        }

        private static void FindAndRecordAnyUncaughtException(Activity? activeScope, string?[]? paramNames, object?[]? paramValues)
        {
            if (paramNames != null)
            {
                int i = 0;

                for (; i < paramNames.Length; i++)
                {
                    if (string.Equals(TracerFodyConstants.ExceptionMarker, paramNames[i]))
                    {
                        break;
                    }
                }

                if (i < paramNames.Length)
                {
                    // Found match
                    var exception = paramValues?[i] as Exception;

                    activeScope.RecordException(exception);
                }
            }
        }

        private static bool ShouldIncludeArguments(Tuple<string, string>?[]? configParameters)
        {
            var includeArgumentsParameter = configParameters
                ?.FirstOrDefault(tup => tup?.Item1 == nameof(TraceOn.IncludeArguments))
                ?.Item2;
            var includeArguments = includeArgumentsParameter == null ? false : bool.Parse(includeArgumentsParameter);
            return includeArguments;
        }

        private static void IncludeArgumentsAsTags(Activity activity, string?[]? paramNames, object?[]? paramValues)
        {
            if (paramNames == null) return;

            for (int paramIndex = 0; paramIndex < paramNames.Length; paramIndex++)
            {
                // Prepending Arguments. because Jaeger alphabetizes the tags, so it was hard to discover them
                string paramName = "arguments." + paramNames[paramIndex];
                object? paramValue = paramValues?[paramIndex];
                // TODO: [enhance] Support other forms of serialization
                string? serializedParamValue = paramValue?.ToString();

                // TODO: [simplify] This supports 'object' directly, could remove serialization at this level, depends how well documented adding serialization at any other level is
                activity.AddTag(paramName, serializedParamValue);
            }
        }

        private static bool ShouldIncludeReturnValue(Tuple<string, string>?[]? configParameters)
        {
            var includeReturnValueParameter = configParameters
                ?.FirstOrDefault(tup => tup?.Item1 == nameof(TraceOn.IncludeReturnValue))
                ?.Item2;
            var includeReturnValue = includeReturnValueParameter == null ? false : bool.Parse(includeReturnValueParameter);
            return includeReturnValue;
        }

        private static void IncludeReturnValueAsTag(Activity activeScope, string?[]? paramNames, object?[]? paramValues)
        {
            if (paramNames != null)
            {
                int i = 0;

                for (; i < paramNames.Length; i++)
                {
                    if (string.Equals(TracerFodyConstants.ReturnValueMarker, paramNames[i]))
                    {
                        break;
                    }
                }

                if (i < paramNames.Length)
                {
                    // Found match
                    var returnValue = paramValues?[i];

                    activeScope.AddEvent(
                        new ActivityEvent(
                            "Logging return value",
                            tags: new ActivityTagsCollection(
                                new[]
                                {
                                    new KeyValuePair<string, object?>(
                                        "ReturnValue",
                                        returnValue
                                        // We observed data omitted in Jaeger UI when actual .Value is null, so we inject <null>
                                        ?? "<null>")
                                })));
                }
            }
        }

    }
}