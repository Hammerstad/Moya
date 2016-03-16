namespace Moya.Dummy.Test.Project
{
    using System.Threading;
    using Moya.Attributes;

    public class TestClass
    {
        [Stress(Users = 15)]
        public void AMethod()
        {
            Thread.Sleep(1);
        }

        [Stress(Users = 6, Times = 1000)]
        public void BMethod()
        {
            Thread.Sleep(1);
        }

        [Warmup(1)]
        [Stress(Users = 13, Times = 500)]
        public void CMethod()
        {
            Thread.Sleep(1);
        }

        [Warmup(2)]
        public void OnlyWarmupMethod()
        {
            Thread.Sleep(1);
        }
    }
}