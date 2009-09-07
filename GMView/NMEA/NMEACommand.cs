using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GMView
{
    /// <summary>
    /// Abstract base class for all NMEA commands
    /// </summary>
    public abstract class NMEACommand
    {
        private static System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
        private static System.Globalization.NumberFormatInfo nf = cul.NumberFormat;

        //[ XmlElementAttribute("nmea_data") ]
        [XmlIgnore]
        public NMEAString sentence;
        [XmlAttribute("time")]
        public DateTime utc_time;

        public enum Status { DataOK, DataWrong, Unknown };

        [XmlIgnore]
        public Status state = Status.Unknown;

        [XmlIgnore]
        public string type
        {
            get
            {
                return sentence.parts[0];
            }
            set { }
        }

        public NMEACommand()
        {
        }

        public NMEACommand(NMEAString isentence)
        {
            sentence = isentence;
        }

        /// <summary>
        /// This method parses command and fills in all information
        /// </summary>
        public abstract void parse();

        /// <summary>
        /// Sets time component utc_time from a given string.
        /// </summary>
        /// <param name="fromStr"></param>
        public virtual void setUTCTime(string fromStr)
        {
            utc_time = new DateTime(utc_time.Year, utc_time.Month, utc_time.Day,
                int.Parse(fromStr.Substring(0, 2)), //HH
                int.Parse(fromStr.Substring(2, 2)), //MM
                int.Parse(fromStr.Substring(4, 2)), //SS
                0, //MilliSec
                DateTimeKind.Utc);
        }

        /// <summary>
        /// Sets the date component to the utc_time DateTime variable
        /// </summary>
        /// <param name="fromStr"></param>
        public virtual void setUTCDate(string fromStr)
        {
            if (fromStr.Length < 6)
                return;

            int year = int.Parse(fromStr.Substring(4, 2));
            year = year > 80 ? year + 1900 : year + 2000;
            utc_time = new DateTime(year,
                int.Parse(fromStr.Substring(2, 2)), //MM
                int.Parse(fromStr.Substring(0, 2)), //DD
                utc_time.Hour, utc_time.Minute, utc_time.Second,
                0, //MilliSec
                DateTimeKind.Utc);
        }

        public static double getDouble(string str)
        {
            if (str == null || str.Length == 0)
                return 0.0;
            try
            {
                return double.Parse(str, nf);
            }
            catch { };
            return 0.0;
        }

        public double getDegrees(string str)
        {
            double val = getDouble(str); //Value in format: DDDMM.MMMM - DDD- degrees, MM - minutes, MMMM - fraction part of minutes
            double v1 = val % 100; //get minutes from degmin.mm
            double v2 = Math.Floor(val / 100.0); // get degrees only
            v2 += v1 / 60.0; //convert minutes to fractional part of degrees
            return v2;
        }

        protected int getInt(string str)
        {
            if (str == null || str.Length == 0)
                return 0;
            try
            {
                return int.Parse(str);
            }
            catch { };
            return 0;
        }

        public virtual bool readyForProcessing() { return true; }
    }
}
