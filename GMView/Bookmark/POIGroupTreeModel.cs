using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Tree model for POI groups without POI itself
    /// </summary>
    public class POIGroupTreeModel : ITreeModel
    {
        private POIGroupFactory groupFact;

        public POIGroupTreeModel(POIGroupFactory igroupFact)
        {
            groupFact = igroupFact;
            rootList.Add(groupFact.findById(0)); //Adding root element
        }

        private List<POIGroup> rootList = new List<POIGroup>();
        #region ITreeModel Members

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                return rootList;
            }

            POIGroup parent = treePath.LastNode as POIGroup;
            if (parent == null)
                return null;
            return parent.Children;
        }

        public bool IsLeaf(TreePath treePath)
        {
            return !(treePath.LastNode is POIGroup);
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion
    }
}
