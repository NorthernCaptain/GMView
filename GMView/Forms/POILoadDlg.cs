using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class POILoadDlg : Form
    {
        public POILoadDlg()
        {
            InitializeComponent();
            fileChooser.addCommonPlace("GPS Tracks and Waypoints", Properties.Resources.MapLayers,
                                        new GMView.Forms.TrackLoadDlg.GPSTrackAndWaypointsBook());
            fileChooser.fileSelectionChanged += new EventHandler(fileChooser_fileSelectionChanged);
            List<ncFileControls.FileFilter> filters = TrackLoader.TrackLoaderFactory.singleton.getPOILoadFilters();
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
            TrackLoader.IPOILoader loader = TrackLoader.TrackLoaderFactory.singleton.getPOILoader(fileInfo);
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
            fileInfo = new GPS.TrackFileInfo(fileChooser.SelectedFile, GPS.TrackFileInfo.SourceType.FileName);

            poiLoadControl.setInfoInto(fileInfo);

            return fileInfo;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Size siz = this.Size;
            siz.Width = ncUtils.DBSetup.singleton.getInt(this.Name + ".size.width", siz.Width);
            siz.Height = ncUtils.DBSetup.singleton.getInt(this.Name + ".size.height", siz.Height);
            this.Size = siz;

            poiLoadControl.NeedPOICB.Enabled = false;
            poiLoadControl.NeedPOICB.Checked = true;
            fileChooser.DirectoryPath = ncUtils.DBSetup.singleton.getString(this.Name + ".filechooser.dir", string.Empty);

        }

        private void POILoadDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            poiLoadControl.saveState();
            ncUtils.DBSetup.singleton.setString(this.Name + ".filechooser.dir", fileChooser.DirectoryPath);
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }

        private void doPreLoad(TrackLoader.IPOILoader loader, GPS.TrackFileInfo fileInfo)
        {
            loader.preLoad(fileInfo);
            poiLoadControl.FileInfo = fileInfo;
        }

        private void clearDialogInfo()
        {
            poiLoadControl.FileInfo = null;
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            //fileInfo = createFileInfo();

            poiLoadControl.setInfoInto(fileInfo);

            DialogResult = DialogResult.OK;
        }

        private void POILoadDlg_ResizeEnd(object sender, EventArgs e)
        {
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            ncUtils.DBSetup.singleton.setInt(this.Name + ".size.width", this.Size.Width);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".size.height", this.Size.Height);
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }

    }
}
