namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class StartupArgumentsContainer
    {
        private readonly IDictionary<string, string> arguments = new Dictionary<string, string>();

        public void AddStartupArgument(string argument, string value)
        {
            arguments[argument] = value;
        }

        public void Print()
        {
            foreach (var argument in arguments)
            {
                Console.WriteLine("{0} :: {1}".FormatWith(argument.Key, argument.Value));
            }
        }
    }
}