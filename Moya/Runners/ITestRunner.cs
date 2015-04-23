using System;

namespace Moya.Runners
{
    public interface ITestRunner
    {
        void Execute<T>(Func<T> function);
        void Execute(Action action); 
    }
}