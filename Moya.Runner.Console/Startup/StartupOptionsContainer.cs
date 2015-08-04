namespace Moya.Runner.Console.Startup
{
    using System.Collections.Generic;

    public class StartupOptionsContainer
    {
        private readonly IDictionary<string, string> options = new Dictionary<string, string>();

        public IDictionary<string, string> Options
        {
            get { return options; }
        }
    }
}