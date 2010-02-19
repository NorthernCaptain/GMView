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
            poiTypeComboBox.loadList(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int ptypeId = ncUtils.DBSetup.singleton.getInt(this.Name + ".poiType.id", 1);
            Bookmarks.POIType ptype = Bookmarks.POITypeFactory.singleton().typeById(ptypeId);
            if (ptype != null)
                poiTypeComboBox.SelectedItem = ptype;

            fileChooser.DirectoryPath = ncUtils.DBSetup.singleton.getString(this.Name + ".filechooser.dir", string.Empty);
        }

        private void TrackLoadDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bookmarks.POIType ptype = poiTypeComboBox.SelectedItem as Bookmarks.POIType;
            if (ptype != null)
                ncUtils.DBSetup.singleton.setInt(this.Name + ".poiType.id", ptype.Id);
            ncUtils.DBSetup.singleton.setString(this.Name + ".filechooser.dir", fileChooser.DirectoryPath);
        }
    }
}
