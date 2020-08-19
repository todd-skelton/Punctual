using System;

namespace Punctual.Periodically
{
    public class PeriodicallyHostedServiceOptions<TScheduledAction> : IHostedServiceOptions, IEquatable<PeriodicallyHostedServiceOptions<TScheduledAction>>
    where TScheduledAction : IScheduledAction
    {
        public bool RunOnStart { get; set; }
        public Frequency Frequency { get; set; }
        public int Period { get; set; }

        public bool Equals(PeriodicallyHostedServiceOptions<TScheduledAction> other)
        {
            if (other == null || RunOnStart != other.RunOnStart || Frequency != other.Frequency || Period != other.Period)
                return false;
            return true;
        }
    }
}