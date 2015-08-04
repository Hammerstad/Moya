﻿namespace Moya.Runner.Console.Extensions
{
    using Startup;

    public static class StringExtensions
    {
        public static OptionType ToOptionType(this string optionTypeName)
        {
            switch (optionTypeName)
            {
                case "-f":
                case "--files":
                    return OptionType.Files;
                case "-h":
                case "--help":
                    return OptionType.Help;
                default:
                    return OptionType.InvalidArgument;
            }
        }
    }
}