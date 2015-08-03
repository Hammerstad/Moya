namespace Moya.Runner.Console
{
    using System;
    using Extensions;

    public class StartupOptionsHandler
    {
        public void HandleOptions(StartupOptionsContainer optionsContainer)
        {
            foreach (var optionKey in optionsContainer.Options.Keys)
            {
                switch (optionKey)
                {
                    case "--help":
                        PrintUsage();
                        break;
                    case "--files":
                        Console.WriteLine(optionsContainer.Options["--files"]);
                        break;
                    default:
                        Console.WriteLine("Error: {0} is not a valid argument.".FormatWith(optionKey));
                        PrintUsage();
                        Environment.Exit(1);
                        break;
                }
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: Moya.Runner.Console.exe (-f|--files)=<path-to-dll>[;<path-to-other-dll>...] ([options] <argument>)");
            Console.WriteLine("\nOptional options:");
            Console.WriteLine("\t-h\t--help\t\tPrints this message.");
            Console.WriteLine("\nRequired options:");
            Console.WriteLine("\t-f\t--files\t\tSpecifies which DLLs to run tests on.");
        }
    }
}