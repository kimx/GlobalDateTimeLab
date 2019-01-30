using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalDateTimeLab.Console.Lib
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTimeBegin(this DateTime value)
        {
            DateTime r = value;
            return new DateTime(r.Year, r.Month, r.Day, 00, 00, 00, r.Kind);
        }

        public static DateTime GetCurrentTaiwanDateTime()
        {
            return DateTime.UtcNow.AddHours(8);
        }

        public static DateTime GetCurrentTaiwanDate()
        {
            return DateTime.UtcNow.AddHours(8).ToDateTimeBegin();
        }

        public static DateTime GetCurrentSouthArfricaDateTime()
        {
            return DateTime.UtcNow.AddHours(2).ToDateTimeBegin();
        }

        //Test Date
        private static int CurrentTimeZoneHour = 0;

        public static DateTime GetTestTaiwanDateTime()
        {
            return new DateTime(2019, 1, 18, CurrentTimeZoneHour + 8, 0, 0, DateTimeKind.Local);
        }
        public static DateTime GetTestUtcDateTime()
        {
            return new DateTime(2019, 1, 18, CurrentTimeZoneHour, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime GetTestSouthAfricaDateTime()
        {
            return new DateTime(2019, 1, 18, CurrentTimeZoneHour + 2, 0, 0, DateTimeKind.Local);
        }

        public static DateTime GetCurrentCultureDateTime()
        {
            var customCulture = Thread.CurrentThread.CurrentCulture as CustomCultureInfo;
            var timeZoneHour = CurrentTimeZoneHour;
            if (customCulture != null)
                timeZoneHour = customCulture.UtcHours;
            return DateTime.UtcNow.AddHours(timeZoneHour);
        }
    }
}
