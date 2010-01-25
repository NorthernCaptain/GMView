using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;
using System.Windows.Forms;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Tree model for POI groups without POI itself
    /// </summary>
    public class POIGroupTreeModel : ITreeModel
    {
        protected POIGroupFactory groupFact;
        protected TreeViewAdv treeView;

        public POIGroupTreeModel(POIGroupFactory igroupFact, TreeViewAdv itree)
        {
            treeView = itree;
            groupFact = igroupFact;
            rootList.Add(groupFact.findById(0)); //Adding root element
        }

        protected List<POIGroup> rootList = new List<POIGroup>();

        #region ITreeModel Members

        public virtual System.Collections.IEnumerable GetChildren(TreePath treePath)
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

        public virtual bool IsLeaf(TreePath treePath)
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
            if (NodesInserted != null)
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
        /// Adds new group to the selected node in the tree
        /// </summary>
        public virtual void addNewGroup()
        {
            Bookmarks.POIGroup parent_group = treeView.SelectedNode.Tag as Bookmarks.POIGroup;
            Bookmarks.POIGroup new_group = new Bookmarks.POIGroup("?new group?");
            new_group.updateDB();
            parent_group.addChild(new_group);
            parent_group.updateChildrenLinksDB();
            groupFact.addGroup(new_group);
            fireNodesInserted(treeView.GetPath(treeView.SelectedNode), new object[] { new_group });
            treeView.SelectedNode.IsExpanded = true;
        }


        /// <summary>
        /// Process Drop operation for dragged nodes in the tree
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="e"></param>
        public virtual void doDropProcessing(DragEventArgs e)
        {
            treeView.BeginUpdate();

            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            Bookmarks.POIGroup dropNode = treeView.DropPosition.Node.Tag as Bookmarks.POIGroup;

            List<object> removed = new List<object>();

            ncUtils.DBConnPool.singleton.beginThreadTransaction();

            try
            {
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
            }
            catch (System.Exception)
            {

            }
            finally
            {
                ncUtils.DBConnPool.singleton.commitThreadTransaction();
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Delete nodes from tree and objects from memory and DB.
        /// </summary>
        /// <param name="treeView"></param>
        public virtual void deleteSelectedNodes()
        {
            if (treeView.SelectedNodes.Count == 0)
                return;
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            try
            {
                List<TreeNodeAdv> deleted = new List<TreeNodeAdv>();
                foreach (TreeNodeAdv node in treeView.SelectedNodes)
                {
                    IPOIBase poi = node.Tag as Bookmarks.IPOIBase;
                    if (poi == null)
                        continue;

                    //We do not delete system entries
                    if (poi.Id < 3)
                        continue;

                    poi.deleteFromDB();
                    deleted.Add(node);
                }
            }
            catch (System.Exception)
            {

            }
            finally
            {
                ncUtils.DBConnPool.singleton.commitThreadTransaction();
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
