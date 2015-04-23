using System;

namespace Moya.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LoadTestAttribute : Attribute
    {
        public int Runners { get; set; }
        public int Times { get; set; }
    }
}
