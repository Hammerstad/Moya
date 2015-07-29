using System;
using System.Reflection;
using Moya.Attributes;
using Moya.Runner.Runners;
using Moya.Runner.Statistics;

namespace ConsoleApplication1
{
    public class OldTestProgram
    {

        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static void Add(int a, int b, int c)
        {
        }

        [Stress(Users = 2, Times = 5)]
        public void Test123()
        {

        }

        public static void PrintLoadTestInfo(MemberInfo t)
        {

            // Using reflection.
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);  // Reflection. 

            // Displaying output. 
            foreach (System.Attribute attr in attrs)
            {
                if (attr is StressAttribute)
                {
                    StressAttribute a = (StressAttribute)attr;
                    System.Console.WriteLine("Author information for {0}", t);
                    System.Console.WriteLine("Times: {0} \t::\t Runners: {1}", a.Times, a.Users);
                }
            }
        }

        public static void TestMethod1()
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

            Console.WriteLine("\n\n");

            MethodInfo[] methods = typeof(Program).GetMethods();
            foreach (var method in methods)
            {
                PrintLoadTestInfo(method);
            }

            Console.Read();
        } 
    }
}