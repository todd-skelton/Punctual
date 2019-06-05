using System;
using System.Collections.Generic;

namespace Punctual.Weekly
{
    public static class WeeklyHostedServiceOptionsExtensions
    {
        public static IEnumerable<WeeklySchedulePoint> GetSchedule<TScheduledAction>(this WeeklyHostedServiceOptions<TScheduledAction> options)
            where TScheduledAction : IScheduledAction
        {
            foreach (var schedule in options.Schedule)
            {
                foreach (DaysToRun days in schedule.Days.GetFlags(true))
                {
                    //bitshift to convert DaysOfWeek flag to DayOfWeek
                    var day = (DayOfWeek)(Math.Log((int)days, 2));

                    foreach (var time in schedule.Times)
                    {
                        yield return new WeeklySchedulePoint(day, time);
                    }
                }
            }
        }
    }


}
