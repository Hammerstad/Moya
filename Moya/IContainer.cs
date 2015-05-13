namespace Moya
{
    public interface IContainer
    {
        TClass Register<TClass>(TClass element) where TClass : class;

        TClass Register<TInterface, TClass>(TClass element) where TClass : class, TInterface;

        TInterface Resolve<TInterface>();

        bool Contains<TInterface>();

        TInterface RegisterAndResolve<TInterface, TClass>(TClass element) where TClass : class, TInterface;
    }
}