using System;

namespace Moya.xUnitExtensions.Exceptions
{
    public class TimingException : Exception
    {
        public TimingException()
        {
            
        }

        public TimingException(string message) : base(message)
        {
            
        }

        public TimingException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}