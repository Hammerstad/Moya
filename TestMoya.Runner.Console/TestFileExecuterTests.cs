namespace TestMoya.Runner.Console
{
    using System;
    using System.IO;
    using System.Reflection;
    using Moya.Runner.Console;
    using Xunit;

    public class TestFileExecuterTests
    {
        public class Ctor
        {
            [Fact]
            public void CallingWithInvalidAssemblyPathThrowsException()
            {
                string invalidAssemblyPath = @"C:\hopefully\not\existing.dll";

                var exception = Record.Exception(() => new TestFileExecuter(invalidAssemblyPath));

                Assert.NotNull(exception);
                Assert.IsType<FileNotFoundException>(exception);
            }

            [Fact]
            public void CallingWithValidAssemblyPathDoesNotThrowException()
            {
                string validAssemblyPath = GetMoyaDummyTestProjectDllPath();

                var exception = Record.Exception(() => new TestFileExecuter(validAssemblyPath));

                Assert.Null(exception);
            }
        }

        public class RunAllTests
        {
            [Fact]
            public void RunningAllTestsFillsTestResultsList()
            {
                string validAssemblyPath = GetMoyaDummyTestProjectDllPath();
                TestFileExecuter testFileExecuter = new TestFileExecuter(validAssemblyPath);

                testFileExecuter.RunAllTests();

                Assert.NotEmpty(testFileExecuter.TestResults);
            }
        }

        private static string GetCurrentAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
        private static string GetMoyaDummyTestProjectDllPath()
        {
#if DEBUG
            return GetCurrentAssemblyDirectory() + @"\..\..\..\Moya.Dummy.Test.Project\bin\Debug\Moya.Dummy.Test.Project.dll";
#else
            return GetCurrentAssemblyDirectory() + @"\..\..\..\Moya.Dummy.Test.Project\bin\Release\Moya.Dummy.Test.Project.dll";
#endif
        }
    }
}