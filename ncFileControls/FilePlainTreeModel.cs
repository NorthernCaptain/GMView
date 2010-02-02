using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Aga.Controls.Tree;

namespace ncFileControls
{
    /// <summary>
    /// Class provides model for plain directory display
    /// </summary>
    class FilePlainTreeModel: ITreeModel
    {
        private TreeViewAdv parentView;

        private string currentPath = string.Empty;

        /// <summary>
        /// Current Path, a full one
        /// </summary>
        public string CurrentPath
        {
            get { return currentPath; }
            set { currentPath = value; }
        }

        private List<FileInfoNode> currentFileList = new List<FileInfoNode>();

        public FilePlainTreeModel(TreeViewAdv parentView)
        {
            this.parentView = parentView;
            currentFileList = fillFileList(currentPath);
        }

        private List<FileInfoNode> fillFileList(string path)
        {
            List<FileInfoNode> dirList = new List<FileInfoNode>();

            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                foreach (string dir in Environment.GetLogicalDrives())
                {
                    FileInfoNode node = new FileInfoNode(new DriveInfo(dir));
                    dirList.Add(node);
                }
                return dirList;
            }

            foreach (string dir in Directory.GetDirectories(path))
            {
                FileInfoNode node = new FileInfoNode(new DirectoryInfo(dir));
                dirList.Add(node);
            }

            foreach (string fname in Directory.GetFiles(path))
            {
                FileInfoNode node = new FileInfoNode(new FileInfo(fname));
                dirList.Add(node);
            }
            return dirList;
        }

        #region ITreeModel Members

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if(treePath.IsEmpty())
                return currentFileList;
            currentPath = Path.Combine(currentPath, (treePath.LastNode as FileInfoNode).Name);
            currentFileList = fillFileList(currentPath);
            return currentFileList;
        }

        public bool IsLeaf(TreePath treePath)
        {
            return (treePath.LastNode as FileInfoNode).IsFile;
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion
    }
}
