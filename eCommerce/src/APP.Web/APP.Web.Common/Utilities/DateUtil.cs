using System;
using System.Threading;

namespace APP.Web.Common.Utilities
{
    public static class DateUtil
    {
        public const double MilisecondsPerday = 24 * 60 * 60 * 1000;

        public static readonly string DefaultTimeFormat = "HH:mm";
        public static readonly string DefaultDateFormat = "yyyy/MM/dd";
        public static readonly string DefaultDateTimeFormat = DefaultDateFormat + " HH:mm:ss";
        public static readonly string DefaultDateTimeFormatWithoutSeconds = DefaultDateFormat + " HH:mm";

        public static string FormatTime(DateTime dt)
        {
            return dt.ToString(DefaultTimeFormat);
        }

        public static string FormatDate(DateTime date, string format = null)
        {
            if (format == null)
            {
                return date.ToString(DefaultDateFormat);
            }
            else
            {
                return date.ToString(format);
            }
        }

        public static string FormatDateTime(DateTime date, string format = null)
        {
            if (format == null)
            {
                return date.ToString(DefaultDateTimeFormat);
            }
            else
            {
                return date.ToString(format);
            }
        }

        public static DateTime ParseDate(string dateString, string format = null)
        {
            if (format == null)
            {
                return DateTime.ParseExact(dateString, DefaultDateFormat, null);
            }
            else
            {
                return DateTime.ParseExact(dateString, format, null);
            }
        }

        public static DateTime ParseDateTime(string dateString, string format = null)
        {
            if (format == null)
            {
                return DateTime.ParseExact(dateString, DefaultDateTimeFormat, null);
            }
            else
            {
                return DateTime.ParseExact(dateString, format, null);
            }
        }

        public static DateRange Today()
        {
            return Today(DateTime.Now);
        }

        public static DateRange Today(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, date.Month, date.Day);
            range.End = range.Start.AddDays(1).AddMilliseconds(-1);

            return range;
        }

        public static DateRange Yesterday()
        {
            return Yesterday(DateTime.Now);
        }

        public static DateRange Yesterday(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = date.Date.AddDays(-1); // new DateTime(date.Year, date.Month, date.Day - 1);
            range.End = range.Start.AddDays(1).AddMilliseconds(-1);

            return range;
        }

        public static DateRange ThisYear(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, 1, 1);
            range.End = range.Start.AddYears(1).AddMilliseconds(-1);

            return range;
        }

        public static DateRange LastYear(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year - 1, 1, 1);
            range.End = range.Start.AddYears(1).AddMilliseconds(-1);

            return range;
        }

        public static DateRange ThisMonth(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, date.Month, 1);
            range.End = range.Start.AddMonths(1).AddMilliseconds(-3);

            return range;
        }

        public static DateRange LastMonth(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = (new DateTime(date.Year, date.Month, 1)).AddMonths(-1);
            range.End = range.Start.AddMonths(1).AddMilliseconds(-1);

            return range;
        }

        public static DateRange LastTwoMonth(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = (new DateTime(date.Year, date.Month, 1)).AddMonths(-2);
            range.End = range.Start.AddMonths(1).AddMilliseconds(-1);

            return range;
        }

        public static DateRange RecentTwoMonth(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = (new DateTime(date.Year, date.Month, 1)).AddMonths(-1);
            range.End = range.Start.AddMonths(2).AddMilliseconds(-1);

            return range;
        }

        public static DateRange ThisWeek(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = date.Date.AddDays(-(int)date.DayOfWeek);
            range.End = range.Start.AddDays(7).AddMilliseconds(-1);

            return range;
        }

        public static DateRange ThisWeek(DateTime date, DayOfWeek startDay)
        {
            DateRange range = new DateRange();

            int strDay = (int)startDay;
            int today = (int)date.DayOfWeek;
            int firstDayOfWeek = today >= strDay ? today - strDay : (today + 7 - strDay);

            range.Start = date.Date.AddDays(-firstDayOfWeek);
            range.End = range.Start.AddDays(7).AddMilliseconds(-1);

            return range;
        }

        public static DateRange LastWeek(DateTime date)
        {
            DateRange range = ThisWeek(date);

            range.Start = range.Start.AddDays(-7);
            range.End = range.End.AddDays(-7);

            return range;
        }

        public static DateTime ToServerDateTime(this DateTime userClientDateTime, double userTimeZoneOffset)
        {
            DateTime date = DateTime.SpecifyKind(userClientDateTime, DateTimeKind.Utc);
            return DateTime.SpecifyKind(TimeZone.CurrentTimeZone.ToLocalTime(date.AddMinutes(-userTimeZoneOffset)), DateTimeKind.Local);
        }

        public static DateTime ToClientDateTime(this DateTime serverDateTime, double userTimeZoneOffset)
        {
            DateTime utcDateTime = serverDateTime.ToUniversalTime();
            return DateTime.SpecifyKind(utcDateTime.AddMinutes(userTimeZoneOffset), DateTimeKind.Unspecified);
        }

        public static int GetUtcOffset(string timeZone)
        {
            string strTimeOffset = timeZone.Replace("GMT", string.Empty).Trim().Replace("+", string.Empty);
            TimeSpan timeOffSet = TimeSpan.Parse(strTimeOffset);
            return (int)timeOffSet.TotalMinutes;
        }

        public static int ServerTimeZoneUtcOffset { get; set; }
        public static string ServerTimeZoneDisplayName { get; set; }

        static DateUtil()
        {
            var now = DateTime.Now;
            ServerTimeZoneUtcOffset = (int)(now - now.ToUniversalTime()).TotalMinutes;
            ServerTimeZoneDisplayName = GetTimeZoneDisplayName(ServerTimeZoneUtcOffset);
        }

        public static string GetTimeZoneDisplayName(string timeZoneValue)
        {
            return DropdownListUtil.GetText(Thread.CurrentThread.CurrentCulture.Name, Models.DropdownListType.TimeZone, timeZoneValue);
        }

        public static string GetTimeZoneDisplayName(int timeZoneOffset)
        {
            int hour = timeZoneOffset / 60;
            int minutes = timeZoneOffset % 60;
            string timeZoneValue;
            if (timeZoneOffset > 0)
            {
                timeZoneValue = string.Format("+{0:00}:{1:00}", hour, minutes);
            }
            else
            {
                timeZoneValue = string.Format("{0:00}:{1:00}", hour, minutes);
            }
            return GetTimeZoneDisplayName(timeZoneValue);
        }

        public static DateTime GetNewDateTime(DateTime dateTime, double utcOffsetFrom, double utcOffsetTo)
        {
            return dateTime.AddMinutes(utcOffsetTo - utcOffsetFrom);
        }
    }

    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange()
        {
            this.Start = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            this.End = DateTime.Now;
        }
        
        public DateRange(string startDate, string endDate)
            : this()
        {
            if (!string.IsNullOrEmpty(startDate))
            {
                this.Start = DateUtil.ParseDate(startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                this.End = DateUtil.ParseDate(endDate).AddMilliseconds(DateUtil.MilisecondsPerday - 1);
            }
        }

        public string StartWithShortDate
        {
            get { return DateUtil.FormatDate(Start); }
        }

        public string EndWithShortDate
        {
            get { return DateUtil.FormatDate(End); }
        }

    }
}
