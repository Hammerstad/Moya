using System;

namespace Moya.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DurationShouldBeAttribute : Attribute, IMoyaAttribute
    {
        public int LessThanOrEqualTo { get; set; }
        public int GreaterThanOrEqualTo { get; set; }
    }
}