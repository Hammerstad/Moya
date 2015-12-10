namespace Moya.Runner.Console.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ArgumentParser
    {
        private readonly Stack<string> _arguments = new Stack<string>();

        internal CommandLineOptions CommandLineOptions { get; } = new CommandLineOptions();
        
        public ArgumentParser(string[] args)
        {
            for (int i = args.Length - 1; i >= 0; i--)
            {
                _arguments.Push(args[i]);
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
            foreach (var assemblyFile in CommandLineOptions.AssemblyFiles)
            {
                Console.WriteLine("\t"+assemblyFile);
            }
            Console.WriteLine("Help   : " + (CommandLineOptions.Help ? "true" : "false"));
            Console.WriteLine("Verbose: " + (CommandLineOptions.Verbose ? "true" : "false"));
        }

        private void ParseAssemblyFilesFromArguments()
        {
            while (_arguments.Count > 0)
            {
                if (_arguments.Peek().StartsWith("-"))
                {
                    break;
                }

                var assemblyFile = _arguments.Pop();

                if (!File.Exists(assemblyFile))
                {
                    throw new ArgumentException($"File not found: {assemblyFile}");
                }
                CommandLineOptions.AssemblyFiles.Add(assemblyFile);
            }
        }

        private void EnsureThereIsAtLeastOneAssembly()
        {
            if (CommandLineOptions.AssemblyFiles.Count == 0)
            {
                throw new ArgumentException("You must specify at least one assembly");
            }
        }

        private void ParseCommandLineOptions()
        {
            while (_arguments.Count > 0)
            {
                var option = PopOption();
                var optionName = GetOptionName(option);

                switch (optionName)
                {
                    case "help":
                        EnsureNoOptionValue(option);
                        CommandLineOptions.Help = true;
                        break;
                    case "verbose":
                        EnsureNoOptionValue(option);
                        CommandLineOptions.Verbose = true;
                        break;
                }
            }
        }

        private static void EnsureNoOptionValue(KeyValuePair<string, string> option)
        {
            if (option.Value != null)
            {
                throw new ArgumentException($"Unknown command line option: {option.Value}");
            }
        }

        private KeyValuePair<string, string> PopOption()
        {
            string option = _arguments.Pop();
            string value = null;

            if(_arguments.Count > 0 && !_arguments.Peek().StartsWith("-"))
            {
                value = _arguments.Pop();
            }

            return new KeyValuePair<string, string>(option, value);
        }

        private static string GetOptionName(KeyValuePair<string, string> option)
        {
            var optionName = option.Key.ToLower();

            if (!optionName.StartsWith("-"))
            {
                throw new ArgumentException($"Unknown command line option: {option.Key}");
            }

            return optionName.Substring(2);
        }
    }
}