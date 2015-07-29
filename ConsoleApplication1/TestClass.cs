namespace ConsoleApplication1
{
    using System;
    using Moya.Attributes;

    public class TestClass
    {
        [Stress(Users = 15)]
        [Result(SlowerThan = 1)]
        public void AMethod()
        {
            Console.WriteLine("A");
        }

        [Stress(Users = 6, Times = 1000)]
        [Result(QuickerThan = 100)]
        public void BMethod()
        {
            Console.WriteLine("B");
        }

        [Warmup(Duration = 10)]
        [Stress(Users = 13, Times = 500)]
        [Result(SlowerThan = 1, QuickerThan = 100)]
        public void CMethod()
        {
            Console.WriteLine("C");
        }
    }
}