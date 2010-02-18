using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class TrackLoadDlg : Form
    {
        class GPSTrackAndWaypointsBook : ncFileControls.IDirBookmark
        {
            #region IDirBookmark Members

            public string directory
            {
                get { return Program.opt.default_track_dir; }
            }
            #endregion
        }

        public TrackLoadDlg()
        {
            InitializeComponent();
            fileChooser.addCommonPlace("GPS Tracks and Waypoints", Properties.Resources.MapLayers,
                                        new GPSTrackAndWaypointsBook());
        }
    }
}
