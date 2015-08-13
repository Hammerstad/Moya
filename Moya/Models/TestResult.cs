namespace Moya.Models
{
    public class TestResult : ITestResult
    {
        public TestType TestType { get; set; }

        public TestOutcome TestOutcome { get; set; }

        public string ErrorMessage { get; set; }

        public long Duration { get; set; }
    }
}