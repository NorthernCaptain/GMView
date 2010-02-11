using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ncFileControls
{
    /// <summary>
    /// Class that sorts the file chooser grid by different columns
    /// </summary>
    class FileGridSorter: IComparer
    {
        private string field;
        private SortOrder order;


        public FileGridSorter(string ifield, SortOrder iorder)
        {
            field = ifield;
            order = iorder;
        }

        private int compareByName(FileInfoNode node1, FileInfoNode node2)
        {
            int ret = node1.Name.CompareTo(node2.Name);

            if (ret == 0)
                return ret;

            if (node1.Name == "..")
                return -1;
            if (node2.Name == "..")
                return 1;
            if (order == SortOrder.Ascending)
                return ret;
            else
                return -ret;
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            FileInfoNode node1 = x as FileInfoNode;
            FileInfoNode node2 = y as FileInfoNode;

            if (node1 == null || node2 == null)
                return 0;

            if (node1.IsFile != node2.IsFile)
            {
                return (node1.IsFile ? 1 : -1);
            }

            int ret = 0;
            switch (field)
            {
                case "Size":
                    ret = node1.Size.CompareTo(node2.Size);
                    break;
                case "Modified":
                    ret = node1.Modified.CompareTo(node2.Modified);
                    break;
                default:
                    return compareByName(node1, node2);
            }

            if (ret == 0)
                return compareByName(node1, node2);

            return (order == SortOrder.Ascending ? ret : -ret);
        }

        #endregion
    }
}
