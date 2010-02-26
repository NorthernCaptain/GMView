using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class TrackLoadControl : UserControl
    {
        public TrackLoadControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadState();
        }

        /// <summary>
        /// Loads state of the control from DB
        /// </summary>
        private void loadState()
        {
            trackColorPicker.SelectedItem = Color.FromArgb(ncUtils.Glob.rnd.Next(180) + 40,
                                                            ncUtils.Glob.rnd.Next(180) + 40,
                                                            ncUtils.Glob.rnd.Next(180) + 40);

            showTrackInfoCB.Checked =
                (ncUtils.DBSetup.singleton.getInt(this.Name + ".showInfo.checked", 1) == 0 ? false : true);
        }

        /// <summary>
        /// Save current state of the control to the DB
        /// </summary>
        public void saveState()
        {
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            ncUtils.DBSetup.singleton.setInt(this.Name + ".showInfo.checked", showTrackInfoCB.Checked ? 1 : 0);
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }

        private GPS.TrackFileInfo fileInfo;

        /// <summary>
        /// Gets or sets FileInformation object
        /// </summary>
        public GPS.TrackFileInfo FileInfo
        {
            get
            {
                if (fileInfo == null)
                    return null;

                setInfoInto(fileInfo);
                return fileInfo;
            }
            set
            {
                fileInfo = value;
                if (fileInfo == null)
                {
                    trackPointsLbl.Text = "-";
                    routePointsLbl.Text = "-";
                    trackNameTb.Text = "";
                }
                else
                {
                    trackPointsLbl.Text = fileInfo.preloadTPointCount.ToString();
                    routePointsLbl.Text = fileInfo.preloadRouteCount.ToString();
                    trackNameTb.Text = fileInfo.preloadName;
                }
            }
        }


        /// <summary>
        /// Sets information from the control to the given info object
        /// </summary>
        /// <param name="fileInfo"></param>
        public void setInfoInto(GPS.TrackFileInfo fileInfo)
        {
            if (fileInfo == null)
                return;

            fileInfo.trackColor = trackColorPicker.SelectedItem;
            fileInfo.showInfoForm = showTrackInfoCB.Checked;
            fileInfo.needTrackSplitting = splitTrackCB.Checked;
        }
    }
}
