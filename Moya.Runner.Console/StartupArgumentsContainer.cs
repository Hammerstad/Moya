namespace Moya.Runner.Console
{
    using System.Collections.Generic;

    public class StartupArgumentsContainer
    {
        private readonly IDictionary<string, string> arguments = new Dictionary<string, string>();

        public IDictionary<string, string> Arguments
        {
            get { return arguments; }
        }
    }
}