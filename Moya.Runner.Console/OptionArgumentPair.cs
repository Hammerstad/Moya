namespace Moya.Runner.Console
{
    public struct OptionArgumentPair
    {
        public string Option { get; set; }

        public string Argument { get; set; }

        public static OptionArgumentPair Create(string stringFromCommandLine)
        {
            string[] optionAndArgument = stringFromCommandLine.Split('=');
            return new OptionArgumentPair
            {
                Option = optionAndArgument[0],
                Argument = optionAndArgument[1]
            };
        }
    }
}