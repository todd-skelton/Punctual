using System;

namespace Punctual.Intervally
{
    public class IntervallyHostedServiceOptions<TScheduledAction> : IHostedServiceOptions, IEquatable<IntervallyHostedServiceOptions<TScheduledAction>>
    where TScheduledAction : IScheduledAction
    {
        public bool RunOnStart { get; set; }
        public Frequency Frequency { get; set; }
        public int Period { get; set; }

        public bool Equals(IntervallyHostedServiceOptions<TScheduledAction> other)
        {
            if (other == null || RunOnStart != other.RunOnStart || Frequency != other.Frequency || Period != other.Period)
                return false;
            return true;
        }
    }
}