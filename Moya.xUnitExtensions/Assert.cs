using System;
using System.Diagnostics;
using Moya.Statistics;
using Moya.xUnitExtensions.Exceptions;

namespace Moya.xUnitExtensions
{
    public class Assert : Xunit.Assert
    {
        public static void LastMethodUsedLessTimeThan(int milliseconds)
        {
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var executionTimeOfLastMethod = DurationManager.DefaultInstance.GetDurationFromHashcode(method.GetHashCode());

            if (executionTimeOfLastMethod >= milliseconds)
            {
                var exceptionMessage = String.Format(
                    "Method used too long time to execute. Expected less than {0}ms, used {1}ms.",
                    milliseconds,
                    executionTimeOfLastMethod);
                throw new TimingException(exceptionMessage);
            }
        }
    }
}