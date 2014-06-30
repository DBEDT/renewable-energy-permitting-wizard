using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HawaiiDBEDT.Web.utils
{
    public static class DateTimeUtils
    {
        public static DateTime ToHawaiiTime(DateTime startDate)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(startDate, "Hawaiian Standard Time");
        }
    }
}