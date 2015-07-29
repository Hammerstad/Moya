namespace Moya.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StressAttribute : MoyaAttribute
    {
        private int users = 1;
        private int times = 1;

        public int Users
        {
            get { return users; }
            set { users = value; }
        }

        public int Times
        {
            get { return times; }
            set { times = value; }
        }
    }
}
