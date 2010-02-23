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
        internal class GPSTrackAndWaypointsBook : ncFileControls.IDirBookmark
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

            fileChooser.fileSelectionChanged += new EventHandler(fileChooser_fileSelectionChanged);
            List<ncFileControls.FileFilter> filters = TrackLoader.TrackLoaderFactory.singleton.getTrackLoadFilters();
            foreach (ncFileControls.FileFilter flt in filters)
            {
                fileChooser.addFileFilter(flt);
            }
        }

        private GPS.TrackFileInfo fileInfo = new GPS.TrackFileInfo();

        /// <summary>
        /// Gets or sets file information about selected track file
        /// </summary>
        public GPS.TrackFileInfo FileInfo
        {
            get { return fileInfo; }
            set { fileInfo = value; }
        }

        void fileChooser_fileSelectionChanged(object sender, EventArgs e)
        {
            if (fileChooser.SelectedFile == null)
            {
                clearDialogInfo();
                okBut.Enabled = false;
                return;
            }
            fileInfo = createFileInfo();
            TrackLoader.ITrackLoader loader = TrackLoader.TrackLoaderFactory.singleton.getTrackLoader(fileInfo);
            if (loader != null)
            {
                doPreLoad(loader, fileInfo);
                okBut.Enabled = true;
            }
            else
                clearDialogInfo();
        }

        /// <summary>
        /// Creates file information filled from the dialog settings and file chooser
        /// </summary>
        /// <returns></returns>
        private GPS.TrackFileInfo createFileInfo()
        {
            GPS.TrackFileInfo fileInfo;
            fileInfo = new GPS.TrackFileInfo(fileChooser.SelectedFile, GPS.TrackFileInfo.SourceType.FileName,
                                             trackColorPicker.SelectedItem);
            fileInfo.needPOI = needPOICB.Checked;
            fileInfo.needTrackSplitting = splitTrackCB.Checked;

            return fileInfo;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size siz = this.Size;
            siz.Width = ncUtils.DBSetup.singleton.getInt(this.Name + ".size.width", siz.Width);
            siz.Height = ncUtils.DBSetup.singleton.getInt(this.Name + ".size.height", siz.Height);
            this.Size = siz;

            int ptypeId = ncUtils.DBSetup.singleton.getInt(this.Name + ".poiType.id", 1);
            Bookmarks.POIType ptype = Bookmarks.POITypeFactory.singleton().typeById(ptypeId);
            if (ptype != null)
                poiTypeComboBox.SelectedItem = ptype;

            fileChooser.DirectoryPath = ncUtils.DBSetup.singleton.getString(this.Name + ".filechooser.dir", string.Empty);

            trackColorPicker.SelectedItem = Color.FromArgb(ncUtils.Glob.rnd.Next(180) + 40,
                                                            ncUtils.Glob.rnd.Next(180) + 40,
                                                            ncUtils.Glob.rnd.Next(180) + 40);
        }

        private void TrackLoadDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bookmarks.POIType ptype = poiTypeComboBox.SelectedItem as Bookmarks.POIType;
            if (ptype != null)
                ncUtils.DBSetup.singleton.setInt(this.Name + ".poiType.id", ptype.Id);
            ncUtils.DBSetup.singleton.setString(this.Name + ".filechooser.dir", fileChooser.DirectoryPath);
        }

        private void doPreLoad( TrackLoader.ITrackLoader loader, GPS.TrackFileInfo fileInfo )
        {
            loader.preLoad(fileInfo);
            trackNameTb.Text = fileInfo.preloadName;
            trackPointsLbl.Text = fileInfo.preloadTPointCount.ToString();
            poisLbl.Text = fileInfo.preloadPOICount.ToString();
            routePointsLbl.Text = fileInfo.preloadRouteCount.ToString();
            POIGroupBox.Enabled = (fileInfo.preloadPOICount > 0);
        }

        private void clearDialogInfo()
        {
            trackNameTb.Text = string.Empty;
            trackPointsLbl.Text = "-";
            poisLbl.Text = "-";
            routePointsLbl.Text = "-";
            POIGroupBox.Enabled = true;
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            //fileInfo = createFileInfo();
            fileInfo.needPOI = needPOICB.Checked;
            fileInfo.needTrackSplitting = splitTrackCB.Checked;
            fileInfo.trackColor = trackColorPicker.SelectedItem;
            fileInfo.defaultPOIType = poiTypeComboBox.SelectedItem as Bookmarks.POIType;
            DialogResult = DialogResult.OK;
        }

        private void TrackLoadDlg_ResizeEnd(object sender, EventArgs e)
        {
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            ncUtils.DBSetup.singleton.setInt(this.Name + ".size.width", this.Size.Width);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".size.height", this.Size.Height);
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }

    }
}
