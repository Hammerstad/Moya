namespace Moya.Utility
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Utility class used to get <see cref="MethodInfo"/> from external
    /// _assembly files.
    /// </summary>
    internal class AssemblyHelper
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Creates an <see cref="AssemblyHelper"/> for a dll.
        /// </summary>
        /// <param name="assemblyPath">The path to the dll.</param>
        internal AssemblyHelper(string assemblyPath)
        {
            _assembly = Assembly.LoadFile(assemblyPath);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> from a specified class and method in
        /// a dll.
        /// </summary>
        /// <example>
        /// <code>
        /// const string MethodName = "AMethod";
        /// const string FullClassName = "My.NameSpace.TestClass";
        /// assemblyHelper = new AssemblyHelper(@"C:\Path\to\my\file.dll");
        /// MethodInfo methodInfo = assemblyHelper.GetMethodFromAssembly(FullClassName, MethodName);
        /// </code>
        /// </example>
        /// <param name="fullClassName">The full class name, including namespace.</param>
        /// <param name="methodName">The name of the desired method.</param>
        /// <returns></returns>
        public MethodInfo GetMethodFromAssembly(string fullClassName, string methodName)
        {
            Type type = _assembly.GetType(fullClassName);
            return type?.GetMethod(methodName);
        }
    }
}