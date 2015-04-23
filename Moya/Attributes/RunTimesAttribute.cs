using System;

namespace Moya.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RunTimesAttribute : Attribute
    {
        int _times;

        public RunTimesAttribute(int times)
        {
            _times = times;
        }
    }
}
