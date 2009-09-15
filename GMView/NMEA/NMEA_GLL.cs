using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Realises GLL NMEA command parsing
    /// </summary>
    public class NMEA_GLL: ncGeo.NMEA_LL
    {
        public NMEA_GLL(ncGeo.NMEAString str)
            : base(str)
        {
            utc_time = DateTime.Now;
        }

        public override void parse()
        {
            lat = getDouble(sentence.parts[1]);
            if (sentence.parts[2] == "S")
                lat = -lat;

            lon = getDouble(sentence.parts[3]);
            if (sentence.parts[4] == "W")
                lon = -lon;

            setUTCTime(sentence.parts[5]);

            if (sentence.parts[6] == "A")
                state = Status.DataOK;
            else
                state = Status.DataWrong;
        }
    }
}
