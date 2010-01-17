using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;
using System.Windows.Forms;

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

        /// <summary>
        /// Process Drop operation for dragged nodes in the tree
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="e"></param>
        public void doDropProcessing(TreeViewAdv treeView, DragEventArgs e)
        {
            treeView.BeginUpdate();

            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            Bookmarks.POIGroup dropNode = treeView.DropPosition.Node.Tag as Bookmarks.POIGroup;

            List<object> removed = new List<object>();

            if (treeView.DropPosition.Position == NodePosition.Inside)
            {
                if (dropNode == null)
                    return;

                foreach (TreeNodeAdv n in nodes)
                {
                    if (n.Tag is Bookmarks.IPOIBase)
                    {
                        Bookmarks.IPOIBase pg = n.Tag as Bookmarks.IPOIBase;
                        pg.reparentMeTo(dropNode, null);

                        removed.Add(n);
                    }
                }
                treeView.DropPosition.Node.IsExpanded = true;
                object[] arr = removed.ToArray();
                fireNodesRemoved(treeView.GetPath(nodes[0].Parent), arr);
                fireNodesInserted(treeView.GetPath(treeView.DropPosition.Node), arr);
            }
            else
            {
                Bookmarks.IPOIBase parent;
                if (dropNode == null)
                {
                    dropNode = treeView.DropPosition.Node.Parent.Tag as Bookmarks.POIGroup;
                    if (dropNode == null)
                        return;
                    parent = dropNode;
                    dropNode = null;
                }
                else
                    parent = dropNode.Parent;

                foreach (TreeNodeAdv n in nodes)
                {
                    if (n.Tag is Bookmarks.IPOIBase)
                    {
                        Bookmarks.IPOIBase pg = n.Tag as Bookmarks.IPOIBase;
                        pg.reparentMeTo(parent, dropNode);

                        removed.Add(n);
                    }
                }
                treeView.DropPosition.Node.Parent.IsExpanded = true;
                object[] arr = removed.ToArray();
                fireNodesRemoved(treeView.GetPath(nodes[0].Parent), arr);
                fireNodesInserted(treeView.GetPath(treeView.DropPosition.Node.Parent), arr);
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Delete nodes from tree and objects from memory and DB.
        /// </summary>
        /// <param name="treeView"></param>
        public void deleteSelectedNodes(TreeViewAdv treeView)
        {
            if (treeView.SelectedNodes.Count == 0)
                return;

            List<TreeNodeAdv> deleted = new List<TreeNodeAdv>();
            foreach (TreeNodeAdv node in treeView.SelectedNodes)
            {
                IPOIBase poi = node.Tag as Bookmarks.IPOIBase;
                if(poi == null)
                    continue;

                //We do not delete system entries
                if (poi.Id < 3)
                    continue;

                poi.deleteFromDB();
                deleted.Add(node);
            }
            fireNodesRemoved(treeView.GetPath(treeView.SelectedNode.Parent), deleted.ToArray());
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion
    }
}
