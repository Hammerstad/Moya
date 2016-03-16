namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ArgumentParser
    {
        private readonly Stack<string> _arguments = new Stack<string>();

        internal CommandLineOptions CommandLineOptions { get; } = new CommandLineOptions();
        
        public ArgumentParser(params string[] args)
        {
            for (int i = args.Length - 1; i >= 0; i--)
            {
                _arguments.Push(args[i]);
            }
        }

        public void ParseArguments()
        {
            ParseAssemblyFilesFromArguments();
            EnsureThereIsAtLeastOneAssemblyOrHelpSpecified();
            ParseCommandLineOptions();
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

        private void EnsureThereIsAtLeastOneAssemblyOrHelpSpecified()
        {
            if (HelpSpecified())
            {
                return;
            }

            if (CommandLineOptions.AssemblyFiles.Count == 0)
            {
                throw new ArgumentException("You must specify at least one assembly.\nType --help for more information.");
            }
        }

        private bool HelpSpecified()
        {
            return _arguments.Contains("--help") || _arguments.Contains("-h");
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