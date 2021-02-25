using System;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLight
{
    public static class Helpers
    {
        static readonly SortedList<double, Func<TimeSpan, string>> offsets =
            new SortedList<double, Func<TimeSpan, string>>{
                { 0.75, _ => "less than a minute"},
                { 1.5, _ => "about a minute"},
                { 45, x => $"{x.TotalMinutes:F0} minutes"},
                { 90, x => "about an hour"},
                { 1440, x => $"about {x.TotalHours:F0} hours"},
                { 2880, x => "a day"},
                { 43200, x => $"{x.TotalDays:F0} days"},
                { 86400, x => "about a month"},
                { 525600, x => $"{x.TotalDays / 30:F0} months"},
                { 1051200, x => "about a year"},
                { double.MaxValue, x => $"{x.TotalDays / 365:F0} years"}
            };

        public static string ToRelativeDate(this DateTime input)
        {
            TimeSpan x = DateTime.Now - input;
            string Suffix = x.TotalMinutes > 0 ? " ago" : " from now";
            x = new TimeSpan(Math.Abs(x.Ticks));
            return offsets.First(n => x.TotalMinutes < n.Key).Value(x) + Suffix;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp/1000).ToLocalTime();
            return dtDateTime;
        }
    }
}
