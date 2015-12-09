namespace Moya.Runner.Console.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;

    public class AssemblyScanner
    {
        private readonly Assembly _assembly;

        public AssemblyScanner(string assemblyFilePath)
        {
            _assembly = Assembly.LoadFile(assemblyFilePath);
        }

        public IEnumerable<Type> GetTypes()
        {
            return _assembly.GetTypes();
        }

        public IEnumerable<MemberInfo> GetMembersWithMoyaAttribute(Type type)
        {
            return type.GetMembers()
                        .Where(x => x.GetCustomAttributes(typeof(MoyaAttribute), true).Length > 0)
                        .ToArray();
        } 
    }
}