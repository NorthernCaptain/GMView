using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Realises GSA sentence parsing
    /// </summary>
    public class NMEA_GSA: NMEACommand
    {
        public int [] usedSatNums = new int[12];
        public int numSats = 0;

        public enum FixPos { NoFix, Fix2D, Fix3D };

        public FixPos fixPos = FixPos.NoFix;
        public double HDOP;
        public double VDOP;
        public double PDOP;

        public static double lastHDOP = 0.0;
        public static int lastUsedSats = 0;

        public NMEA_GSA(NMEAString str)
            : base(str)
        {
        }

        public override void parse()
        {
            numSats = 0;
            fixPos = (FixPos)getInt(sentence.parts[2]);
            for (int i = 0; i < 12; i++)
            {
                usedSatNums[i] = getInt(sentence.parts[i+3]);
                if (usedSatNums[i] > 0)
                    numSats++;
            }
            PDOP = getDouble(sentence.parts[15]);
            HDOP = getDouble(sentence.parts[16]);
            VDOP = getDouble(sentence.parts[17]);
            state = Status.DataOK;
            lastHDOP = HDOP;
            lastUsedSats = numSats;
        }
    }
}
