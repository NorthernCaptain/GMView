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
        public EditBooks()
        {
            InitializeComponent();
            fillGrid();
        }

        private void fillGrid()
        {
            treeView.Model = new SortedTreeModel(new Bookmarks.POITreeModel(Bookmarks.POIGroupFactory.singleton()));
            SortedTreeModel model = treeView.Model as SortedTreeModel;
            model.Comparer = new Bookmarks.POIGridSorter("Name", SortOrder.Ascending);
            nodeCombo_Type.DropDownItems.AddRange(Bookmarks.POITypeFactory.singleton().items.ToArray());
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
    }
}
