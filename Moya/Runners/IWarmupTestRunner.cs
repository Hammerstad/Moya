namespace Moya.Runners
{
    using Attributes;

    /// <summary>
    /// A test runner which runs a method decorated with a <see cref="WarmupAttribute"/>
    /// attribute for a given time without recording any statistics.
    /// </summary>
    internal interface IWarmupTestRunner : IPreTestRunner
    {
        /// <summary>
        /// How long the attributed method will be run. Measured in seconds.
        /// </summary>
        int Duration { get; set; } 
    }
}