using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GMView.Forms
{
    public partial class POILoadControl : UserControl
    {
        public POILoadControl()
        {
            InitializeComponent();
            poiTypeComboBox.loadList(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadState();
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
                    poisLbl.Text = "-";
                } else
                {
                    poisLbl.Text = fileInfo.preloadPOICount.ToString();
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

            fileInfo.needPOI = needPOICB.Checked;
            if (fileInfo.needPOI)
            {
                fileInfo.defaultPOIType = poiTypeComboBox.SelectedItem as Bookmarks.POIType;
            }

            if (tuneNameCommRb.Checked)
                fileInfo.nameSwap = GMView.GPS.TrackFileInfo.POISwapNamesType.NoChanges;
            else
                if (tuneNameDescRb.Checked)
                    fileInfo.nameSwap = GMView.GPS.TrackFileInfo.POISwapNamesType.SwapNameDesc;
                else
                    fileInfo.nameSwap = GMView.GPS.TrackFileInfo.POISwapNamesType.SwapNameComment;
            fileInfo.descSwap = (tuneDescCommCB.Checked ? GMView.GPS.TrackFileInfo.POISwapNamesType.SwapDescComment :
                                    GMView.GPS.TrackFileInfo.POISwapNamesType.NoChanges);
        }

        /// <summary>
        /// Saves state of the control to DB
        /// </summary>
        public void saveState()
        {
            Bookmarks.POIType ptype = poiTypeComboBox.SelectedItem as Bookmarks.POIType;
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            if (ptype != null)
                ncUtils.DBSetup.singleton.setInt(this.Name + ".poiType.id", ptype.Id);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".needPOI.checked", needPOICB.Checked ? 1 : 0);
            /* Uncomment this if we want to save state of tune options
            ncUtils.DBSetup.singleton.setInt(this.Name + ".tuneDescComm.checked", tuneDescCommCB.Checked ? 1 : 0);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".tuneNoChangeRb.checked", tuneNoChangesRb.Checked ? 1 : 0);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".tuneNameDescRb.checked", tuneNameDescRb.Checked ? 1 : 0);
            ncUtils.DBSetup.singleton.setInt(this.Name + ".tuneNameCommRb.checked", tuneNameCommRb.Checked ? 1 : 0);
             */
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }

        /// <summary>
        /// Loads state of the control from DB
        /// </summary>
        private void loadState()
        {
            int ptypeId = ncUtils.DBSetup.singleton.getInt(this.Name + ".poiType.id", 1);
            Bookmarks.POIType ptype = Bookmarks.POITypeFactory.singleton().typeById(ptypeId);
            if (ptype != null)
                poiTypeComboBox.SelectedItem = ptype;
            needPOICB.Checked =
                (ncUtils.DBSetup.singleton.getInt(this.Name + ".needPOI.checked", 0) == 0 ? false : true);
            tuneDescCommCB.Checked =
                (ncUtils.DBSetup.singleton.getInt(this.Name + ".tuneDescComm.checked", 0) == 0 ? false : true);
            tuneNoChangesRb.Checked =
                (ncUtils.DBSetup.singleton.getInt(this.Name + ".tuneNoChangeRb.checked", 1) == 0 ? false : true);
            tuneNameDescRb.Checked =
                (ncUtils.DBSetup.singleton.getInt(this.Name + ".tuneNameDescRb.checked", 0) == 0 ? false : true);
            tuneNameCommRb.Checked =
                (ncUtils.DBSetup.singleton.getInt(this.Name + ".tuneNameCommRb.checked", 0) == 0 ? false : true);
        }

        /// <summary>
        /// Gets Checkbox that tell need we load POI at all or not
        /// </summary>
        public CheckBox NeedPOICB
        {
            get
            {
                return needPOICB;
            }
        }
    }
}
