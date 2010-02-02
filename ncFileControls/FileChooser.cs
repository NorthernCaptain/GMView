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
        public FileChooser()
        {
            InitializeComponent();
            toolBox.AddTab("Common places");
            toolBox.AddTab("My places");

            initTree();
        }

        /// <summary>
        /// Initialize tree and create main model for browsing directories
        /// </summary>
        private void initTree()
        {
            mainModel = new FilePlainTreeModel(treeView);
            treeView.Model = new SortedTreeModel(mainModel);
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
    }
}
