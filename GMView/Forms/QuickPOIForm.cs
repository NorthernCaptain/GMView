using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GMView.Bookmarks;

namespace GMView.Forms
{
    /// <summary>
    /// Form for quick POI creation during driving
    /// </summary>
    public partial class QuickPOIForm : Form
    {
        private POIType currentType = null;
        private Bookmark mypoi;

        /// <summary>
        /// Gets current POI type selected by user or null
        /// </summary>
        public POIType poiType
        {
            get
            {
                return currentType;
            }
        }

        public QuickPOIForm(Bookmark ipoi)
        {
            mypoi = ipoi;
            InitializeComponent();
            poiTypeList.BeginUpdate();
            poiTypeList.loadList();
            poiTypeList.EndUpdate();
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            currentType = poiTypeList.SelectedItem as POIType;
            mypoi.qchangeType(poiType);
            mypoi.updateDB();
            Bookmarks.POIGroup pgroup = Bookmarks.POIGroupFactory.singleton().findByName("quick add");
            if (pgroup == null)
                pgroup = Bookmarks.POIGroupFactory.singleton().findById(0);
            mypoi.addLinkDB(pgroup);
            pgroup.addChild(mypoi);
            BookMarkFactory.singleton.unregister(mypoi);
            BookMarkFactory.singleton.register(mypoi);
        }
    }
}
