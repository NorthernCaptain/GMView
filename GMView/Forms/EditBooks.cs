using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using GMView.Bookmarks;

namespace GMView.Forms
{
    public partial class EditBooks : Form
    {
        private Bookmarks.POIGroupFactory groupFactory;
        private BookMarkFactory poiFactory;

        public EditBooks(BookMarkFactory ipoiFactory, Bookmarks.POIGroupFactory igroupFactory)
        {
            poiFactory = ipoiFactory;
            groupFactory = igroupFactory;
            InitializeComponent();
            fillGrid();
            fillTypeGrid();
        }

        private Bookmarks.POITreeModel treeModel;
        private void fillGrid()
        {
            treeModel = new Bookmarks.POITreeModel(groupFactory, treeView);
            treeView.Model = new SortedTreeModel(treeModel);
            SortedTreeModel model = treeView.Model as SortedTreeModel;
            model.Comparer = new Bookmarks.POIGridSorter("Name", SortOrder.Ascending);
            nodeCombo_Type.DropDownItems.AddRange(Bookmarks.POITypeFactory.singleton().items.ToArray());
            nodeCombo_Type.CreatingEditor += new EventHandler<Aga.Controls.Tree.NodeControls.EditEventArgs>(nodeCombo_Type_CreatingEditor);
            nodeDescr.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText);
            nodeName.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText);
            nodeTextBox_Comment.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText);
            nodeTextBox_Lat.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText);
            nodeTextBox_Lon.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText);

            IEnumerator<TreeNodeAdv> ienum = treeView.AllNodes.GetEnumerator();
            ienum.MoveNext();
            ienum.Current.IsExpanded = true;
        }

        private Bookmarks.POITypeTreeModel typeTreeModel;
        private void fillTypeGrid()
        {
            typeTreeModel = new Bookmarks.POITypeTreeModel(Bookmarks.POITypeFactory.singleton(),
                                                           treeViewPType);
            treeViewPType.Model = new SortedTreeModel(typeTreeModel);
            PTnodeTextBoxName.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText);
            PTnodeTextBoxDesc.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(nodeDescr_DrawText); 
        }

        void nodeDescr_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawEventArgs e)
        {
            e.Font = treeView.Font;
        }

        void nodeCombo_Type_CreatingEditor(object sender, Aga.Controls.Tree.NodeControls.EditEventArgs e)
        {
            ComboBox cbox = e.Control as ComboBox;
            if (cbox == null)
                return;
            cbox.Font = treeView.Font;
            cbox.DropDownWidth = 150;
            cbox.DropDownHeight = 250;
            cbox.DrawMode = DrawMode.OwnerDrawFixed;
            cbox.DrawItem += new DrawItemEventHandler(cbox_DrawItem);
        }

        protected int delta_x = 32;
        protected int heightItem = 40;
        protected int delta_y = -1;

        /// <summary>
        /// Draw combobox dropdown list with poi type icons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbox = sender as ComboBox;
            e.DrawBackground();

            if (e.Index < 0)
                return;

            Bookmarks.POIType pi = cbox.Items[e.Index] as Bookmarks.POIType;

            if (delta_y == -1)
            {
                delta_y = (int)(heightItem - e.Font.GetHeight(e.Graphics)) / 2;
            }

            if (pi != null)
            {
                ImageDot dot = GMView.IconFactory.singleton.getIcon(pi);
                if (dot != null)
                    e.Graphics.DrawImage(dot.img, e.Bounds.Left + (delta_x - dot.real_len) / 2,
                        e.Bounds.Top + (heightItem - dot.real_hei) / 2, dot.real_len, dot.real_hei);
            }

            e.Graphics.DrawString(cbox.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor),
                e.Bounds.Left + delta_x, e.Bounds.Top + delta_y);

            if (cbox.ItemHeight != heightItem)
                cbox.ItemHeight = heightItem;

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// When we click on column we set sorter for it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_ColumnClicked(object sender, TreeColumnEventArgs e)
        {
            SortedTreeModel model = treeView.Model as SortedTreeModel;
            TreeColumn col = e.Column;
            switch(col.SortOrder)
            {
                case SortOrder.Ascending:
                    col.SortOrder = SortOrder.Descending;
                    model.Comparer = new Bookmarks.POIGridSorter(col.Header, col.SortOrder);
                    break;
                case SortOrder.Descending:
                    col.SortOrder = SortOrder.None;
                    model.Comparer = new Bookmarks.POIGridSorter("Name", SortOrder.Ascending);
                    break;
                default:
                    col.SortOrder = SortOrder.Ascending;
                    model.Comparer = new Bookmarks.POIGridSorter(col.Header, col.SortOrder);
                    break;
            }
            
        }

        private void treeView_SelectionChanged(object sender, EventArgs e)
        {
            if (treeView.SelectedNodes.Count == 0)
                addGroupToolStripMenuItem.Enabled = false;
            else
                addGroupToolStripMenuItem.Enabled = !treeView.SelectedNode.IsLeaf;
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeModel.addNewGroup();
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeView.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && treeView.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
                TreeNodeAdv parent = treeView.DropPosition.Node;
                if (treeView.DropPosition.Position != NodePosition.Inside)
                    parent = parent.Parent;
                if(parent.Parent == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                foreach (TreeNodeAdv node in nodes)
                    if (!CheckNodeParent(parent, node))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                e.Effect = e.AllowedEffect;
            }
        }

        private bool CheckNodeParent(TreeNodeAdv parent, TreeNodeAdv node)
        {
            while (parent != null)
            {
                if (node == parent)
                    return false;
                else
                    parent = parent.Parent;
            }
            return true;
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            treeModel.doDropProcessing(e);
        }

        private void deletePOIOrGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to delete POI. Deleted POI will be lost completely.\nAre you sure?",
                "Delete POI?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != 
                       DialogResult.Yes)
            return;

            treeModel.deleteSelectedNodes();
        }

        private void changeTypeOfPOIsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.SelectPOITypeForm ptForm = new Forms.SelectPOITypeForm();
            ptForm.Owner = this;

            foreach (TreeNodeAdv node in treeView.SelectedNodes)
            {
                Bookmark firstPOI = node.Tag as Bookmark;
                if (firstPOI != null)
                {
                    ptForm.result = firstPOI.Ptype;
                    break;
                }
            }

            if (ptForm.ShowDialog() != DialogResult.OK)
            {
                ptForm.Dispose();
                return;
            }

            Bookmarks.POIType newType = ptForm.result;
            if(newType == null)
                return;

            foreach (TreeNodeAdv node in treeView.SelectedNodes)
            {
                Bookmark poi = node.Tag as Bookmark;
                if (poi != null)
                {
                    poi.Ptype = newType;
                }
            }
            treeView.Invalidate();
        }

        
        
        private void okBut_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private POIType currentPT;

        private void treeViewPType_SelectionChanged(object sender, EventArgs e)
        {
            setTypeInfo((treeViewPType.SelectedNode ==  null ?  null : treeViewPType.SelectedNode.Tag as POIType));
            PTapplyBut.Enabled = true;
        }

        private void setTypeInfo(POIType ptype)
        {
            currentPT = ptype;
            if (ptype != null)
            {
                PTnameTB.Text = currentPT.Name;
                PTdescrTB.Text = currentPT.Text;
                PTminZoomLvlNum.Value = (decimal)currentPT.MinZoomLvl;
                PTautoShowCB.Checked = currentPT.IsAutoShow;
                PTquickAddCB.Checked = currentPT.IsQuickType;
                PTiconCXNum.Value = (decimal)currentPT.iconDeltaX;
                PTiconCYNum.Value = (decimal)currentPT.iconDeltaY;
                if (ptype.Id != 0)
                {
                    pt_delta_x = currentPT.iconDeltaX;
                    pt_delta_y = currentPT.iconDeltaY;
                }
                PTiconImage = currentPT.IconImg;
                PTiconString = currentPT.iconName;
            }
            else
            {
                PTnameTB.Text = string.Empty;
                PTdescrTB.Text = string.Empty;
                PTautoShowCB.Checked = false;
                PTquickAddCB.Checked = false;
                PTiconImage = null;
                PTiconString = String.Empty;
            }
            PTiconPic.Invalidate();
        }

        private int pt_delta_x;
        private int pt_delta_y;

        private void PTiconPic_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Brushes.White, 1);
            Pen linepen = new Pen(Brushes.DarkBlue, 1);

            e.Graphics.DrawRectangle(pen, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
            if (PTiconImage != null)
            {
                e.Graphics.DrawImage(PTiconImage, 0, 0, PTiconImage.Width, PTiconImage.Height);
                e.Graphics.DrawLine(pen, pt_delta_x - 2, pt_delta_y +1, pt_delta_x + 4, pt_delta_y +1);
                e.Graphics.DrawLine(pen, pt_delta_x +1, pt_delta_y - 2, pt_delta_x +1, pt_delta_y + 4);
                e.Graphics.DrawLine(linepen, pt_delta_x - 3, pt_delta_y, pt_delta_x + 3, pt_delta_y);
                e.Graphics.DrawLine(linepen, pt_delta_x, pt_delta_y - 3, pt_delta_x, pt_delta_y + 3);
            }
        }

        private void PTiconPic_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentPT == null)
                return;
            ImageDot idot = IconFactory.singleton.getIcon(currentPT);
            if (e.Button != MouseButtons.Left)
                return;

            if (e.X >= idot.real_len || e.Y >= idot.real_hei)
                return;

            pt_delta_x = e.X;
            pt_delta_y = e.Y;

            PTiconPic.Invalidate();
            PTiconCXNum.Value = (decimal)e.X;
            PTiconCYNum.Value = (decimal)e.Y;
        }

        private void cancelChangesBut_Click(object sender, EventArgs e)
        {
            treeViewPType_SelectionChanged(sender, e);
        }

        private void newTypeBut_Click(object sender, EventArgs e)
        {
            currentPT = new POIType();
            setTypeInfo(currentPT);
            PTapplyBut.Enabled = true;
            PTnameTB.Focus();
        }

        private void PTapplyBut_Click(object sender, EventArgs e)
        {
            if(currentPT == null)
                return;

            if(PTnameTB.Text.Trim().Length == 0)
            {
                PTnameTB.Focus();
                return;
            }
            if(PTdescrTB.Text.Trim().Length == 0)
            {
                PTdescrTB.Focus();
                return;
            }

            bool newType = currentPT.Id == 0;
            currentPT.iconName = PTiconString;
            currentPT.iconDeltaY = pt_delta_y;
            currentPT.iconDeltaX = pt_delta_x;

            currentPT.Name = PTnameTB.Text.Trim();
            currentPT.Text = PTdescrTB.Text.Trim();

            currentPT.IsQuickType = PTquickAddCB.Checked;
            currentPT.IsAutoShow = PTautoShowCB.Checked;
            currentPT.MinZoomLvl = (int)PTminZoomLvlNum.Value;

            PTapplyBut.Enabled = false;

            if (newType)
            {
                POITypeFactory.singleton().registerType(currentPT);
                POITypeFactory.singleton().resortAll();
                typeTreeModel.fireStructureChanged();
            }
        }

        private string PTiconString;
        private Image PTiconImage;

        private void ChangeIconBut_Click(object sender, EventArgs e)
        {
            PTIconOpenDialog.InitialDirectory = Program.opt.iconSetPath;
            if(PTIconOpenDialog.ShowDialog() == DialogResult.OK)
            {
                string iconString = System.IO.Path.GetFileName(PTIconOpenDialog.FileName);
                try
                {
                    Image iconImage = new Bitmap(System.IO.Path.Combine(Program.opt.iconSetPath, iconString));
                    if (iconImage.Width > 48 || iconImage.Height > 48)
                    {
                        MessageBox.Show("Icon size should be less or equal to 48x48 pixels.\nPlease, choose another icon",
                            "Wrong Icon size", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
                    }
                    else
                    {
                        PTiconString = iconString;
                        PTiconImage = iconImage;
                        PTiconPic.Invalidate();

                        if(PTnameTB.Text.Trim().Length == 0)
                        {
                            PTnameTB.Text = System.IO.Path.GetFileNameWithoutExtension(iconString).ToLower();
                        }
                        if(PTdescrTB.Text.Trim().Length == 0)
                        {
                            string desc = System.IO.Path.GetFileNameWithoutExtension(iconString).ToLower();
                            desc = char.ToUpper(desc[0]) + desc.Substring(1);
                            PTdescrTB.Text = desc;
                        }
                    }
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Could not load image for the icon.\nFile path: " +
                    System.IO.Path.Combine(Program.opt.iconSetPath, PTiconString), "Error loading image",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PTiconCXNum_ValueChanged(object sender, EventArgs e)
        {
            pt_delta_x = (int)PTiconCXNum.Value;
            pt_delta_y = (int)PTiconCYNum.Value;
            PTiconPic.Invalidate();
        }

        private void PTdelTypeBut_Click(object sender, EventArgs e)
        {
            if(currentPT != null && currentPT.Id != 0)
            {
                if(!currentPT.isFlag(POIType.FlagUserEntry))
                {
                    MessageBox.Show("You could not delete system types.\nOnly types created by yourself could be deleted",
                        "Could not delete type", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if(MessageBox.Show("You are about to delete POI type: " + currentPT.Name +
                    "\nAre you sure?", "Delete POI type", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    POITypeFactory.singleton().unregisterType(currentPT);
                    currentPT.deleteFromDB();
                    typeTreeModel.fireStructureChanged();
                    setTypeInfo(null);
                }
            }
        }

    }
}
