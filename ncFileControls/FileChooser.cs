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

        internal class MyPlace : IDirBookmark
        {
            string dirName;
            string name;

            internal MyPlace(string iname, string idir)
            {
                name = iname;
                dirName = idir;
            }

            #region IDirBookmark Members

            public string directory
            {
                get { return dirName; }
            }

            #endregion
        }

        private List<FileFilter> fileFilters = new List<FileFilter>();

        /// <summary>
        /// Gets a list of file filters used in File chooser
        /// </summary>
        public List<FileFilter> FileFilters
        {
            get { return fileFilters; }
        }

        /// <summary>
        /// Adds new file filter to the list of filters to use.
        /// </summary>
        /// <param name="filter"></param>
        public void addFileFilter(FileFilter filter)
        {
            fileFilters.Add(filter);

            fileTypeCB.Items.Clear();
            fileTypeCB.Items.AddRange(fileFilters.ToArray());
            fileTypeCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Gets or sets current file filter
        /// </summary>
        public FileFilter currentFilter
        {
            get
            {
                return mainModel.FileFilter;
            }

            set
            {
                foreach (FileFilter flt in fileFilters)
                {
                    if (flt.Filter.Equals(value.Filter))
                    {
                        mainModel.FileFilter = flt;
                        fileTypeCB.SelectedItem = flt;
                        return;
                    }
                }
                addFileFilter(value);

                fileTypeCB.SelectedItem = value;
                mainModel.FileFilter = value;
            }
        }

        public FileChooser()
        {
            InitializeComponent();
            mainModel = new FilePlainTreeModel(treeView);
            mainModel.DirectoryChanged += mainModel_DirectoryChanged;
            initToolbox();
            addFileFilter(new FileFilter("All files (*.*)", "*.*"));
        }

        /// <summary>
        /// Event raises when directory was changed. Refill our widgets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mainModel_DirectoryChanged(object sender, EventArgs e)
        {
            List<string> contents = mainModel.getRolledDir(mainModel.CurrentPath);
            dirCB.Items.Clear();
            dirCB.Items.AddRange(contents.ToArray());
            dirCB.SelectedIndex = contents.Count - 1;

            fileCB.Items.Clear();
            fileCB.SelectedText = String.Empty;
            toolBox.SelectedTab.SelectedItemIndex = -1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initTree();

            for (int i = 0; i < 4; i++)
            {
                string pname = ncUtils.DBSetup.singleton.getString(this.Name + ".toolbox.myplace.name." + i.ToString(),
                    null);
                if (pname == null)
                    continue;

                string pdir = ncUtils.DBSetup.singleton.getString(this.Name + ".toolbox.myplace.dir." + i.ToString(),
                    null);
                toolBox[1].AddItem(pname, 3, 3, false, new MyPlace(pname, pdir));
            }

            toolBox[1].View = Silver.UI.ViewMode.LargeIcons;
            toolBox[1].ShowOnlyOneItemPerRow = true;

            toolBox.SelectedTabIndex = 1;
            toolBox.SelectedTab.SelectedItemIndex = -1;
            toolBox.SelectedTabIndex = 0;
            toolBox.SelectedTab.SelectedItemIndex = -1;
        }
        /// <summary>
        /// Initialize tree and create main model for browsing directories
        /// </summary>
        private void initTree()
        {
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

        /// <summary>
        /// Initializes toolbox with common places
        /// </summary>
        private void initToolbox()
        {
            toolBox.Size = new Size(105, toolBox.Size.Height);
            toolBox.AddTab("Common places");
            toolBox.AddTab("My places");

            ImageList largeImlist = new ImageList();
            toolBox.LargeImageList = largeImlist;
            largeImlist.Images.Add(ncFileControls.Properties.Resources.folder_home2);
            largeImlist.Images.Add(ncFileControls.Properties.Resources.document);
            largeImlist.Images.Add(ncFileControls.Properties.Resources.desktop);
            largeImlist.Images.Add(ncFileControls.Properties.Resources.folder_html);
            largeImlist.ImageSize = new System.Drawing.Size(64, 64);
            largeImlist.ColorDepth = ColorDepth.Depth32Bit;

            toolBox[0].AddItem("My Computer", 0, 0, false, new CommonDirs.MyComputerBook());
            toolBox[0].AddItem("My Documents", 1, 1, false, new CommonDirs.MyDocumentsBook());
            toolBox[0].AddItem("Desktop", 2, 2, false, new CommonDirs.MyDesktopBook());
            toolBox[0].View = Silver.UI.ViewMode.LargeIcons;
            toolBox[0].ShowOnlyOneItemPerRow = true;
        }

        private FilePlainTreeModel mainModel;

        /// <summary>
        /// Current directory full path
        /// </summary>
        public string DirectoryPath
        {
            get { return mainModel.CurrentPath; }
            set { setCurrentDir(value); }
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                FileInfoNode node = treeView.SelectedNode.Tag as FileInfoNode;
                if (node != null && !node.IsFile)
                {
                    changeCurrentDir(node);
                }
            }
        }

        /// <summary>
        /// Changes current dir to the given sub-entry or upper dir
        /// </summary>
        /// <param name="node"></param>
        private void changeCurrentDir(FileInfoNode node)
        {
            string lastEntry = mainModel.changeDir(node);
            if (!string.IsNullOrEmpty(lastEntry))
            {
                foreach (TreeNodeAdv tnode in treeView.AllNodes)
                {
                    FileInfoNode nfo = tnode.Tag as FileInfoNode;
                    if (nfo.Name.Equals(lastEntry))
                    {
                        treeView.SelectedNode = tnode;
                        break;
                    }
                }
            }
            else
            {
                IEnumerator<TreeNodeAdv> allnodes = treeView.AllNodes.GetEnumerator();
                allnodes.MoveNext();
                treeView.SelectedNode = allnodes.Current;
            }
        }

        /// <summary>
        /// Set current directory to the given path
        /// </summary>
        /// <param name="dir"></param>
        private void setCurrentDir(string dir)
        {
            mainModel.setDir(dir);
            IEnumerator<TreeNodeAdv> allnodes = treeView.AllNodes.GetEnumerator();
            allnodes.MoveNext();
            treeView.SelectedNode = allnodes.Current;
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

        private void dirCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mainModel.CurrentPath = dirCB.SelectedItem.ToString();
        }

        /// <summary>
        /// Raises when selection is changed in the tree view. Sets the current file and fires
        /// fileSelectionChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectionChanged(object sender, EventArgs e)
        {
            fileCB.Text = null;
            currentFileInfo = null;

            if (treeView.SelectedNode != null)
            {
                FileInfoNode nfo = treeView.SelectedNode.Tag as FileInfoNode;
                if (nfo != null && nfo.IsFile)
                {
                    fileCB.Text = nfo.Name;
                    currentFileInfo = nfo;
                }
            }

            if (fileSelectionChanged != null)
                fileSelectionChanged(this, null);
        }

        private FileInfoNode currentFileInfo = null;
        /// <summary>
        /// Event that fires if current selected file changed to another one.
        /// </summary>
        public event EventHandler fileSelectionChanged;

        /// <summary>
        /// Gets the current selected file with a full path to it.
        /// </summary>
        public string SelectedFile
        {
            get
            {
                if (string.IsNullOrEmpty(fileCB.Text))
                    return null;
                currentFileInfo = new FileInfoNode(fileCB.Text);
                return Path.Combine(mainModel.CurrentPath, currentFileInfo.Name);
            }
            set
            {
                if (value == null)
                {
                    setCurrentDir(string.Empty);
                    return;
                }

                string path = Path.GetDirectoryName(value);
                string file = Path.GetFileName(value);

                setCurrentDir(path);
                FileInfoNode node = mainModel.findFileEntry(file);
                if (node == null)
                {
                    currentFileInfo = new FileInfoNode(file);
                    currentFileInfo.ParentPath = mainModel;
                }
                else
                {
                    currentFileInfo = node;
                }

                fileCB.Text = file;

                foreach (TreeNodeAdv tnode in treeView.AllNodes)
                {
                    if (tnode.Tag == node)
                    {
                        treeView.SelectedNode = tnode;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Is some file selected or not. If selected then use SelectedFile to retrieve full file name.
        /// </summary>
        public bool isSelected
        {
            get { return currentFileInfo != null; }
        }

        /// <summary>
        /// Adds new place bookmark into the toolbox
        /// </summary>
        /// <param name="name"></param>
        /// <param name="img"></param>
        /// <param name="place"></param>
        public void addCommonPlace(string name, Bitmap img, IDirBookmark place)
        {
            int idx = toolBox.LargeImageList.Images.Count;
            toolBox.LargeImageList.Images.Add(img);
            int selectedIdx = toolBox.SelectedTab.SelectedItemIndex;
            toolBox[0].AddItem(name, idx, idx, false, place);
            toolBox.SelectedTab.SelectedItemIndex = selectedIdx;
        }

        /// <summary>
        /// Process key press events inside tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                treeView_DoubleClick(sender, e);
                e.Handled = true;
            }
        }

        private void upDirBut_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(mainModel.CurrentPath))
                changeCurrentDir(new FileInfoNode(".."));
        }

        private void toolBox_ItemSelectionChanged(Silver.UI.ToolBoxItem sender, EventArgs e)
        {
            IDirBookmark dirBook = sender.Object as IDirBookmark;
            if (dirBook != null)
                setCurrentDir(dirBook.directory);
        }

        private void fileCB_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void newDirBut_Click(object sender, EventArgs e)
        {
            AskNameForm dirnameDlg = new AskNameForm("Create new directory", "Enter new folder name:");
            dirnameDlg.SelectedText = "New folder";
            dirnameDlg.StartPosition = FormStartPosition.CenterScreen;

            if (dirnameDlg.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void bookDirBut_Click(object sender, EventArgs e)
        {
            AskNameForm dirnameDlg = new AskNameForm("Add 'My place' bookmark", "Enter new bookmark name:");
            string text = FilePlainTreeModel.getLastPathEntry(mainModel.CurrentPath);
            if (!string.IsNullOrEmpty(text))
                text = text.ToUpper()[0] + text.Substring(1);

            dirnameDlg.SelectedText = text;
            dirnameDlg.StartPosition = FormStartPosition.CenterScreen;

            if (dirnameDlg.ShowDialog() == DialogResult.OK)
            {
                int idx = toolBox[1].ItemCount;

                toolBox[1].AddItem(dirnameDlg.SelectedText, 3, 3, false,
                    new MyPlace(dirnameDlg.SelectedText, mainModel.CurrentPath));

                ncUtils.DBSetup.singleton.setString(this.Name + ".toolbox.myplace.name." + idx.ToString(),
                                                    dirnameDlg.SelectedText);
                ncUtils.DBSetup.singleton.setString(this.Name + ".toolbox.myplace.dir." + idx.ToString(),
                                                    mainModel.CurrentPath);
            }
        }

        private void fileTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainModel.FileFilter = fileTypeCB.SelectedItem as FileFilter;
            mainModel.refresh();
        }

    }
}
