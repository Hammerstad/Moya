namespace Moya.Attributes
{
    using System;
    using Factories;

    /// <summary>
    /// Attribute that is applied to a method indicating that it contains
    /// configuration which should be applied to all tests in this file.
    /// The attributed method should have a single <see cref="IMoyaTestRunnerFactory"/>
    /// as parameter.
    /// <example>
    /// <code>
    /// [MoyaConfiguration]
    /// public void MyConfigMethod(IMoyaTestRunnerFactory testRunnerFactory)
    /// {
    ///     testRunnerFactory.AddTestRunnerForAttribute(...,...);
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MoyaConfigurationAttribute : MoyaAttribute
    {
         
    }
}