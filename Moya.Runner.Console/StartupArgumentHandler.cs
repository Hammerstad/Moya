namespace Moya.Runner.Console
{
    using System;
    public class StartupArgumentHandler
    {
        private readonly StartupArgumentsContainer argumentsContainer;

        public StartupArgumentHandler(StartupArgumentsContainer argumentsContainer)
        {
            this.argumentsContainer = argumentsContainer;
        }

        public void HandleArguments()
        {
            foreach (var argumentKey in argumentsContainer.Arguments.Keys)
            {
                Console.WriteLine(argumentKey);
            }
        }
    }
}