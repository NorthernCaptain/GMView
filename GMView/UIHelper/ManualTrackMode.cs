using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMView.UIHelper
{
    /// <summary>
    /// Create new track by manually defining waypoints
    /// </summary>
    public class ManualTrackMode: MouseBaseProc
    {
        private GPSTrack manual_track;

        public GPSTrack manualTrack
        {
            get { return manual_track; }
            set 
            {
                if (manual_track != null)
                {
                    GPSTrackFactory.singleton.delTrack(manual_track);
                    mainform.mapo.delSub(manual_track);
                }

                manual_track = value;
                if (manual_track == null)
                    initTrack();
                GPSTrackFactory.singleton.addTrack(manual_track);
                mainform.mapo.addSub(manual_track);
            }
        }

        private void initTrack()
        {
            manual_track = new GPSTrack(mainform.mapo);
            manual_track.trackMode = GPSTrack.TrackMode.ViewSaved;
            manual_track.need_arrows = false;
            manual_track.track_name = "Manual track";
        }

        public ManualTrackMode(GMViewForm form, UserControl dPane)
        {
            mainform = form;
            drawPane = dPane;
        }

        public override void initGL()
        {
            if (manual_track != null)
                manual_track.initGLData();
        }

        public override void modeEnter(MouseBaseProc oldone)
        {
            base.modeEnter(oldone);
        }

        public override void modeLeave()
        {
            base.modeLeave();
        }

        protected override bool onMouseUpTranslated(System.Drawing.Point xy)
        {
            if (manual_track == null)
                return false;

            if(eargs.Button == MouseButtons.Right)
            {
                removeLastPoint();
            } else
            {
                double lon, lat;
                mainform.mapo.getLonLatByVisibleXY(xy, out lon, out lat);
                addNewPoint(lon, lat);
            }
            GML.repaint();
            return true;
        }

        public void resetTrack()
        {
            manual_track.resetTrackData();
        }

        /// <summary>
        /// Remove last point from track
        /// </summary>
        private void removeLastPoint()
        {
            manual_track.delLastPoint();
        }

        /// <summary>
        /// Add new waypoint to the track with given coordinates
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        private void addNewPoint(double lon, double lat)
        {
            ncGeo.NMEA_LL nmea_ll = new NMEA_RMC(lon, lat, ncGeo.NMEA_LL.PointType.MWP);
            manual_track.addManualPoint(nmea_ll);
            manual_track.show();
        }
    }
}
