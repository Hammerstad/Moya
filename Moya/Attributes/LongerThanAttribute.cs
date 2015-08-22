namespace Moya.Attributes
{
    using System;

    /// <summary>
    /// Attribute that is applied to a method indicating that it should 
    /// check if execution took longer time than a specified value.
    /// This check is applied after the main test phase.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class LongerThanAttribute : MoyaAttribute
    {
        /// <summary>
        /// Attribute that is applied to a method indicating that it should 
        /// check if execution took longer time than a specified value.
        /// This check is applied after the main test phase. 
        /// </summary>
        /// <param name="seconds">How long the main test phase is supposed to be as a minimum</param>
        public LongerThanAttribute(int seconds)
        {
            Seconds = seconds;
        }

        /// <summary>
        /// Indicates how long the main test phase should be.
        /// The check is non-strict, so the supplied value will also be accepted.
        /// </summary>
        /// <example>
        /// This sample shows a method run with the stress attribute, and later
        /// verified with the LongerThan attribute. The stress test should
        /// take longer than 4 seconds.
        /// <code>
        /// class TestClass 
        /// { 
        ///     [Stress(Times = 4, Users = 2)]
        ///     [LongerThan(Seconds = 4)]
        ///     static void MyMethod()  
        ///     { 
        ///         ...
        ///     } 
        /// } 
        /// </code>
        /// </example>
        public int Seconds { get; private set; }
    }
}