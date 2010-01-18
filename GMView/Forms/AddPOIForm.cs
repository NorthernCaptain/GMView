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
    public partial class AddPOIForm : Form
    {
        private POIGroupFactory groupFactory;
        private BookMarkFactory poiFactory;

        private POIGroupTreeModel treeModel;
        private Bookmark currentPOI;

        public AddPOIForm(POIGroupFactory igroupFactory, BookMarkFactory ipoiFactory,
                        Bookmark ipoi)
        {
            poiFactory = ipoiFactory;
            groupFactory = igroupFactory;
            currentPOI = ipoi;

            InitializeComponent();

            treeModel = new POIGroupTreeModel(groupFactory);
            treeView.Model = treeModel;

            poiTypeListBox.loadList();
            poiTypeListBox.SelectedItem = currentPOI.Ptype;

            latTB.Text = currentPOI.LatitudeS;
            lonTb.Text = currentPOI.LongitudeS;
            altTB.Value = (decimal) currentPOI.altitude;
        }

        private void AddPOIForm_Load(object sender, EventArgs e)
        {
            IEnumerator<Aga.Controls.Tree.TreeNodeAdv> inum = treeView.AllNodes.GetEnumerator();
            inum.MoveNext();
            inum.Current.IsExpanded = true;
            inum.MoveNext();
            if(inum.Current == null)
            {
                inum.Reset();
                inum.MoveNext();
            }
            treeView.SelectedNode = inum.Current;            
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            if (nameTb.Text.Trim().Length == 0)
            {
                nameTb.Focus();
                return;
            }

            if (latTB.Text.Trim().Length == 0)
            {
                latTB.Focus();
                return;
            }

            if (lonTb.Text.Trim().Length == 0)
            {
                lonTb.Focus();
                return;
            }

            currentPOI.Name = nameTb.Text;
            currentPOI.Description = descrTb.Text;
            currentPOI.Comment = commentTB.Text;
            currentPOI.Ptype = (POIType) poiTypeListBox.SelectedItem;
            currentPOI.LongitudeS = lonTb.Text;
            currentPOI.LatitudeS = latTB.Text;
            currentPOI.altitude = (double)altTB.Value;
            currentPOI.updateDB();

            poiFactory.unregister(currentPOI);
            poiFactory.register(currentPOI);
            currentPOI.IsShown = true;
            POIGroup parentGrp = (treeView.SelectedNode != null) ? treeView.SelectedNode.Tag as POIGroup : null;
            if (parentGrp == null)
                parentGrp = groupFactory.findById(0);
            currentPOI.addLinkDB(parentGrp);
            parentGrp.addChild(currentPOI);

            this.DialogResult = DialogResult.OK;
        }
    }
}
