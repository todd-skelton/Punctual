using System;
using System.Collections.Generic;
using System.Linq;

namespace Punctual.Weekly
{
    public class WeeklyHostedServiceOptions<TScheduledAction> : IHostedServiceOptions, IEquatable<WeeklyHostedServiceOptions<TScheduledAction>>
    where TScheduledAction : IScheduledAction
    {
        public WeeklyHostedServiceOptions()
        {
            Schedule = new List<DailyScheduleOptions>();
        }

        public List<DailyScheduleOptions> Schedule { get; set; }

        public bool Equals(WeeklyHostedServiceOptions<TScheduledAction> other)
        {
            if (other == null || !Schedule.SequenceEqual(other.Schedule))
                return false;
            return true;
        }
    }
}
