using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Realises VTG sentence parsing
    /// </summary>
    public class NMEA_VTG: ncGeo.NMEACommand
    {
        public double speed = 0.0;

        public NMEA_VTG(ncGeo.NMEAString str)
            : base(str)
        {
        }

        public override void parse()
        {
            speed = getDouble(sentence.parts[4]);
            state = Status.DataOK;
        }
    }
}
