namespace Moya.Attributes
{
    using System;

    /// <summary>
    /// Attribute that is applied to a method indicating that it should 
    /// have a warmup phase before other tests are run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class WarmupAttribute : MoyaAttribute
    {
        /// <summary>
        /// The duration of the warmup phase. Value represents minutes.
        /// </summary>
        /// <example>
        /// This sample shows a method warming up for 60 seconds.
        /// <code>
        /// class TestClass 
        /// { 
        ///     [Warmup(60)]
        ///     static void MyMethod()  
        ///     { 
        ///         ...
        ///     } 
        /// } 
        /// </code>
        /// </example>
        public int Duration { get; private set; }

        /// <summary>
        /// Attribute that is applied to a method indicating that it should 
        /// have a warmup phase before other tests are run.
        /// </summary>
        /// <param name="duration">The duration of the warmup phase. Value represents minutes.</param>
        public WarmupAttribute(int duration)
        {
            Duration = duration;
        }
    }
}