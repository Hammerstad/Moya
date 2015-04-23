using System;
using System.Collections.Concurrent;

namespace Moya.Statistics
{
    public class DurationManager : IDurationManager
    {
        private static readonly Lazy<IDurationManager> defaultInstance;

        private readonly ConcurrentDictionary<int, long> methodDurationMapping; 

		static DurationManager() {
            defaultInstance = new Lazy<IDurationManager>(() => new DurationManager());
		}

        private DurationManager()
        {
            methodDurationMapping = new ConcurrentDictionary<int, long>();
		}

        public static IDurationManager DefaultInstance { get { return defaultInstance.Value; } }

        public long GetDurationFromHashcode(int hashcode)
        {
            long returnvalue;
            methodDurationMapping.TryGetValue(hashcode, out returnvalue);

            return returnvalue;
        }

        public void RegisterDuration(int hashcode, long duration)
        {
            Console.WriteLine("{0} {1}",hashcode,duration);
            methodDurationMapping.AddOrUpdate(hashcode, duration, (k, v) => v);
        }
    }
}