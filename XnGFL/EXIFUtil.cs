using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XnGFL
{
    /// <summary>
    /// Class containes various static utils (helpers) for parsing EXIF
    /// </summary>
    public class EXIFUtil
    {
        private static Regex dateTimeParse = new Regex(
        @"(?<year>\d+)\:(?<month>\d+):(?<day>\d+)\ (?<hour>\d+):(?<minute>\d+):(?<second>\d+)"
        );

        private static Regex dateParse = new Regex(@"(?<year>\d+)\:(?<month>\d+):(?<day>\d+)");

        /// <summary>
        /// Parse EXIF date and time string and return DateTime object
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(String s)
        {
            Match m = dateTimeParse.Match(s);
            return new DateTime(
                Int32.Parse(m.Groups["year"].Value),
                Int32.Parse(m.Groups["month"].Value),
                Int32.Parse(m.Groups["day"].Value),
                Int32.Parse(m.Groups["hour"].Value),
                Int32.Parse(m.Groups["minute"].Value),
                Int32.Parse(m.Groups["second"].Value)
                );
        }

        /// <summary>
        /// Format DateTime object into EXIF date+time string
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static String FormatDateTime(DateTime time)
        {
            return time.ToString("yyyy:MM:dd HH:mm:ss");
        }

        /// <summary>
        /// Parse EXIF date only string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ParseDate(String s)
        {
            Match m = dateParse.Match(s);
            return new DateTime(
                Int32.Parse(m.Groups["year"].Value),
                Int32.Parse(m.Groups["month"].Value),
                Int32.Parse(m.Groups["day"].Value),
                0, 0, 0
                );
        }

        public static String FormatDate(DateTime time)
        {
            return time.ToString("yyyy:MM:dd");
        }

        /// <summary>
        /// Converts EXIF string of degrees into double degrees value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double EXIFtoGPSDegrees(string s)
        {
            return ncUtils.Glob.parseLonLat(s);
        }
    }
}
