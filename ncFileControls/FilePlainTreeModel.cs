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
        /// Current Path, a full one. Gets or sets it
        /// </summary>
        public string CurrentPath
        {
            get { return currentPath; }
            set 
            {
                string newDir = value;
                if (newDir.Equals(currentPath))
                    return;
                currentPath = newDir;

                currentFileList = fillFileList(currentPath);
                if (StructureChanged != null)
                    StructureChanged(this, new TreePathEventArgs());

                if (DirectoryChanged != null)
                    DirectoryChanged(this, new EventArgs());
            }
        }

        private List<FileInfoNode> currentFileList = new List<FileInfoNode>();

        public FilePlainTreeModel(TreeViewAdv parentView)
        {
            this.parentView = parentView;
            currentFileList = fillFileList(currentPath);
        }

        /// <summary>
        /// Build file listing for current directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

            string uppath = Path.GetFullPath(Path.Combine(currentPath, ".."));
            FileInfoNode fn = new FileInfoNode(new DirectoryInfo(uppath));
            fn.Name = "..";
            dirList.Add(fn);

            foreach (string dir in Directory.GetDirectories(path))
            {
                FileInfoNode node = new FileInfoNode(new DirectoryInfo(dir));
                dirList.Add(node);
            }

            foreach (string fname in Directory.GetFiles(path, fileFilter))
            {
                FileInfoNode node = new FileInfoNode(new FileInfo(fname));
                dirList.Add(node);
            }
            return dirList;
        }

        /// <summary>
        /// Return last entry of the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getLastPathEntry(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            string[] contents = path.Split(Path.DirectorySeparatorChar);

            if (contents.Length < 1)
                return null;

            return contents[contents.Length - 1];
        }

        /// <summary>
        /// Change current directory to the given entry relative to our dir.
        /// </summary>
        /// <param name="dirEntry"></param>
        public string changeDir(FileInfoNode dirEntry)
        {
            string oldpath = currentPath;
            bool upperPath = dirEntry.Name == "..";
            string lastPath = getLastPathEntry(currentPath);

            currentPath = Path.Combine(currentPath, dirEntry.Name);
            currentPath = Path.GetFullPath(currentPath);

            if (oldpath == currentPath && dirEntry.Name == "..")
                currentPath = string.Empty;

            currentFileList = fillFileList(currentPath);
            if (StructureChanged != null)
                StructureChanged(this, new TreePathEventArgs());

            if (DirectoryChanged != null)
                DirectoryChanged(this, new EventArgs());
            return (upperPath ? lastPath : null);
        }

        /// <summary>
        /// Sets current directory to the given path
        /// </summary>
        /// <param name="newDir"></param>
        public void setDir(string newDir)
        {
            newDir = Path.GetFullPath(newDir);
            if (newDir.Equals(currentPath))
                return;
            currentPath = newDir;

            currentFileList = fillFileList(currentPath);
            if (StructureChanged != null)
                StructureChanged(this, new TreePathEventArgs());

            if (DirectoryChanged != null)
                DirectoryChanged(this, new EventArgs());
        }

        /// <summary>
        /// Roll over full path by its contents increasing incrementally. Return a list of such paths
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> getRolledDir(string path)
        {
            List<string> rolled = new List<string>();
            rolled.Add(string.Empty);

            if (string.IsNullOrEmpty(path))
            {
                return rolled;
            }

            path = Path.GetFullPath(path);
            string[] contents = path.Split(Path.DirectorySeparatorChar);

            rolled.Add(contents[0]);
            string pathpart = contents[0];
            for(int i = 1; i< contents.Length; i++)
            {
                if (String.IsNullOrEmpty(contents[i]))
                    continue;
                pathpart = Path.Combine(pathpart, contents[i]);
                rolled.Add(pathpart);
            }
            return rolled;
        }

        public event EventHandler<EventArgs> DirectoryChanged;

        private string fileFilter = "*";

        /// <summary>
        /// Gets or sets current file filter
        /// </summary>
        public string FileFilter
        {
            get { return fileFilter; }
            set { fileFilter = value; }
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
            return true;
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion
    }
}
