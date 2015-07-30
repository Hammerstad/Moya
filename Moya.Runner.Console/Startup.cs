namespace Moya.Runner.Console
{
    public class Startup
    {
        private readonly StartupArgumentsResolver startupArgumentsResolver = new StartupArgumentsResolver();
        private readonly StartupArgumentsContainer startupArgumentsContainer = new StartupArgumentsContainer();

        public void Run(string[] args)
        {
            AddStartupArguments(args);
            HandleArguments();
        }

        private void AddStartupArguments(string[] args)
        {
            foreach (var arg in args)
            {
                AddStartupArgument(arg);
            }
        }

        private void AddStartupArgument(string arg)
        {
            ArgumentValuePair argumentValuePair = ArgumentValuePair.Create(arg);
            string longArgumentName = startupArgumentsResolver.GetLongArgumentName(argumentValuePair.Argument);
            startupArgumentsContainer.Arguments[longArgumentName] = argumentValuePair.Value;
        }

        private void HandleArguments()
        {
            var startupArgumentHandler = new StartupArgumentHandler(startupArgumentsContainer);
            startupArgumentHandler.HandleArguments();
        }
    }
}