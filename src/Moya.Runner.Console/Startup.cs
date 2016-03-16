namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;

    internal class Startup
    {
        private readonly ArgumentParser _argumentParser;

        public Startup(string[] arguments)
        {
            _argumentParser = new ArgumentParser(arguments);
        }

        internal void Run()
        {
            _argumentParser.ParseArguments();
            ShowHelpMessageAndExitIfSpecified();
            RunTestsForSpecifiedAssemblies();
        }

        private void RunTestsForSpecifiedAssemblies()
        {
            foreach (var assemblyFilePath in _argumentParser.CommandLineOptions.AssemblyFilePaths)
            {
                string normalizedPath = NormalizePath(assemblyFilePath);
                TestFileExecuter testFileExecuter = new TestFileExecuter(normalizedPath);
                List<ITestResult> testResults = testFileExecuter.RunAllTests();
                PrintTestResults(testResults);
            }
        }

        private static void PrintTestResults(List<ITestResult> testResults)
        {
            foreach (var testResult in testResults)
            {
                Console.WriteLine("TestResult: ");
                Console.WriteLine("\tDuration:\t" + testResult.Duration);
                Console.WriteLine("\tErrormessage:\t" + testResult.ErrorMessage);
                Console.WriteLine("\tOutcome:\t" + testResult.TestOutcome);
                Console.WriteLine("\tType:\t\t" + testResult.TestType);
            }
        }

        private void ShowHelpMessageAndExitIfSpecified()
        {
            if (_argumentParser.CommandLineOptions.Help)
            {
                ShowHelpMessage();
                Environment.Exit(0);
            }
        }

        private static void ShowHelpMessage()
        {
            Console.WriteLine("Usage: Moya.Runner.Console.exe ([options] <argument>) <path-to-dll>[;<path-to-other-dll>...]");
            Console.WriteLine("\nOptional options:");
            Console.WriteLine("\t-h\t--help\t\tPrints this message.");
            Console.WriteLine("\t-v\t--verbose\t\tPrints more information.");
        }

        private static string NormalizePath(string assemblyFilePath)
        {
            if (Path.IsPathRooted(assemblyFilePath))
            {
                return Path.GetFullPath(assemblyFilePath);
            }

            var currentDir = Directory.GetCurrentDirectory();
            var combinedPath = Path.Combine(currentDir, assemblyFilePath);
            return Path.GetFullPath(combinedPath);
        }
    }
}