namespace Moya.Dummy.Test.Project
{
    using System.Threading;
    using Attributes;

    public class TestClass
    {
        [Stress(Users = 15)]
        [Result(SlowerThan = 1)]
        public void AMethod()
        {
            Thread.Sleep(1);
        }

        [Stress(Users = 6, Times = 1000)]
        [Result(QuickerThan = 100)]
        public void BMethod()
        {
            Thread.Sleep(1);
        }

        [Warmup(Duration = 10)]
        [Stress(Users = 13, Times = 500)]
        [Result(SlowerThan = 1, QuickerThan = 100)]
        public void CMethod()
        {
            Thread.Sleep(1);
        }
    }
}