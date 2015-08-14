namespace Moya.Runners
{
    using System.Collections.Generic;
    using Models;

    public interface ILessThanTestRunner : IPostTestRunner
    {
        ICollection<ITestResult> previousTestResults { get; set; }

        int Seconds { get; }
    }
}