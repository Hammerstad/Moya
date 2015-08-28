namespace Moya.Models
{
    using System;

    /// <summary>
    /// Represents a Moya test case
    /// </summary>
    public class TestCase
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The path of the file containing the test case.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The name of the method to be executed.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The full class name of the method to be executed, e.g. "Moya.Models.TestCase"
        /// </summary>
        public string ClassName { get; set; }
    }
}