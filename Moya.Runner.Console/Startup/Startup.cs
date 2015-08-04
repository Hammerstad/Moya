namespace Moya.Runner.Console.Startup
{
    using System.Collections.Generic;
    using Extensions;
    using Moya.Extensions;

    public class Startup
    {
        private readonly StartupOptionsContainer startupOptionsContainer = new StartupOptionsContainer();

        public void Run(string[] options)
        {
            AddStartupOptionsToContainer(options);
            HandleOptions();
        }

        private void AddStartupOptionsToContainer(IEnumerable<string> options)
        {
            foreach (var option in options)
            {
                AddStartupOption(option);
            }
        }

        private void AddStartupOption(string option)
        {
            startupOptionsContainer.Options.AddRange(ParseStartupOption(option));
        }

        private void HandleOptions()
        {
            var startupOptionsHandler = new StartupOptionsHandler();
            startupOptionsHandler.HandleOptions(startupOptionsContainer);
        }

        private static IDictionary<OptionType, string> ParseStartupOption(string stringFromCommandLine)
        {
            string[] optionAndArgument = stringFromCommandLine.Split('=');

            return new Dictionary<OptionType, string>()
            {
                { optionAndArgument[0].ToOptionType(), optionAndArgument[1] ?? string.Empty}
            };
        }
    }
}