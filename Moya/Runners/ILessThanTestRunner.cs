namespace Moya.Runners
{
    using System.Collections.Generic;
    using Models;

    internal interface ILessThanTestRunner : IPostTestRunner
    {
        ICollection<ITestResult> previousTestResults { get; set; }

        int Seconds { get; }
    }
}