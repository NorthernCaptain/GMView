using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GMView.Forms
{
    public partial class TrackSaveDlg : Form
    {
        public TrackSaveDlg()
        {
            InitializeComponent();
            fileChooser.addCommonPlace("GPS Tracks and Waypoints", Properties.Resources.MapLayers,
                                        new TrackLoadDlg.GPSTrackAndWaypointsBook());

            List<ncFileControls.FileFilter> filters = TrackLoader.TrackLoaderFactory.singleton.getTrackSaveFilters();
            foreach (ncFileControls.FileFilter flt in filters)
            {
                fileChooser.addFileFilter(flt);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size siz = this.Size;
            siz.Width = ncUtils.DBSetup.singleton.getInt(this.Name + ".size.width", siz.Width);
            siz.Height = ncUtils.DBSetup.singleton.getInt(this.Name + ".size.height", siz.Height);
            this.Size = siz;
        }

        private GPSTrack currentTrack;

        /// <summary>
        /// Sets the current track that we want to save.
        /// </summary>
        public GPSTrack CurrentTrack
        {
            get { return currentTrack; }
            set 
            { 
                currentTrack = value;
                trackNameTb.Text = currentTrack.track_name;
                FileName = currentTrack.fileName;
                trackPointsNumLbl.Text = currentTrack.countPoints.ToString();

                double minlat, minlon, maxlat, maxlon;

                currentTrack.getBounds(out minlon, out minlat, out maxlon, out maxlat);

                double delta_lon = (maxlon - minlon) / 25;
                double delta_lat = (maxlat - minlat) / 25;

                List<Bookmark> booklist = BookMarkFactory.singleton.getBookmarksByBounds(
                    minlon - delta_lon, minlat - delta_lat, maxlon + delta_lon, maxlat + delta_lat);

                poiNumLbl.Text = booklist.Count.ToString();
            }
        }


        /// <summary>
        /// Gets or sets current file name
        /// </summary>
        public string FileName
        {
            get
            {
                return fileChooser.SelectedFile;
            }

            set 
            {
                fileChooser.SelectedFile = value;
            }
        }

        /// <summary>
        /// Gets file information, not only its name, but also the options
        /// </summary>
        public GPS.TrackFileInfo FileInfo
        {
            get
            {
                string fname = fileChooser.SelectedFile;
                if(string.IsNullOrEmpty(fname))
                    return null;

                string ext = Path.GetExtension(fname);
                try
                {
                    if (string.IsNullOrEmpty(ext))
                    {
                        string defExt = Path.GetExtension(fileChooser.currentFilter.Filter);
                        fname = Path.ChangeExtension(fname, (defExt.Equals(".*") ? ".gpx" : defExt));
                    }
                }
                catch (System.Exception)
                {

                }

                if (string.IsNullOrEmpty(System.IO.Path.GetExtension(fname)))
                    fname += ".gpx";
                GPS.TrackFileInfo nfo = new GPS.TrackFileInfo(fname, GPS.TrackFileInfo.SourceType.FileName);
                nfo.needPOI = this.savePOICb.Checked;
                nfo.preloadName = trackNameTb.Text;
                return nfo;
            }
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            string fname = fileChooser.SelectedFile;

            if(fname == null)
            {
                MessageBox.Show("No file name specified!\nPlease, supply a file name!",
                    "File name required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ext = Path.GetExtension(fname);
            try
            {
                if (string.IsNullOrEmpty(ext))
                {
                    string defExt = Path.GetExtension(fileChooser.currentFilter.Filter);
                    fname = Path.ChangeExtension(fname, (defExt.Equals(".*") ? ".gpx" : defExt));
                }
            }
            catch (System.Exception)
            {

            }

            if (System.IO.Directory.Exists(fname))
            {
                MessageBox.Show("You selected directory, please, specify a file name!",
                    "File name required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("File '" + System.IO.Path.GetFileName(fname) + "' already exists.\n"
                    + "Do you want to overwrite it?", "File exists", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.No)
                    return;
            }
            DialogResult = DialogResult.OK;
        }

        private void TrackSaveDlg_ResizeEnd(object sender, EventArgs e)
        {
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            ncUtils.DBSetup.singleton.setInt(this.Name + ".size.width", this.Size.Width);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".size.height", this.Size.Height);
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }
    }
}
