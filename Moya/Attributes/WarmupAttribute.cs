namespace Moya.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WarmupAttribute : MoyaAttribute
    {
        private int times = 1;
        public int Duration { get; set; }

        public int Times
        {
            get { return times; }
            set { times = value; }
        }
    }
}