using System;
using System.Reflection;

namespace Moya.Runner
{
    public interface IAttributeDiscoverer
    {
        void DiscoverAttributesForMethod(MethodInfo method, Type type = null);
        void DiscoverAttributesForClass(Type type);
    }
}