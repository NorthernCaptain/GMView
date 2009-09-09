using System;
using System.Collections.Generic;
using System.Text;

namespace ncUtils
{
    /// <summary>
    /// Class provides global variables and some utility handy methods
    /// </summary>
    public class Glob
    {
        static System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("");
        /// <summary>
        /// We use it for formating floating point number always with . (dot), not ',' comma
        /// </summary>
        public static System.Globalization.NumberFormatInfo numformat = cul.NumberFormat;

        public static Random rnd = new Random((int)DateTime.Now.Ticks);

        public static long lastRefreshTicks = 0L;

        private static string[] course_names = new string[] 
        {
            "N", "NNE", "NE", "ENE", "E",
            "ESE", "SE", "SSE", "S",
            "SSW", "SW", "WSW", "W",
            "WNW", "NW", "NNW", "N"
        };

        public static string lonString(double lon)
        {
            return Math.Abs(lon).ToString("F4") + (lon < 0.0 ? " W" : " E");
        }

        public static string latString(double lat)
        {
            return Math.Abs(lat).ToString("F4") + (lat < 0.0 ? " S" : " N");
        }

        public static string lonlatDigitString(double val)
        {
            return val.ToString("F8");
        }

        public static string courseString(double dir_angle)
        {
            dir_angle %= 360.0;
            return dir_angle.ToString("F1") + " " + course_names[(int)((dir_angle + 11.25) / 22.5)];
        }

        public static double parseLonLat(string lon)
        {
            double result = 0.0;
            char[] symb = new char[] { ',', '.', '\'', '°', '"' };
            string[] parts = lon.Split(symb);
    
            if (parts.Length <= 2) // we have lon or lat in the format of degrees.fraction_of_degrees
            {
                int integral = int.Parse(parts[0]);
                
                double fractional;
                try
                {
                    fractional = double.Parse(parts[1]);
                    result = (double)(Math.Sign(integral))*(Math.Abs((double)integral) + fractional / Math.Pow(10.0, (double)parts[1].Length));
                }
                catch
                {
                    result = integral;
                }
                return result;
            }
            if (parts.Length == 3) // we have format: degrees,minutes.fraction_of_minutes
            {
                int integral = int.Parse(parts[0]);
                int minutes = int.Parse(parts[1]);
                double fractional = double.Parse(parts[2]);

                result = (double)minutes + fractional / Math.Pow(10.0, (double)parts[2].Length);
                result = (double)(Math.Sign(integral))*(Math.Abs((double)integral) + result / 60.0);
                return result;
            }
            if (parts.Length >= 4) // we have degrees,minutes,seconds.fraction_of_seconds
            {
                int integral = (int)parseRationalToDouble(parts[0]);
                int minutes = (int) parseRationalToDouble(parts[1]);
                int seconds = (int) parseRationalToDouble(parts[2]);
                double fractional = double.Parse(parts[3].Length > 8 ? parts[3].Substring(0, 8) : parts[3]);

                result = (double)seconds + fractional / Math.Pow(10.0, (double)parts[3].Length);
                result = (double)minutes + result / 60.0;
                result = (double)(Math.Sign(integral))*(Math.Abs((double)integral) + result / 60.0);
            }
            return result;
        }

        /// <summary>
        /// Parses rational string value (ex: 14/3) and return double.
        /// </summary>
        /// <param name="rval"></param>
        /// <returns></returns>
        public static double parseRationalToDouble(string rval)
        {
            if (rval.Contains("/"))
            {
                string[] part = rval.Split('/');
                return double.Parse(part[0]) / double.Parse(part[1]);
            }
            return double.Parse(rval);
        }
    }
}
