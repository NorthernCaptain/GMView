using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class GeoTagForm : Form
    {
        private XnGFL.ExifViewControl exifcontrol;

        public GeoTagForm(XnGFL.ExifViewControl eview)
        {
            exifcontrol = eview;
            InitializeComponent();

            exifcontrol.Dock = DockStyle.Fill;
            exifcontrol.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            exifcontrol.Size = this.ClientSize;
            this.Controls.Add(exifcontrol);
            this.PerformLayout();

            Program.onShutdown += shutdown;

            BookMarkFactory.singleton.onChanged += bookmark_onChanged;
            bookmark_onChanged(BookMarkFactory.singleton);

            GPSTrackFactory.singleton.onTrackListChanged += trackList_onChanged;
            trackList_onChanged(GPSTrackFactory.singleton);
        }

        /// <summary>
        /// Calls when bookmarks were changed, reloads info into exifcontrol
        /// </summary>
        /// <param name="factory"></param>
        private void bookmark_onChanged(BookMarkFactory factory)
        {
            if (factory.bookmarks.Count == 0)
            {
                exifcontrol.setPOIs(null);
                return;
            }

            Bookmark[] arr = new Bookmark[factory.bookmarks.Count];
            factory.bookmarks.Values.CopyTo(arr, 0);
            exifcontrol.setPOIs(arr);
        }

        /// <summary>
        /// Calls when tracklist were changed, reloads info into exifcontrol
        /// </summary>
        /// <param name="factory"></param>
        private void trackList_onChanged(GPSTrackFactory factory)
        {
            if (factory.trackList.Count == 0)
            {
                exifcontrol.setTracks(null);
                return;
            }

            ncGeo.IGPSTrack[] arr = new ncGeo.IGPSTrack[factory.trackList.Count];
            int idx = 0;
            foreach (ncGeo.IGPSTrack track in factory.trackList)
                arr[idx++] = track;

            exifcontrol.setTracks(arr);
        }

        /// <summary>
        /// do not dispose on closing, just hide it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeoTagForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            this.Visible = false;
        }

        private void shutdown()
        {
            exifcontrol.shutdown();
        }

    }
}
