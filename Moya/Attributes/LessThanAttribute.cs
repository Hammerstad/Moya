namespace Moya.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LessThanAttribute : MoyaAttribute
    {
        public LessThanAttribute(int seconds)
        {
            Seconds = seconds;
        }

        public LessThanAttribute()
        {

        }

        public int Seconds { get; set; }
    }
}