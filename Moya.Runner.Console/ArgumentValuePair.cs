namespace Moya.Runner.Console
{
    public struct ArgumentValuePair
    {
        public string Argument { get; set; }

        public string Value { get; set; }

        public static ArgumentValuePair Create(string commandlineArgument)
        {
            string[] splitArgument = commandlineArgument.Split('=');
            return new ArgumentValuePair
            {
                Argument = splitArgument[0],
                Value = splitArgument[1]
            };
        }
    }
}