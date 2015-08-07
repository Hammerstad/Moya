namespace Moya.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WarmupAttribute : MoyaAttribute
    {
        public int Duration { get; set; }
    }
}