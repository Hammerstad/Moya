namespace Moya.Runner.Statistics
{
    using System;
    using System.Collections.Concurrent;

    public class DurationManager : IDurationManager
    {
        private static readonly Lazy<IDurationManager> defaultInstance;

        private ConcurrentDictionary<int, long> methodDurationMapping;

        public ConcurrentDictionary<int, long> MethodDurationMapping
        {
            get { return methodDurationMapping; }
            private set { methodDurationMapping = value; }
        }

        static DurationManager() {
            defaultInstance = new Lazy<IDurationManager>(() => new DurationManager());
		}

        private DurationManager()
        {
            MethodDurationMapping = new ConcurrentDictionary<int, long>();
		}

        public static IDurationManager DefaultInstance { get { return defaultInstance.Value; } }

        public long GetDurationFromHashcode(int hashcode)
        {
            long returnvalue;
            methodDurationMapping.TryGetValue(hashcode, out returnvalue);

            return returnvalue;
        }

        public void AddOrUpdateDuration(int hashcode, long duration)
        {
            methodDurationMapping.AddOrUpdate(hashcode, duration, (k, v) => v);
        }
    }
}