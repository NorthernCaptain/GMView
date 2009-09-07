using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView
{
    partial class GMViewForm
    {
        #region GPS and NMEA processing
        void gtrack_onTrackChanged()
        {
            GML.tranBegin();
            NMEA_LL nmeall = gtrack.lastData;
            if (nmeall != null)
            {
                if (opt.gps_follow_map)
                {
                    if (mapo.ensureCentered(nmeall.lon, nmeall.lat))
                    {
                        if (opt.gps_rotate_map)
                        {
                            NMEA_LL nonzero = gtrack.lastNonZeroPos;
                            mapo.angle = nonzero.dir_angle;
                            ((IGML)drawPane).angle = mapo.angle;
                        }
                    }
                }
                repaintMap();
                gpsStatSLb.Text = "GPS: " + nmeall.lon.ToString("F3") + " " + nmeall.lat.ToString("F3");
            }
            GML.tranEnd();
        }

        void satellites_onSatellitesChanged()
        {
            GML.tranBegin();
            if (!opt.is_full_screen)
            {
                satInfoSLb.Text = "Sat: " + satellites.satellitesInUse + "/" + satellites.satellites.Count
                    + " HDOP:" + satellites.HDOP.ToString("F1") + " " + satellites.fixPos.ToString();
            }
            DateTime now = DateTime.Now;
            if (now.Ticks - lastRepaint.Ticks > 2000000L)
                repaintMap();
            GML.tranEnd();
        }

        /// <summary>
        /// Process nmea command. This method will be called in nmea thread, not in main one.
        /// </summary>
        /// <param name="cmd"></param>
        void nmea_thread_onNMEACommand(NMEACommand cmd)
        {
            switch (cmd.type)
            {
                case "GSV":
                    {
                        NMEA_GSV gsv = cmd as NMEA_GSV;
                        satellites.updateSatellites(gsv);
                    }
                    break;
                case "GSA":
                    {
                        NMEA_GSA gsa = cmd as NMEA_GSA;
                        satellites.updateSatellites(gsa);
                    }
                    break;
                default:
                    {
                        NMEA_LL nmeall = cmd as NMEA_LL;
                        if (nmeall != null)
                        {
                            gtrack.newGPSData(nmeall);
                            gtrack_mini.newGPSData(nmeall);
                        }
                    }
                    break;
            }

            this.Invoke(new NMEAThread.OnNMEACommandDelegate(NMEAProcessCommand), new Object[] { cmd });
        }

        private bool com_state_ok = true;
        void nmea_thread_onLogNMEAString(NMEAString nmeastr)
        {
            try
            {
                if(opt.nmea_log)
                    this.Invoke(new NMEAThread.OnNMEAStingDelegate(logWin.NMEALog), new Object[] { nmeastr });
                if (com_state_ok == false && nmeastr.error == null)
                {
                    com_state_ok = true;
                    this.Invoke(new NMEAThread.OnNMEAStingDelegate(comStateLamp), new Object[] { nmeastr });
                } else
                if (com_state_ok == true && nmeastr.error != null)
                {
                    com_state_ok = false;
                    this.Invoke(new NMEAThread.OnNMEAStingDelegate(comStateLamp), new Object[] { nmeastr });
                }
            }
            catch { };
        }


        /// <summary>
        /// This method will be called in main thread after new nmea data has been processed in nmea thread.
        /// Here we update our visual information according to a new data arrived.
        /// </summary>
        /// <param name="cmd"></param>
        void NMEAProcessCommand(NMEACommand cmd)
        {
            NMEA_LL nmeall = cmd as NMEA_LL;
            if (nmeall != null)
            {
                setGPSLamp(cmd);
                gtrack_onTrackChanged();
            }
            else
                satellites_onSatellitesChanged();
        }


        private void setGPSLamp(NMEACommand cmd)
        {
            if (cmd.state == NMEACommand.Status.DataOK)
            {
                gpsLampSLb.Image = (com_lamp_state % 3) == 1 ? global::GMView.Properties.Resources.lamp_on2
                    : global::GMView.Properties.Resources.lamp_on;
            }
            else
            {
                gpsLampSLb.Image = (com_lamp_state % 3) == 1 ? global::GMView.Properties.Resources.lamp_off2
                    : global::GMView.Properties.Resources.lamp_off;
            }
        }

        void comStateLamp(NMEAString str)
        {
            com_lamp_state++;
            if (str.error == null)
            {
                comStateSLb.Image = (com_lamp_state % 2) == 1 ? global::GMView.Properties.Resources.lamp_on2
                    : global::GMView.Properties.Resources.lamp_on;
            }
            else
            {
                comStateSLb.Image = (com_lamp_state % 2) == 1 ? global::GMView.Properties.Resources.lamp_off2
                    : global::GMView.Properties.Resources.lamp_off;
            }
        }

        #endregion

    }
}
