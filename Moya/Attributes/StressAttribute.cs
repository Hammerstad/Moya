namespace Moya.Attributes
{
    using System;

    /// <summary>
    /// Attribute that is applied to a method indicating that it should 
    /// have a stress phase, which executes the given method multiple times.
    /// The attributed method can also be executed in parallel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class StressAttribute : MoyaAttribute
    {
        /// <summary>
        /// Indicates how many times the attributed method will be run in parallel.
        /// This can also be enhanced with sequential execution by specifying the 
        /// <see cref="Users"/> attribute.
        /// </summary>
        /// <example>
        /// This sample shows a method run four times after each other, by two users
        /// (twice in parallel), for a total of eight executions.
        /// <code>
        /// class TestClass 
        /// { 
        ///     [Stress(Times = 4, Users = 2)]
        ///     static void MyMethod()  
        ///     { 
        ///         ...
        ///     } 
        /// } 
        /// </code>
        /// </example>
        public int Users { get; set; } = 1;

        /// <summary>
        /// Indicates how many times the attributed method will be run in sequence.
        /// This can also be enhanced with parallel execution by specifying the 
        /// <see cref="Users"/> attribute.
        /// </summary>
        /// <example>
        /// This sample shows a method run four times after each other, by two users
        /// (twice in parallel), for a total of eight executions.
        /// <code>
        /// class TestClass 
        /// { 
        ///     [Stress(Times = 4, Users = 2)]
        ///     static void MyMethod()  
        ///     { 
        ///         ...
        ///     } 
        /// } 
        /// </code>
        /// </example>
        public int Times { get; set; } = 1;
    }
}
