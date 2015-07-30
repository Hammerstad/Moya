namespace Moya.Runner.Console
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