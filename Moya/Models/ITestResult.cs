namespace Moya.Models
{
    public interface ITestResult
    {
        TestType TestType { get; set; }

        TestOutcome TestOutcome { get; set; }

        string ErrorMessage { get; set; }

        long Duration { get; set; } 
    }
}