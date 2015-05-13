namespace Moya.Runner.VisualStudio
{
    using System;

    public class Constants
    {
        public const string ExecutorUriString = "executor://MoyaTestExecutor";

        public static readonly Uri ExecutorUri = new Uri(ExecutorUriString);
    }
}