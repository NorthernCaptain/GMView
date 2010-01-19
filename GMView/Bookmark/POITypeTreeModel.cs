using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls.Tree;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Tree model for displaying POI types in the TreeViewAdv
    /// </summary>
    public class POITypeTreeModel: ITreeModel
    {
        private POITypeFactory factory;
        private TreeViewAdv treeView;

        public POITypeTreeModel(POITypeFactory ifactory, TreeViewAdv itree)
        {
            factory = ifactory;
            treeView = itree;
        }

        #region ITreeModel Members

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            return factory.items;
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
