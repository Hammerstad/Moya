namespace Moya.Runner.Console
{
    using System;
    using Extensions;

    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                new Startup.Startup().Run(args);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Error: {0}".FormatWith(e.Message));
                Environment.Exit(1);
            }
        }
    }
}
