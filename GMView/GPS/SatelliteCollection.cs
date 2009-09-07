using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Here we store information about all our visible satellites
    /// Also we mark satellites in use.
    /// </summary>
    public class SatelliteCollection
    {
        public SortedDictionary<int, Satellite> satellites = new SortedDictionary<int, Satellite>();
        private SortedDictionary<int, Satellite> dupsats = new SortedDictionary<int, Satellite>();

        public int satellitesInUse = 0;
        public double HDOP = 0.0;
        public double VDOP = 0.0;
        public double PDOP = 0.0;
        public int hdelusion_idx = 0;
        public NMEA_GSA.FixPos fixPos = NMEA_GSA.FixPos.NoFix;

        public delegate void OnSatelliteChangeDelegate();
        public event OnSatelliteChangeDelegate onSatellitesChanged;

        public SatelliteCollection()
        {
        }

        /// <summary>
        /// We update our collection of satellites according to the new information
        /// from GSV message (satellites in view).
        /// </summary>
        /// <param name="gsvInfo"></param>
        public void updateSatellites(NMEA_GSV gsvInfo)
        {
            lock (this)
            {
                dupsats.Clear();

                foreach (Satellite sat in gsvInfo.satInfo)
                {
                    Satellite ourSat;
                    if (!satellites.TryGetValue(sat.prn, out ourSat))
                    {
                        ourSat = sat;
                    }
                    else
                    {
                        ourSat.copy(sat);
                    }
                    dupsats.Add(ourSat.prn, ourSat);
                }

                SortedDictionary<int, Satellite> dummy = satellites;
                satellites = dupsats;
                dupsats = dummy;
            }

            if (onSatellitesChanged != null)
                onSatellitesChanged();
        }

        /// <summary>
        /// Here we update state of our satellites to InUse, becouse
        /// in GSA message we have this information
        /// </summary>
        /// <param name="gsaInfo"></param>
        public void updateSatellites(NMEA_GSA gsaInfo)
        {
            lock (this)
            {
                satellitesInUse = 0;
                foreach (KeyValuePair<int, Satellite> sat in satellites)
                {
                    sat.Value.state = Satellite.State.Visible;
                }

                for (int i = 0; i < gsaInfo.numSats; i++)
                {
                    int prn = gsaInfo.usedSatNums[i];
                    Satellite ourSat;
                    if (satellites.TryGetValue(prn, out ourSat))
                    {
                        ourSat.state = Satellite.State.InUse;
                        satellitesInUse++;
                    }
                }

                HDOP = gsaInfo.HDOP;
                VDOP = gsaInfo.VDOP;
                PDOP = gsaInfo.PDOP;
                fixPos = gsaInfo.fixPos;
                do
                {
                    if (HDOP <= 1.0)
                    {
                        //ideal quality
                        hdelusion_idx = 0;
                        break;
                    }
                    if (HDOP <= 2.0)
                    {
                        //excellent quality
                        hdelusion_idx = 1;
                        break;
                    }
                    if (HDOP <= 4.0)
                    {
                        //good quality
                        hdelusion_idx = 2;
                        break;
                    }
                    if (HDOP <= 7.0)
                    {
                        //moderate quality
                        hdelusion_idx = 3;
                        break;
                    }
                    //fair quality
                    hdelusion_idx = 4;
                } while (false);

            }

            if (onSatellitesChanged != null)
                onSatellitesChanged();
        }
    }
}
