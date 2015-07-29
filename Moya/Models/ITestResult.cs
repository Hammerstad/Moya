namespace Moya.Models
{
    public interface ITestResult
    {
        TestOutcome TestOutcome { get; set; }

        string ErrorMessage { get; set; }

        long Duration { get; set; } 
    }
}