namespace Moya.Statistics
{
    public interface IDurationManager
    {
        long GetDurationFromHashcode(int hashcode);
        void RegisterDuration(int hashcode, long duration);
    }
}