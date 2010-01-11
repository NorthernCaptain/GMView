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
        }
    }
}
