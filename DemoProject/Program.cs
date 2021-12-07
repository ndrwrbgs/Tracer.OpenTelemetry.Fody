namespace DemoProject.OpenTelemetry
{
    using System;
    using global::OpenTelemetry;
    using global::OpenTelemetry.Trace;
    using Tracer.OpenTelemetry;
    using TracerAttributes;

    class Program
    {
        static void Main(string[] args)
        {
            // We just AddSource and it connects, right? What do we do with the result?
            _ = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySourceConstants.Name)
                .AddConsoleExporter()
                .Build();

            Abc(Environment.CurrentDirectory, Environment.CommandLine);
        }

        public static string d => "abc";
        
        [TraceOn(Target = TraceTarget.Private, IncludeReturnValue = true, IncludeArguments = true)]
        public static string Abc(string a, [NoTrace] string b)
        {
            _ = d;
            return "foo";
        }
    }
}
