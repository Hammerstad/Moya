namespace Moya.Attributes
{
    using System;

    /// <summary>
    /// Attribute that is applied to a method indicating that it should 
    /// have a warmup phase before other tests are run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WarmupAttribute : MoyaAttribute
    {
        /// <summary>
        /// Sets the duration of the warmup phase. Value represents minutes.
        /// </summary>
        /// <example>
        /// This sample shows a method warming up for 60 seconds.
        /// <code>
        /// class TestClass 
        /// { 
        ///     [Warmup(Duration = 60)]
        ///     static void MyMethod()  
        ///     { 
        ///         ...
        ///     } 
        /// } 
        /// </code>
        /// </example>
        public int Duration { get; set; }
    }
}