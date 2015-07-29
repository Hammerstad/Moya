namespace Moya.Attributes
{
    using System;
    using Models;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResultAttribute : MoyaAttribute
    {
        public int QuickerThan { get; set; }

        public int SlowerThan { get; set; }

        public Scalability Scales { get; set; }
    }
}