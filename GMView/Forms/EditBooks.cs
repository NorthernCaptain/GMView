using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;

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

        private void okBut_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

    }
}
