using System;

namespace Punctual.Weekly
{
    [Flags]
    public enum DaysToRun
    {
        Sunday = 1 << DayOfWeek.Sunday,
        Monday = 1 << DayOfWeek.Monday,
        Tuesday = 1 << DayOfWeek.Tuesday,
        Wednesday = 1 << DayOfWeek.Wednesday,
        Thursday = 1 << DayOfWeek.Thursday,
        Friday = 1 << DayOfWeek.Friday,
        Saturday = 1 << DayOfWeek.Saturday,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Weekend = Saturday | Sunday,
        All = Weekdays | Weekend
    }


}
