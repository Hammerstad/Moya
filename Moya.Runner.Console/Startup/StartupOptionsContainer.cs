namespace Moya.Runner.Console.Startup
{
    using System.Collections.Generic;

    public class StartupOptionsContainer
    {
        private readonly IDictionary<OptionType, string> options = new Dictionary<OptionType, string>();

        public IDictionary<OptionType, string> Options
        {
            get { return options; }
        }
    }
}