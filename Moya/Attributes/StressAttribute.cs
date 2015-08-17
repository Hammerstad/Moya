namespace Moya.Attributes
{
    using System;

    /// <summary>
    /// Attribute that is applied to a method indicating that it should 
    /// have a stress phase, which executes the given method multiple times.
    /// The attributed method can also be executed in parallel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StressAttribute : MoyaAttribute
    {
        /// <summary>
        /// The amount of times the attributed method will be run in parrallel.
        /// </summary>
        private int users = 1;

        /// <summary>
        /// The amount of times the attributed method will be run in sequence.
        /// </summary>
        private int times = 1;

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
        public int Users
        {
            get { return users; }
            set { users = value; }
        }

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
        public int Times
        {
            get { return times; }
            set { times = value; }
        }
    }
}
