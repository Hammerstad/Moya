using System;
using System.Diagnostics.CodeAnalysis;

namespace Moya.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is designed as an extensibility point.")]
    
    class RunTimesAttribute : Attribute
    {
        int _times;

        public RunTimesAttribute(int times)
        {
            _times = times;
        }
    }
}
