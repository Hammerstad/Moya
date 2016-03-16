namespace TestMoya.Utility
{
    using System;
    using System.IO;
    using System.Reflection;
    using Moya.Utility;
    using Shouldly;
    using Xunit;

    public class AssemblyHelperTests
    {
        public class Ctor
        {
            [Fact]
            public void InvalidAssemblyPathThrowsFileNotFoundException()
            {
                var exception = Record.Exception(() => new AssemblyHelper("C:/invalid/path/to/a.dll"));

                Assert.Equal(typeof(FileNotFoundException), exception.GetType());
                exception.Message.ShouldStartWith("The system cannot find the file specified.");
            }

            [Fact]
            public void ValidAssemblyPathDoesNotThrowException()
            {
                var pathToDummyDll = GetMoyaDummyTestProjectDllPath();

                var exception = Record.Exception(() => new AssemblyHelper(pathToDummyDll));

                Assert.Null(exception);
            }
        }

        public class GetMethodFromAssembly
        {
            private AssemblyHelper assemblyHelper;

            [Fact]
            public void ValidMethodReturnsMethodInfo()
            {
                var pathToDummyDll = GetMoyaDummyTestProjectDllPath();
                assemblyHelper = new AssemblyHelper(pathToDummyDll);
                const string MethodName = "AMethod";
                const string FullClassName = "Moya.Dummy.Test.Project.TestClass";

                var method = assemblyHelper.GetMethodFromAssembly(FullClassName, MethodName);

                Assert.Equal(MethodName, method.Name);
                // ReSharper disable once PossibleNullReferenceException
                Assert.Equal(FullClassName, method.DeclaringType.FullName);
            }

            [Fact]
            public void InvalidClassNameReturnsNull()
            {
                var pathToDummyDll = GetMoyaDummyTestProjectDllPath();
                assemblyHelper = new AssemblyHelper(pathToDummyDll);
                const string MethodName = "AMethod";
                const string FullClassName = "Moya.Dummy.Test.Project.FakeTestClass";

                var method = assemblyHelper.GetMethodFromAssembly(FullClassName, MethodName);

                Assert.Null(method);
            }

            [Fact]
            public void ValidClassNameAndInvalidMethodNameReturnsNull()
            {
                var pathToDummyDll = GetMoyaDummyTestProjectDllPath();
                assemblyHelper = new AssemblyHelper(pathToDummyDll);
                const string MethodName = "AFakeMethod";
                const string FullClassName = "Moya.Dummy.Test.Project.TestClass";

                var method = assemblyHelper.GetMethodFromAssembly(FullClassName, MethodName);

                Assert.Null(method);
            }

            [Fact]
            public void InvalidClassNameAndInvalidMethodNameReturnsNull()
            {
                var pathToDummyDll = GetMoyaDummyTestProjectDllPath();
                assemblyHelper = new AssemblyHelper(pathToDummyDll);
                const string MethodName = "AFakeMethod";
                const string FullClassName = "Moya.Dummy.Test.Project.FakeTestClass";

                var method = assemblyHelper.GetMethodFromAssembly(FullClassName, MethodName);

                Assert.Null(method);
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