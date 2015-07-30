namespace Moya.Runner.Console
{
    using System;

    public class StartupOptionsHandler
    {
        private readonly StartupOptionsContainer optionsContainer;

        public StartupOptionsHandler(StartupOptionsContainer optionsContainer)
        {
            this.optionsContainer = optionsContainer;
        }

        public void HandleOptions()
        {
            foreach (var optionKey in optionsContainer.Options.Keys)
            {
                Console.WriteLine(optionKey);
            }
        }
    }
}