using System;
using System.Collections.Generic;
using System.Text;

namespace ncGeo
{
    /// <summary>
    /// Common geo routines as statis members
    /// </summary>
    public class CommonGeo
    {
        /// <summary>
        /// Degrees to radians convertion factor
        /// </summary>
        public const double deg2rad = 0.017453292519943295769236907684886;  //Math.PI / 180.0;

        /// <summary>
        /// average radius of the Earth in km.
        /// </summary>
        public const double avgREarth = 6378.137;

        /// <summary>
        /// Return true if two doubles almost equal, used instead of == operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool almostEqual(double v1, double v2)
        {
            return Math.Abs(v1 - v2) < 0.000001;
        }

        /// <summary>
        /// Calculates distance (in km) between two points with coordinates in lon/lat.
        /// </summary>
        /// <param name="lon1">point 1 lon</param>
        /// <param name="lat1">point 1 lat</param>
        /// <param name="lon2">point 2 lon</param>
        /// <param name="lat2">point 2 lat</param>
        /// <returns>distance in km</returns>
        public static double getDistanceByLonLat(double lon1, double lat1, double lon2, double lat2)
        {
            //convert degrees to radians
            lat1 = Math.PI * lat1 / 180.0;
            lat2 = Math.PI * lat2 / 180.0;
            lon1 = Math.PI * lon1 / 180.0;
            lon2 = Math.PI * lon2 / 180.0;

            double v1 = Math.Sin(lat1) * Math.Sin(lat2);
            double v2 = Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon1 - lon2);
            double d = Math.Acos(v1 + v2);
            return d * avgREarth;
        }

        /// <summary>
        /// More accurate method for distance calculation between two geo coordinates (lon, lat)
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double getDistanceByLonLat2(double lon1, double lat1, double lon2, double lat2)
        {
            //convert degrees to radians
            lat1 = Math.PI * lat1 / 180.0;
            lat2 = Math.PI * lat2 / 180.0;
            lon1 = Math.PI * lon1 / 180.0;
            lon2 = Math.PI * lon2 / 180.0;

            double sinlat = Math.Sin((lat2 - lat1) / 2.0);
            double sinlon = Math.Sin((lon2 - lon1) / 2.0);
            double v1 = sinlat * sinlat + Math.Cos(lat1) * Math.Cos(lat2) * sinlon * sinlon;
            return 2.0 * Math.Asin(Math.Sqrt(v1)) * avgREarth;
        }

    }
}
