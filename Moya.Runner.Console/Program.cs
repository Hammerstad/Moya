using System;

namespace Moya.Runner.Console
{
    using Models;

    class Program
    {
        static void Main(string[] args)
        {
            ITestCaseExecuter testCaseExecuter = new TestCaseExecuter();
            testCaseExecuter.RunTest(new TestCase
            {
                ClassName = "ConsoleApplication1.TestClass",
                FilePath = @"C:\Users\emh\cygwin64\home\emh\workspace\Moya\ConsoleApplication1\bin\Debug\ConsoleApplication1.exe",
                Id = Guid.NewGuid(),
                MethodName = "CMethod"
            });
        }
    }
}
