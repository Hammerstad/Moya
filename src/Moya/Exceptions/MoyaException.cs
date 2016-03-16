namespace Moya.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occur during execution of Moya.
    /// </summary>
    public class MoyaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoyaException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public MoyaException(string message) : base(message)
        {

        }
    }
}