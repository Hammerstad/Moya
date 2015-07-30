namespace Moya.Runner.Console
{
    using System.Collections.Generic;

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
                return option;
            }

            return option.Length > 1 ? option : optionToOptionMapping[option];
        }

        private bool OptionIsValid(string option)
        {
            return optionToOptionMapping.ContainsKey(option);
        }
    }
}