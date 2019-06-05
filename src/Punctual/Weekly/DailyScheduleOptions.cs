using System;
using System.Collections.Generic;
using System.Linq;

namespace Punctual.Weekly
{
    public class DailyScheduleOptions : IEquatable<DailyScheduleOptions>
    {
        public DailyScheduleOptions()
        {
        }

        public DailyScheduleOptions(DaysToRun days, List<TimeSpan> times)
        {
            Days = days;
            Times = times;
        }

        public DaysToRun Days { get; set; }
        public List<TimeSpan> Times { get; set; }

        public bool Equals(DailyScheduleOptions other)
        {
            if (other == null || Days != other.Days || !Times.SequenceEqual(other.Times))
                return false;
            return true;
        }
    }
}
