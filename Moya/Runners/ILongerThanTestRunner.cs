namespace Moya.Runners
{
    using System.Collections.Generic;
    using Models;

    internal interface ILongerThanTestRunner : IPostTestRunner
    {
        ICollection<ITestResult> previousTestResults { get; set; }

        int Seconds { get; }
    }
}