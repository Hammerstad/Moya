﻿namespace TestMoya.Runner.Console
{
    using System;
    using System.IO;
    using System.Reflection;
    using Moya.Runner.Console;
    using Xunit;

    public class ArgumentParserTests
    {
        public class ParseArguments
        {
            private ArgumentParser argumentParser;

            [Fact]
            public void VerboseIsFalseWhenNotSpecified()
            {
                argumentParser = new ArgumentParser(GetMoyaDummyTestProjectDllPath());

                argumentParser.ParseArguments();

                Assert.False(argumentParser.CommandLineOptions.Verbose);
            }

            [Fact]
            public void HelpIsFalseWhenNotSpecified()
            {
                argumentParser = new ArgumentParser(GetMoyaDummyTestProjectDllPath());

                argumentParser.ParseArguments();

                Assert.False(argumentParser.CommandLineOptions.Help);
            }

            [Fact]
            public void VerboseIsTrueWhenSpecified()
            {
                argumentParser = new ArgumentParser(GetMoyaDummyTestProjectDllPath(), "--verbose");

                argumentParser.ParseArguments();

                Assert.True(argumentParser.CommandLineOptions.Verbose);
            }

            [Fact]
            public void HelpIsTrueWhenSpecified()
            {
                argumentParser = new ArgumentParser(GetMoyaDummyTestProjectDllPath(), "--help");

                argumentParser.ParseArguments();

                Assert.True(argumentParser.CommandLineOptions.Help);
            }

            [Fact]
            public void HelpWithOptionValueThrowsException()
            {
                argumentParser = new ArgumentParser(GetMoyaDummyTestProjectDllPath(), "--help", "invalid help option");

                var exception = Record.Exception(() => argumentParser.ParseArguments());

                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
                Assert.Equal("Unknown command line option: invalid help option", exception.Message);
            }

            [Fact]
            public void VerboseWithOptionValueThrowsException()
            {
                argumentParser = new ArgumentParser(GetMoyaDummyTestProjectDllPath(), "--verbose", "invalid verbose option");

                var exception = Record.Exception(() => argumentParser.ParseArguments());

                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
                Assert.Equal("Unknown command line option: invalid verbose option", exception.Message);
            }

            [Fact]
            public void FirstArgumentNotAssemblyShouldThrowException()
            {
                argumentParser = new ArgumentParser("--verbose", GetMoyaDummyTestProjectDllPath());

                var exception = Record.Exception(() => argumentParser.ParseArguments());

                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
                Assert.Equal("You must specify at least one assembly.\nType --help for more information.", exception.Message);
            }

            [Fact]
            public void AssemblyFileDoesNotExistThrowsException()
            {
                const string fakeAssemblyPath = "/path/to/fake.dll";
                argumentParser = new ArgumentParser(fakeAssemblyPath);

                var exception = Record.Exception(() => argumentParser.ParseArguments());

                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
                Assert.Equal("File not found: " + fakeAssemblyPath, exception.Message);
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