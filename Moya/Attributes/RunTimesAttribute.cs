using System;

namespace Moya.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
     class RunTimesAttribute : Attribute
    {
        int _times;

        public RunTimesAttribute(int times)
        {
            _times = times;
        }
    }
}
