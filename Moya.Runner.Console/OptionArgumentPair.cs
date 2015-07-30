namespace Moya.Runner.Console
{
    using System.Linq;

    public struct OptionArgumentPair
    {
        public string Option { get; set; }

        public string Argument { get; set; }

        public static OptionArgumentPair Create(string stringFromCommandLine)
        {
            string[] optionAndArgument = stringFromCommandLine.Split('=');

            return new OptionArgumentPair
            {
                Option = optionAndArgument.Any() ? optionAndArgument[0] : string.Empty,
                Argument = optionAndArgument.Count() > 1 ? optionAndArgument[1] : string.Empty
            };
        }
    }
}