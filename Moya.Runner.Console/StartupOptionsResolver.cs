namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class StartupOptionsResolver
    {
        private readonly IDictionary<string, string> optionToOptionMapping = new Dictionary<string, string>
        {
            {"-f","--files"},
            {"--files","-f"},
            {"-h","--help"},
            {"--help","-h"},
        };

        public string GetLongOptionName(string option)
        {
            if (!OptionIsValid(option))
            {
                throw new ArgumentException("{0} is not a valid option. ".FormatWith(option));
            }

            return option.Length > 1 ? option : optionToOptionMapping[option];
        }

        private bool OptionIsValid(string option)
        {
            return optionToOptionMapping.ContainsKey(option);
        }
    }
}