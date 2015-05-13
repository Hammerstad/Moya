using System;

namespace Moya.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LoadTestAttribute : Attribute, IMoyaAttribute
    {
        private int runners = 1;
        private int times = 1;

        public int Runners
        {
            get { return runners; }
            set { runners = value; }
        }

        public int Times
        {
            get { return times; }
            set { times = value; }
        }
    }
}
