using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace ncFileControls
{
    /// <summary>
    /// Class holds information about file
    /// </summary>
    public class FileInfoNode
    {
        private string name;
        /// <summary>
        /// Name of the file. Not a full name, just name of the file without path, but with extension
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Shows the display name of the file or directory entry
        /// </summary>
        public string DisplayName
        {
            get
            {
                return (isFile ? name : "[" + name + "]");
            }
            set
            {
                string newname = value.Replace('[', ' ').Replace(']', ' ').Trim();
                name = newname;
            }
        }

        /// <summary>
        /// Return Icon of this type of file (dir only)
        /// </summary>
        public Bitmap IconImg
        {
            get
            {
                if (!isFile)
                {
                    if (name == "..")
                        return Properties.Resources.updir2;

                    return Properties.Resources.dir1;
                }

                return null;
            }
        }

        private long size;
        /// <summary>
        /// File size in bytes
        /// </summary>
        public long Size
        {
            get { return size; }
            set { size = value; }
        }
        private DateTime modified;

        /// <summary>
        /// Date of last file modification
        /// </summary>
        public DateTime Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        public string DateS
        {
            get
            {
                return modified.ToShortDateString() + " " + modified.ToShortTimeString();
            }
        }

        private string extension;
        /// <summary>
        /// Extension of the file
        /// </summary>
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        /// <summary>
        /// String representation of the file size
        /// </summary>
        public string SizeS
        {
            get
            {
                if (!isFile && size == 0)
                    return "<DIR>";
                if (size > 1073741824) //GB
                    return ((double)size / 1073741824.0).ToString("F1") + "GB";
                if (size > 1048576) //1024*1024
                    return ((double)size / 1048576.0).ToString("F1") + "MB";
                if (size > 1024)
                    return ((double)size / 1024.0).ToString("F1") + " KB";
                return size.ToString() + " b";
            }
        }

        private bool isFile = true;

        /// <summary>
        /// Is this entry an ordinary file or other type
        /// </summary>
        public bool IsFile
        {
            get { return isFile; }
            set { isFile = value; }
        }

        /// <summary>
        /// Constructor from FileSystemInfo
        /// </summary>
        /// <param name="finfo"></param>
        public FileInfoNode(FileSystemInfo finfo)
        {
            FileInfo file = finfo as FileInfo;
            if (file != null)
            {
                fillFromFile(file);
                return;
            }

            DirectoryInfo dir = finfo as DirectoryInfo;
            if (dir != null)
            {
                fillFromDir(dir);
                return;
            }
        }

        public FileInfoNode(DriveInfo finfo)
        {
            fillDriveInfo(finfo);
        }


        /// <summary>
        /// Fill info from IO.FileInfo
        /// </summary>
        /// <param name="finfo"></param>
        private void fillFromFile(FileInfo finfo)
        {
            name = finfo.Name;
            isFile = true;
            size = finfo.Length;
            modified = finfo.LastWriteTime;
            extension = finfo.Extension;
        }


        /// <summary>
        /// Fill info from IO.DirectoryInfo
        /// </summary>
        /// <param name="finfo"></param>
        private void fillFromDir(DirectoryInfo finfo)
        {
            name = finfo.Name;
            isFile = false;
            size = 0;
            modified = finfo.LastWriteTime;
            extension = string.Empty;
        }


        /// <summary>
        /// Fill info from IO.DriveInfo
        /// </summary>
        /// <param name="finfo"></param>
        private void fillDriveInfo(DriveInfo finfo)
        {
            name = finfo.Name;
            isFile = false;
            try
            {
                size = finfo.TotalFreeSpace;
            }
            catch (System.Exception)
            {
            	
            }
            modified = DateTime.Now;
            extension = string.Empty;
        }
    }
}
