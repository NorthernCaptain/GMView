using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Realises GSV (satellites in view) sentence parsing
    /// </summary>
    public class NMEA_GSV: NMEACommand
    {
        private static NMEA_GSV incomplete_gsv = null;
        public static NMEA_GSV getNewGSV(NMEAString str)
        {
            if (incomplete_gsv == null || incomplete_gsv.readyForProcessing())
            {
                incomplete_gsv = new NMEA_GSV(str);
            }
            else
                incomplete_gsv.sentence = str;

            return incomplete_gsv;
        }


        private int total_messages=0;
        private bool completed = false;

        public List<Satellite> satInfo;
        public static int lastVisibleSats = 0;

        public NMEA_GSV(NMEAString str)
            : base(str)
        {
            satInfo = new List<Satellite>();
        }

        public override bool readyForProcessing()
        {
            return completed;
        }

        public override void parse()
        {
            total_messages = getInt(sentence.parts[1]);
            completed = total_messages == getInt(sentence.parts[2]);
            int count = sentence.parts.Count;
            fillSatInfo(4);
            if (count >= 12)
            {
                fillSatInfo(8);
                if (count >= 16)
                {
                    fillSatInfo(12);
                    if(count >= 20)
                        fillSatInfo(16);
                }
            }
            if (completed)
            {
                lastVisibleSats = satInfo.Count;
            }
        }

        private void fillSatInfo(int idx)
        {
            try
            {
                int prn = getInt(sentence.parts[idx++]);
                if (prn == 0)
                    return;
                Satellite sat = new Satellite();
                sat.prn = prn;
                sat.height = getInt(sentence.parts[idx++]);
                sat.azimuth = getInt(sentence.parts[idx++]);
                sat.signal_strength = getInt(sentence.parts[idx++]);
                sat.state = Satellite.State.Visible;
                satInfo.Add(sat);
            }
            catch { };
        }

    }
}
