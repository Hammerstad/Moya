using System;
using Moya.Runners;
using Moya.Statistics;

namespace ConsoleApplication1
{
    class Program
    {
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static void Add(int a, int b, int c)
        {
        }

        public static void Main()
        {
            var decoratedLoadTestRunner = new TimerDecorator(new LoadTestRunner
            {
                Runners = 100,
                Times = 100
            });

            Action action = () => Add(2, 3, 4);
            Func<int> function = () => Add(2, 3);
            decoratedLoadTestRunner.Execute(action);
            decoratedLoadTestRunner.Execute(function);

            Console.WriteLine(DurationManager.DefaultInstance.GetDurationFromHashcode(action.GetHashCode()));
            Console.WriteLine(DurationManager.DefaultInstance.GetDurationFromHashcode(function.GetHashCode()));

            Console.Read();
        } 
    }
}
