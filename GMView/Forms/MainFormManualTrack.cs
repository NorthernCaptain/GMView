using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    partial class GMViewForm
    {
        private GPSTrack manual_track;

        private void initManualTrackMode()
        {
            manual_track = new GPSTrack(mapo);
            manual_track.trackMode = GPSTrack.TrackMode.ViewSaved;
            manual_track.need_arrows = false;
            manual_track.track_name = "Manual track";
            mapo.addSub(manual_track);

            onMTrackLeftClick += new onMouseActionDelegate(GMViewForm_onMTrackLeftClick);
            onMTrackRightClick += new onMouseActionDelegate(GMViewForm_onMTrackRightClick);
        }

        void GMViewForm_onMTrackRightClick(Point mouse_p)
        {
            manual_track.delLastPoint();
            repaintMap();
        }

        void GMViewForm_onMTrackLeftClick(Point mouse_p)
        {
            double lon, lat;
            mapo.getLonLatByVisibleXY(mouse_p, out lon, out lat);
            NMEA_LL nmea_ll = new NMEA_RMC(lon, lat, NMEA_LL.PointType.MWP);
            manual_track.addManualPoint(nmea_ll);
            manual_track.show();
            repaintMap();
        }
    }
}
