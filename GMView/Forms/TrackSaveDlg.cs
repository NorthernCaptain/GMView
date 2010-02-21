using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class TrackSaveDlg : Form
    {
        public TrackSaveDlg()
        {
            InitializeComponent();
            fileChooser.addCommonPlace("GPS Tracks and Waypoints", Properties.Resources.MapLayers,
                                        new TrackLoadDlg.GPSTrackAndWaypointsBook());

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
            if(System.IO.Directory.Exists(fname))
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
