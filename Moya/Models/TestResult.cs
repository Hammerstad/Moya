namespace Moya.Models
{
    public class TestResult : ITestResult
    {
        public TestOutcome TestOutcome { get; set; }

        public string ErrorMessage { get; set; }

        public long Duration { get; set; }
    }
}