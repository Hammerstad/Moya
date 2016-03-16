namespace Moya.Models
{
    /// <summary>
    /// Represents the type of a Moya test, e.g. <see cref="F:TestType.PreTest"/>.
    /// </summary>
    public enum TestType
    {
        /// <summary>
        /// Represents a test run in the pre-test phase
        /// </summary>
        PreTest,

        /// <summary>
        /// Represents a test run in the main-test phase
        /// </summary>
        Test,

        /// <summary>
        /// Represents a test run in the post-test phase
        /// </summary>
        PostTest
    }
}