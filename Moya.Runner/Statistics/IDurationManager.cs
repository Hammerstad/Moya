namespace Moya.Runner.Statistics
{
    using System.Collections.Concurrent;

    public interface IDurationManager
    {
        long GetDurationFromHashcode(int hashcode);
        void AddOrUpdateDuration(int hashcode, long duration);
        ConcurrentDictionary<int, long> MethodDurationMapping { get; }
    }
}