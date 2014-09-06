using System;

namespace SurveyWeb.Models
{
    public static class DateTimeUtils
    {
        public static readonly string[] groupName = {"Older than month",
            "Last month", "4 weeks ago", "3 weeks ago", 
            "2 weeks ago", "Last week",
            "Monday", "Tuesday", "Wednesday", 
            "Thursday", "Friday", "Saturday", "Sunday",
            "Yesterday",
            "Today",
            "Tomorrow", 
            "Monday", "Tuesday", "Wednesday", 
            "Thursday", "Friday", "Saturday", "Sunday", 
            "Next week", "In 2 weeks", 
            "In 3 weeks", "In 4 weeks", "Next month", 
            "Newer than next month"};

        public static int ToGroupIndex(this DateTime sourceDateTime)
        {
            return GetDateTimeGroupIndex(DateTime.Now, sourceDateTime) + 14;
        }

        public static string GetGroupName(int groupIndex)
        {
            return groupName[groupIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtRef"></param>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        /// <remarks>http://www.codeproject.com/Articles/35440/How-to-mimic-Outlook-group-naming</remarks>
        internal static int GetDateTimeGroupIndex(DateTime dtRef, DateTime dtSource)
        {
            // Because week starts on Monday in France
            int dtWeekDay = (Convert.ToInt32(dtSource.DayOfWeek) + 6) % 7;
            int dtRefWeekDay = (Convert.ToInt32(dtRef.DayOfWeek) + 6) % 7;

            int ts = (dtRef.Date - dtSource.Date).Days;

            if (ts == 0)
                return 0;  // same day
            
            if (ts == 1)
                return -1; // day before

            if (ts == -1)
                return 1; // day after

            // same week 
            if ((ts > 1) && (ts <= dtRefWeekDay))
                return dtWeekDay - 8;

            if ((-ts > 1) && (-ts <= 6 - dtRefWeekDay))
                return dtWeekDay + 2;

            // previous / next weeks
            for (int nIdx = 0; nIdx <= 3; nIdx++)
            {
                if ((ts > dtRefWeekDay + nIdx * 7) && 
                    (ts <= dtRefWeekDay + (nIdx + 1) * 7))
                if (dtRef.Month == dtSource.Month)
                    return -(9 + nIdx);
                else
                    return -13;
                if ((-ts > 6 - dtRefWeekDay + nIdx * 7) && 
                    (-ts <= 6 - dtRefWeekDay + (nIdx + 1) * 7))
                if (dtRef.Month == dtSource.Month)
                    return (9 + nIdx);
                else
                    return 13;
            } 

            if (Math.Abs(dtSource.Month - dtRef.Month) == 1) 
                return Math.Sign(ts) * (-13);
            
            return Math.Sign(ts) * (-14);
        }

        public static DateTime ConvertFromUnixTimestamp(this double? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp.Value).ToLocalTime();
            return dtDateTime;
        }
    }
}