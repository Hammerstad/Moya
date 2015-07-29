namespace Moya.Models
{
    public class TestResult
    {
        public TestOutcome TestOutcome { get; set; }

        public string ErrorMessage { get; set; }

        public int Duration { get; set; }
    }
}