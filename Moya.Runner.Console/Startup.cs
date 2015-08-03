namespace Moya.Runner.Console
{
    using System.Collections.Generic;

    public class Startup
    {
        private readonly StartupOptionsResolver startupOptionsResolver = new StartupOptionsResolver();
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
            OptionArgumentPair optionArgumentPair = OptionArgumentPair.Create(option);
            string longOptionName = startupOptionsResolver.GetLongOptionName(optionArgumentPair.Option);
            startupOptionsContainer.Options[longOptionName] = optionArgumentPair.Argument;
        }

        private void HandleOptions()
        {
            var startupOptionsHandler = new StartupOptionsHandler();
            startupOptionsHandler.HandleOptions(startupOptionsContainer);
        }
    }
}