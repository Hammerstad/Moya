﻿namespace Moya.Utility
{
    using System;
    using System.Reflection;

    public class AssemblyHelper
    {
        private readonly Assembly assembly;

        public AssemblyHelper(string assemblyPath)
        {
            assembly = Assembly.LoadFile(assemblyPath);
        }

        public MethodInfo GetMethodFromAssembly(string className, string methodName)
        {
            Type type = assembly.GetType(className);
            if (type != null)
            {
                return type.GetMethod(methodName);
            }
            return null;
        }
    }
}