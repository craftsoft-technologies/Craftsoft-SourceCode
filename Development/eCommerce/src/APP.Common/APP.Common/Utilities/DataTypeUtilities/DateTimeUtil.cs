using System;

namespace APP.Common.Utilities.DataTypeUtilities
{
    public static class DateTimeUtil
    {
        public static DateTime FromMilliSeconds(double microSec)
        {
            DateTime startTime = new DateTime(1970, 1, 1);

            TimeSpan time = TimeSpan.FromMilliseconds(microSec);
            return startTime.Add(time);
        }
    }
}
