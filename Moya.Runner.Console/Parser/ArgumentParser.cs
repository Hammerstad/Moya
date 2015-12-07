namespace Moya.Runner.Console.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ArgumentParser
    {
        private readonly Stack<string> arguments = new Stack<string>();
        private readonly List<string> assemblyFiles = new List<string>();
        private readonly CommandLineOptions commandLineOptions = new CommandLineOptions();

        internal CommandLineOptions CommandLineOptions { get { return commandLineOptions; } }
        internal List<string> AssemblyFiles { get { return assemblyFiles; } } 

        public ArgumentParser(string[] args)
        {
            for (int i = args.Length - 1; i >= 0; i--)
            {
                arguments.Push(args[i]);
            }
        }

        public void ParseArguments()
        {
            ParseAssemblyFilesFromArguments();
            EnsureThereIsAtLeastOneAssembly();
            ParseCommandLineOptions();
            PrintDebug();
        }

        private void PrintDebug()
        {
            Console.WriteLine("Assembly files:");
            foreach (var assemblyFile in assemblyFiles)
            {
                Console.WriteLine("\t"+assemblyFile);
            }
            Console.WriteLine("Help   : " + (commandLineOptions.Help ? "true" : "false"));
            Console.WriteLine("Verbose: " + (commandLineOptions.Verbose ? "true" : "false"));
        }

        private void ParseAssemblyFilesFromArguments()
        {
            while (arguments.Count > 0)
            {
                if (arguments.Peek().StartsWith("-"))
                {
                    break;
                }

                var assemblyFile = arguments.Pop();

                if (!File.Exists(assemblyFile))
                {
                    throw new ArgumentException(String.Format("File not found: {0}", assemblyFile));
                }
                assemblyFiles.Add(assemblyFile);
            }
        }

        private void EnsureThereIsAtLeastOneAssembly()
        {
            if (assemblyFiles.Count == 0)
            {
                throw new ArgumentException("You must specify at least one assembly");
            }
        }

        private void ParseCommandLineOptions()
        {
            while (arguments.Count > 0)
            {
                var option = PopOption();
                var optionName = GetOptionName(option);

                switch (optionName)
                {
                    case "help":
                        EnsureNoOptionValue(option);
                        commandLineOptions.Help = true;
                        break;
                    case "verbose":
                        EnsureNoOptionValue(option);
                        commandLineOptions.Verbose = true;
                        break;
                }
            }
        }

        private static void EnsureNoOptionValue(KeyValuePair<string, string> option)
        {
            if (option.Value != null)
            {
                throw new ArgumentException(String.Format("Unknown command line option: {0}", option.Value));
            }
        }

        private KeyValuePair<string, string> PopOption()
        {
            string option = arguments.Pop();
            string value = null;

            if(arguments.Count > 0 && !arguments.Peek().StartsWith("-"))
            {
                value = arguments.Pop();
            }

            return new KeyValuePair<string, string>(option, value);
        }

        private static string GetOptionName(KeyValuePair<string, string> option)
        {
            var optionName = option.Key.ToLower();

            if (!optionName.StartsWith("-"))
            {
                throw new ArgumentException(String.Format("Unknown command line option: {0}", option.Key));
            }

            return optionName.Substring(2);
        }
    }
}