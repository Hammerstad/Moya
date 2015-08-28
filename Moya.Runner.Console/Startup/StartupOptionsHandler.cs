namespace Moya.Runner.Console.Startup
{
    using System;
    using System.Collections.Generic;

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
                        Console.WriteLine("Error: {0} is not a valid argument.", optionsContainer.Options[optionKey]);
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
            Console.WriteLine("Usage: Moya.Runner.Console.exe (-f|--files)=<path-to-dll>[;<path-to-other-dll>...] ([options] <argument>)");
            Console.WriteLine("\nOptional options:");
            Console.WriteLine("\t-h\t--help\t\tPrints this message.");
            Console.WriteLine("\nRequired options:");
            Console.WriteLine("\t-f\t--files\t\tSpecifies which DLLs to run with Moya.");
        }
    }
}