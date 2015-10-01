namespace Moya.Utility
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Utility class used to get <see cref="MethodInfo"/> from external
    /// assembly files.
    /// </summary>
    internal class AssemblyHelper
    {
        private readonly Assembly assembly;

        /// <summary>
        /// Creates an <see cref="AssemblyHelper"/> for a dll.
        /// </summary>
        /// <param name="assemblyPath">The path to the dll.</param>
        internal AssemblyHelper(string assemblyPath)
        {
            assembly = Assembly.LoadFile(assemblyPath);
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
            Type type = assembly.GetType(fullClassName);
            if (type != null)
            {
                return type.GetMethod(methodName);
            }
            return null;
        }
    }
}