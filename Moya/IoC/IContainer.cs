namespace Moya.IoC
{
    public interface IContainer
    {
        TClass Register<TClass>(TClass element) where TClass : class;

        TClass Register<TInterface, TClass>(TClass element) where TClass : class, TInterface;

        TInterface Resolve<TInterface>();

        bool Contains<TInterface>();

        TClass TryRegister<TInterface, TClass>(TClass element) where TClass : class, TInterface;
    }
}