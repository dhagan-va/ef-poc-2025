using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle.Common
{
    public static class Extensions
    {
        private const string DateFormat = "yyMMdd";
        private const string TimeFormat = "hhmm";
        
        public static string ToEdiDate(this DateTime timeStamp)
        {
            return timeStamp.Date.ToString(DateFormat);
        }

        public static string ToEdiTime(this DateTime timeStamp)
        {
            return timeStamp.TimeOfDay.ToString(TimeFormat);
        }
    }
}
