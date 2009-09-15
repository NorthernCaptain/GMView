using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Realises RMC sentence parsing
    /// </summary>
    public class NMEA_RMC: ncGeo.NMEA_LL
    {
        public const double knot2km = 1.852;

        public NMEA_RMC()
            : base()
        {
        }

        public NMEA_RMC(ncGeo.NMEAString str)
            : base(str)
        {
        }

        public NMEA_RMC(double ilon, double ilat, PointType pt)
        {
            lon = ilon;
            lat = ilat;
            ptype = pt;
            this.state = Status.DataOK;
            utc_time = DateTime.Now;
        }

        public override void parse()
        {
            setUTCTime(sentence.parts[1]);
            setUTCDate(sentence.parts[9]);

            if (sentence.parts[2] == "A")
                state = Status.DataOK;
            else
                state = Status.DataWrong;

            lat = getDegrees(sentence.parts[3]);
            if (sentence.parts[4] == "S")
                lat = -lat;

            lon = getDegrees(sentence.parts[5]);
            if (sentence.parts[6] == "W")
                lon = -lon;

            speed = getDouble(sentence.parts[7]) * knot2km;
            dir_angle = getDouble(sentence.parts[8]);
            HDOP = NMEA_GSA.lastHDOP; //last HDOP from GSA message, we don't have it here
            usedSats = NMEA_GSA.lastUsedSats; //last used number of satellites from GSV message, same reason
            height = NMEA_GGA.lastHeight;
            visibleSats = NMEA_GSV.lastVisibleSats;
            calculate_dir_xy();
        }
    }
}
