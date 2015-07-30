namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using Extensions;


    public class StartupArgumentsResolver
    {
        private readonly IDictionary<string, string> argumentToArgumentMapping = new Dictionary<string, string>
        {
            {"-f","--files"},
            {"--files","-f"},
            {"-h","--help"},
            {"--help","-h"},
        };

        public string GetLongArgumentName(string argument)
        {
            if (!ArgumentIsValid(argument))
            {
                throw new ArgumentException("{0} is not a valid argument. ".FormatWith(argument));
            }

            return argument.Length > 1 ? argument : argumentToArgumentMapping[argument];
        }

        private bool ArgumentIsValid(string argument)
        {
            return argumentToArgumentMapping.ContainsKey(argument);
        }
    }
}