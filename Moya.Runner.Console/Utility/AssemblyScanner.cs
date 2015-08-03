using System;
using System.Collections.Generic;
using System.Reflection;
using Moya.Attributes;

namespace Moya.Runner.Console.Utility
{
    public class AssemblyScanner
    {
        private readonly IDictionary<string, string> typeAndMethodnames = new Dictionary<string, string>();

        public IDictionary<string, string> GetTypenameAndMethodnameWithMoyaAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(MoyaAttribute), true).Length > 0)
                {
                    //typeAndMethodnames.Add(type.Name, );
                }
            }
            return null;
        } 
    }
}