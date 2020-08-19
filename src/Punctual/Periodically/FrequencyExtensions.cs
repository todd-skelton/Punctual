using System;

namespace Punctual.Periodically
{
    public static class FrequencyExtensions
    {
        public static TimeSpan GetPeriodically(this Frequency frequency, int period)
        {
            switch (frequency)
            {
                case Frequency.Milliseconds:
                    return TimeSpan.FromMilliseconds(period);
                case Frequency.Seconds:
                    return TimeSpan.FromSeconds(period);
                case Frequency.Minutes:
                    return TimeSpan.FromMinutes(period);
                case Frequency.Hours:
                    return TimeSpan.FromHours(period);
                case Frequency.Days:
                    return TimeSpan.FromDays(period);
                case Frequency.Weeks:
                    return TimeSpan.FromDays(period * 7);
                default:
                    throw new InvalidOperationException("Invalid frequency");

            }
        }
    }
}