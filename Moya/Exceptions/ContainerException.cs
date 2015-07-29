namespace Moya.Exceptions
{
    using System;

    public class ContainerException : MoyaException
    {
         public ContainerException()
        {
            
        }

        public ContainerException(String message) : base(message)
        {
            
        }

        public ContainerException(String message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}