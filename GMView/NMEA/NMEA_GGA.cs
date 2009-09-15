using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    public class NMEA_GGA: ncGeo.NMEA_LL
    {
        public static double lastHeight = 0.0;
        public NMEA_GGA(): base()
        {
            utc_time = DateTime.Now;
        }

        public NMEA_GGA(ncGeo.NMEAString str)
            : base(str)
        {
            utc_time = DateTime.Now;
        }

        public override void parse()
        {
            setUTCTime(sentence.parts[1]);
            lat = getDegrees(sentence.parts[2]);
            if (sentence.parts[3] == "S")
                lat = -lat;

            lon = getDegrees(sentence.parts[4]);
            if (sentence.parts[5] == "W")
                lon = -lon;

            if (sentence.parts[6] == "0")
                state = Status.DataWrong;
            else
                state = Status.DataOK;

            usedSats = getInt(sentence.parts[7]);
            HDOP = getDouble(sentence.parts[8]);
            height = getDouble(sentence.parts[9]);
            lastHeight = height;
        }
    }
}
