namespace Moya.Runner.Console
{
    public class Startup
    {
        private readonly StartupArgumentsResolver startupArgumentsResolver = new StartupArgumentsResolver();
        private readonly StartupArgumentsContainer startupArgumentsContainer = new StartupArgumentsContainer();

        public void Run(string[] args)
        {
            foreach (var arg in args)
            {
                ArgumentValuePair argumentValuePair = ArgumentValuePair.Create(arg);
                string longArgumentName = startupArgumentsResolver.GetLongArgumentName(argumentValuePair.Argument);
                startupArgumentsContainer.AddStartupArgument(longArgumentName, argumentValuePair.Value);
            }
            startupArgumentsContainer.Print();
        } 
    }
}