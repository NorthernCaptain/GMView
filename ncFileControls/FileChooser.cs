using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Aga.Controls.Tree;

namespace ncFileControls
{
    /// <summary>
    /// Class implements my custom file chooser control that can be used to build dialogs
    /// with additional widgets
    /// </summary>
    public partial class FileChooser : UserControl
    {
        private bool inInit = true;

        public FileChooser()
        {
            InitializeComponent();
            toolBox.AddTab("Common places");
            toolBox.AddTab("My places");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initTree();
        }
        /// <summary>
        /// Initialize tree and create main model for browsing directories
        /// </summary>
        private void initTree()
        {
            mainModel = new FilePlainTreeModel(treeView);
            SortedTreeModel smodel = new SortedTreeModel(mainModel);
            treeView.Model = smodel;
            smodel.Comparer = new FileGridSorter("Name", SortOrder.Ascending);

            TreeColumn[] cols = new TreeColumn[treeView.Columns.Count];
            foreach (TreeColumn col in treeView.Columns)
            {
                int idx = ncUtils.DBSetup.singleton.getInt(this.Name + ".tree.col." + col.Header + ".index",
                                                            col.Index);
                cols[idx] = col;
            }

            treeView.Columns.Clear();
            foreach (TreeColumn col in cols)
            {
                treeView.Columns.Add(col);
                col.Width = ncUtils.DBSetup.singleton.getInt(this.Name + ".tree.col." + col.Header + ".width",
                                                            col.Width);
            }
            
            
            inInit = false;
        }

        private FilePlainTreeModel mainModel;

        /// <summary>
        /// Current directory full path
        /// </summary>
        public string DirectoryPath
        {
            get { return mainModel.CurrentPath; }
            set { mainModel.CurrentPath = value; }
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                FileInfoNode node = treeView.SelectedNode.Tag as FileInfoNode;
                if (node != null && !node.IsFile)
                    mainModel.changeDir(node);
            }
        }

        private void treeView_ColumnClicked(object sender, TreeColumnEventArgs e)
        {
            SortedTreeModel model = treeView.Model as SortedTreeModel;
            TreeColumn col = e.Column;
            switch (col.SortOrder)
            {
                case SortOrder.Ascending:
                    col.SortOrder = SortOrder.Descending;
                    model.Comparer = new FileGridSorter(col.Header, col.SortOrder);
                    break;
                case SortOrder.Descending:
                    col.SortOrder = SortOrder.None;
                    model.Comparer = new FileGridSorter("Name", SortOrder.Ascending);
                    break;
                default:
                    col.SortOrder = SortOrder.Ascending;
                    model.Comparer = new FileGridSorter(col.Header, col.SortOrder);
                    break;
            }
        }

        private void treeView_ColumnReordered(object sender, TreeColumnEventArgs e)
        {
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            foreach (TreeColumn col in treeView.Columns)
            {
                ncUtils.DBSetup.singleton.setInt(this.Name + ".tree.col." + col.Header + ".index",
                                                 col.Index);
            }
            ncUtils.DBConnPool.singleton.commitThreadTransaction();
        }

        private void treeView_ColumnWidthChanged(object sender, TreeColumnEventArgs e)
        {
            if (inInit)
                return;

            ncUtils.DBSetup.singleton.setInt(this.Name + ".tree.col." + e.Column.Header + ".width",
                                             e.Column.Width);
        }
    }
}
