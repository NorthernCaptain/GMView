using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace ncGeo
{
    /// <summary>
    /// Class contains information about Lon Lat coordinates.
    /// This is a base class for GLL and GGA sentences
    /// </summary>
    public abstract class NMEA_LL: NMEACommand
    {
        public enum PointType
        {
            TP, //Track point
            MWP, //Manual waypoint
            AWP, //Auto waypoint
            SWP, //Stop waypoint
            STARTP, //Start point
            ENDTP,
            MaxPT
        }

        public static PointType parsePointType(string from)
        {
            switch (from)
            {
                case "TP":
                    return PointType.TP;
                case "MWP":
                    return PointType.MWP;
                case "STARTP":
                    return PointType.STARTP;
                case "ENDTP":
                    return PointType.ENDTP;
                case "AWP":
                    return PointType.AWP;
                case "SWP":
                    return PointType.SWP;
                default:
                    return PointType.TP;
            }
        }

        [XmlAttributeAttribute()]
        public double lon;
        [XmlAttributeAttribute()]
        public double lat;
        [XmlAttributeAttribute("h")]
        public double height;
        [XmlAttributeAttribute()]
        public double HDOP;
        [XmlAttributeAttribute("sats")]
        public int usedSats = 0;
        public int visibleSats = 0;
        [XmlAttributeAttribute()]
        public double speed = 0.0;
        /// <summary>
        /// angle of direction of movement in degrees (azimuth)
        /// </summary>
        [XmlAttributeAttribute("azim")]
        public double dir_angle = 0.0;
        [XmlAttribute("type")]
        public PointType ptype = PointType.TP;

        const double dir_len = 60.0;
        const double dir_right_len = 35.0;
        const double dir_speed_multiplier = 0.5;

        [XmlIgnore]
        public Point dir_xy = new Point();
        [XmlIgnore]
        public Point dir_right_xy = new Point();

        public NMEA_LL()
            : base()
        {
        }

        public NMEA_LL(NMEAString nstr):base(nstr)
        {
        }

        public void calculate_dir_xy()
        {
            double dir_rad = dir_angle * Math.PI / 180.0;
            double dir_sin = Math.Sin(dir_rad);
            double dir_cos = Math.Cos(dir_rad);
            double speed_delta = speed * dir_speed_multiplier;
            dir_xy.X = (int)((dir_len + speed_delta) * dir_sin);
            dir_xy.Y = (int)((dir_len + speed_delta) * -dir_cos);
            dir_right_xy.X = (int)(dir_right_len * dir_cos);
            dir_right_xy.Y = (int)(dir_right_len * dir_sin);
        }
    }
}
