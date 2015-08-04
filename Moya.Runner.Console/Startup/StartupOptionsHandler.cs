namespace Moya.Runner.Console.Startup
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class StartupOptionsHandler
    {
        private const char FilenameSeparator = ';';
        private readonly List<string> files = new List<string>(); 

        public void HandleOptions(StartupOptionsContainer optionsContainer)
        {
            foreach (var optionKey in optionsContainer.Options.Keys)
            {
                switch (optionKey)
                {
                    case "--help":
                    case "-h":
                        PrintUsage();
                        break;
                    case "--files":
                    case"-f":
                        var filenamesWithSeparator = optionsContainer.Options["--files"];
                        foreach (var filename in filenamesWithSeparator.Split(FilenameSeparator))
                        {
                            files.Add(filename);
                        }
                        break;
                    default:
                        System.Console.WriteLine("Error: {0} is not a valid argument.".FormatWith(optionKey));
                        PrintUsage();
                        Environment.Exit(1);
                        break;
                }
            }
            InvokeMoyaRunners();
        }

        private void InvokeMoyaRunners()
        {
            foreach (var file in files)
            {
                ITestFileExecuter testFileExecuter = new TestFileExecuter(file);
                testFileExecuter.RunAllTests();
            }
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine("Usage: Moya.Runner.Console.exe (-f|--files)=<path-to-dll>[;<path-to-other-dll>...] ([options] <argument>)");
            System.Console.WriteLine("\nOptional options:");
            System.Console.WriteLine("\t-h\t--help\t\tPrints this message.");
            System.Console.WriteLine("\nRequired options:");
            System.Console.WriteLine("\t-f\t--files\t\tSpecifies which DLLs to run with Moya.");
        }
    }
}