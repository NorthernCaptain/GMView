using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class implements ITreeModel for filling TreeViewAdv widget
    /// </summary>
    public class POITreeModel: ITreeModel
    {
        private POIGroupFactory groupFact;

        public POITreeModel(POIGroupFactory igroupFact)
        {
            groupFact = igroupFact;
            rootList.Add(groupFact.findById(0)); //Adding root element
        }

        private List<POIGroup> rootList = new List<POIGroup>();

        #region ITreeModel Members

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if(treePath.IsEmpty())
            {
                return rootList;
            }

            POIGroup parent = treePath.LastNode as POIGroup;
            if(parent == null)
                return null;
            LinkedList<POIGroup> grplist = parent.Children;
            List<Bookmark> poilist = BookMarkFactory.singleton.loadByParent(parent.Id);
            if (grplist == null || grplist.Count == 0)
                return poilist;
            if (poilist == null || poilist.Count == 0)
                return grplist;

            List<object> all = new List<object>();
            foreach (POIGroup pg in grplist)
            {
                all.Add(pg);
            }
            foreach (Bookmark poi in poilist)
            {
                all.Add(poi);
            }
            return all;
        }

        public bool IsLeaf(TreePath treePath)
        {
            return !(treePath.LastNode is POIGroup);
        }

        /// <summary>
        /// Fires NodesInserted event that updates TreeView after insertion
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="inserted"></param>
        public void fireNodesInserted(TreePath parent, object[] inserted)
        {
            if(NodesInserted != null)
            {
                TreeModelEventArgs args = new TreeModelEventArgs(parent, inserted);
                NodesInserted(this, args);
            }
        }

        /// <summary>
        /// Fires NodesRemoved event that updates TreeView after removing some nodes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="removed"></param>
        public void fireNodesRemoved(TreePath parent, object[] removed)
        {
            if (NodesRemoved != null)
            {
                TreeModelEventArgs args = new TreeModelEventArgs(parent, removed);
                NodesRemoved(this, args);
            }
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion
    }
}
