namespace Moya.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LongerThanAttribute : MoyaAttribute
    {
        public LongerThanAttribute(int seconds)
        {
            Seconds = seconds;
        }

        public LongerThanAttribute()
        {
            
        }

        public bool Strict { get; set; }
        public int Seconds { get; set; }
    }
}