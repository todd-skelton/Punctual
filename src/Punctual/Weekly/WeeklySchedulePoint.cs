using System;

namespace Punctual.Weekly
{
    public class WeeklySchedulePoint
    {
        private DateTimeOffset _cachedNextDateTimeOffset = DateTimeOffset.MinValue;

        public WeeklySchedulePoint(DayOfWeek dayOfWeek, TimeSpan time)
        {
            DayOfWeek = dayOfWeek;
            Time = time;
        }

        public DayOfWeek DayOfWeek { get; }
        public TimeSpan Time { get; }

        public DateTimeOffset NextDateTimeOffset
        {
            get
            {
                var now = DateTimeOffset.Now;

                if (_cachedNextDateTimeOffset == DateTimeOffset.MinValue || _cachedNextDateTimeOffset < now)
                {
                    _cachedNextDateTimeOffset = CalculateNextDateTimeOffset(now);
                }
                return _cachedNextDateTimeOffset;
            }
        }

        private DateTimeOffset CalculateNextDateTimeOffset(DateTimeOffset now)
        {
            var dayOfWeek = now.DayOfWeek;
            var timeOfDay = now.TimeOfDay;
            var daysUntilNext = ((int)DayOfWeek - (int)dayOfWeek + 7) % 7;

            if (daysUntilNext == 0 && timeOfDay > Time)
            {
                daysUntilNext += 7;
            }

            return now.Subtract(now.TimeOfDay).AddDays(daysUntilNext).Add(Time);
        }
    }


}
