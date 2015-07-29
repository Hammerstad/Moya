namespace Moya.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LoadTestAttribute : MoyaAttribute
    {
        private int threads = 1;
        private int times = 1;

        public int Threads
        {
            get { return threads; }
            set { threads = value; }
        }

        public int Times
        {
            get { return times; }
            set { times = value; }
        }
    }
}
