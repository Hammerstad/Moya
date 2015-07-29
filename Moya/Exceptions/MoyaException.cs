namespace Moya.Exceptions
{
    using System;

    public class MoyaException : Exception
    {
        public MoyaException()
        {
            
        }

        public MoyaException(String message) : base(message)
        {
            
        }

        public MoyaException(String message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}