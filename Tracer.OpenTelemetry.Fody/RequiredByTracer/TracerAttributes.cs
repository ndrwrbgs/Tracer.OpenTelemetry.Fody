// ReSharper disable once CheckNamespace - Required to be specifically this by Tracer.Fody
namespace TracerAttributes
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Namespace and type are required for projects of Tracer.Fody.
    /// TODO: Suggest an .Interfaces style import for Tracer.Fody similar to ArgValidation.Fody
    /// </summary>
    // TODO: [enhance] extend AttributeUsage further after testing (see here: https://github.com/csnemes/tracer/search?q=traceon)
    [PublicAPI]
    [AttributeUsage(
        validOn: AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Struct, 
        AllowMultiple = true, 
        Inherited = true)]
    public class TraceOn : Attribute
    {
        public TraceTarget Target { get; set; }

        /* TODO: [enhance] Tracer.Fody integrated NoReturnTrace and NoTrace(on parameter) as first class, these custom properties here might not be necessary anymore if we find sensible knobs exist in the Weaver file
         Followed up from above. They are first class now, but cannot be opt'ed out of except at the attribute level. To keep private information private, we keep the current API of opt-in for _our_ library, while Tracer.Fody still enables NoTrace-on-parameter and NoReturnTrace to opt back out.
         E.g.
         [TraceOn(IncludeArguments = true)] // tells this library to emit to ActivitySource any arguments Tracer.Fody passes to it
         void MyMethod([NoTrace] string arg1, string arg2) // tells Tracer.Fody to not pass arg1 (arg2 will be traced due to line above)
        */
        public bool IncludeArguments { get; set; }

        public bool IncludeReturnValue { get; set; }

        public TraceOn()
        {
        }

        public TraceOn(TraceTarget traceTarget)
        {
            Target = traceTarget;
        }
    }
    
    [PublicAPI]
    [AttributeUsage(
        validOn: AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter, 
        AllowMultiple = true,
        Inherited = true)]
    public class NoTrace : Attribute
    {
    }

    [PublicAPI]
    [AttributeUsage(
        validOn: AttributeTargets.Method | AttributeTargets.Property, 
        AllowMultiple = false, 
        Inherited = true)]
    public class NoReturnTrace : Attribute
    {
    }

    [PublicAPI]
    public enum TraceTarget
    {
        Public,
        Internal,
        Protected,
        Private
    }
}