using System;
using Moya.Attributes;

namespace ConsoleApplication1
{
    public class TestClass
    {
        [LoadTest(Runners = 15)]
        [DurationShouldBe(GreaterThanOrEqualTo = 1)]
        public void AMethod()
        {
            Console.WriteLine("A");
        }

        [LoadTest(Runners = 6, Times = 1000)]
        [DurationShouldBe(LessThanOrEqualTo = 100)]
        public void BMethod()
        {
            Console.WriteLine("B");
        }

        [LoadTest(Runners = 13, Times = 500)]
        [DurationShouldBe(GreaterThanOrEqualTo = 1, LessThanOrEqualTo = 100)]
        public void CMethod()
        {
            Console.WriteLine("C");
        }
    }
}