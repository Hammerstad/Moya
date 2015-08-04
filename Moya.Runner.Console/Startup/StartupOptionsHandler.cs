namespace Moya.Runner.Console.Startup
{
    using System;
    using System.Collections.Generic;
    using Moya.Extensions;

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
                    case OptionType.Help:
                        PrintUsage();
                        break;
                    case OptionType.Files:
                        var filenamesWithSeparator = optionsContainer.Options[OptionType.Files];
                        foreach (var filename in filenamesWithSeparator.Split(FilenameSeparator))
                        {
                            files.Add(filename);
                        }
                        break;
                    default:
                        Console.WriteLine("Error: {0} is not a valid argument.".FormatWith(optionsContainer.Options[optionKey]));
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