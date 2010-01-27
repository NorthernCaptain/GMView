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
    public class POITreeModel: POIGroupTreeModel
    {

        public POITreeModel(POIGroupFactory igroupFact, TreeViewAdv itree)
            : base(igroupFact, itree)
        {
        }

        #region ITreeModel Members

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if(treePath.IsEmpty())
            {
                return rootList;
            }

            POIGroup parent = treePath.LastNode as POIGroup;
            if(parent == null)
                return null;
            LinkedList<POIGroup> grplist = parent.Children;
            List<Bookmark> poilist = BookMarkFactory.singleton.loadByParent(parent.Id, true);
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

        public override bool IsLeaf(TreePath treePath)
        {
            return !(treePath.LastNode is POIGroup);
        }

        #endregion
    }
}
