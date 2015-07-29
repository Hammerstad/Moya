namespace Moya.Exceptions
{
    using System;

    public class MoyaAttributeNotFoundException : MoyaException
    {
        public MoyaAttributeNotFoundException()
        {
            
        }

        public MoyaAttributeNotFoundException(String message) : base(message)
        {
            
        }

        public MoyaAttributeNotFoundException(String message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}