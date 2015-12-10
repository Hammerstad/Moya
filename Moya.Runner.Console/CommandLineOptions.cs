namespace Moya.Runner.Console
{
    using System.Collections.Generic;

    internal class CommandLineOptions
    {
        internal bool Verbose { get; set; }

        internal bool Help { get; set; }

        internal List<string> AssemblyFiles { get; } = new List<string>();
    }
}