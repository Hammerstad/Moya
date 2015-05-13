namespace Moya.Runner.Datamodels
{
    using System;

    public class TestResult
    {
        public TestOutcome TestOutcome { get; set; }
        public String ErrorMessage { get; set; }
    }
}