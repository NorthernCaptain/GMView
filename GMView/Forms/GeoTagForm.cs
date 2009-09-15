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


        private void trackList_onChanged(GPSTrackFactory factory)
        {
        }

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
