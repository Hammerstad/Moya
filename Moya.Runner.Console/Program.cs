namespace Moya.Runner.Console
{
    using System;

    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Startup startup = new Startup(args);
                startup.Run();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: {0}", e.Message);
                Environment.Exit(1);
            }
        }
    }
}
