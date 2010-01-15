using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using ncUtils;
using System.Drawing;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class represents POI group, that is used for grouping POIs in user interface
    /// </summary>
    public class POIGroup: IPOIBase
    {
        protected int id = 0;
        /// <summary>
        /// Unique id of the group, filled when inserting group into DB
        /// </summary>
        public int Id
        {
            get { return id; }
        }
        protected string name;

        /// <summary>
        /// Group name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; updateDB(); }
        }
        protected string description;

        /// <summary>
        /// Group long description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; updateDB(); }
        }


        protected Image icon;
        public Image IconImage
        {
            get { return null; }
            set {}
        }

        private bool shown = false;
        /// <summary>
        /// If we set this property to true, then we show all POIs in this group,
        /// if to false, then we hide all POIs
        /// </summary>
        public bool IsShown
        {
            get { return shown; }
            set 
            {
                if (value == shown || childrenPOIs == null)
                    return;
                
                foreach (Bookmark poi in childrenPOIs)
                {
                    poi.IsShown = value;
                }
                shown = value;
            }
        }

        public POIGroup()
        {
            icon = IconFactory.singleton.getIcon(POITypeFactory.singleton().typeById(1)).img;
        }

        public POIGroup(string iname)
        {
            name = iname;
            description = iname;
            icon = IconFactory.singleton.getIcon(POITypeFactory.singleton().typeById(1)).img;
        }

        /// <summary>
        /// Constructor reads group data from opened DB cursor.
        /// Fields must be in the following order:
        ///   id, name, description
        /// </summary>
        /// <param name="reader"></param>
        public POIGroup(System.Data.Common.DbDataReader reader)
        {
            id = reader.GetInt32(0);
            name = reader.GetString(1);
            description = reader.GetString(2);
            icon = IconFactory.singleton.getIcon(POITypeFactory.singleton().typeById(1)).img;
        }

        /// <summary>
        /// Update or insert group data into DB table
        /// </summary>
        public virtual void updateDB()
        {
            if(id == 0)
            {
                DBObj dbo = null;
                try
                {
                    dbo = new DBObj(@"insert into poi (name, description, type, is_group) "
                                + "values (@NAME, @DESCRIPTION, 0, 1)");
                    dbo.addStringPar("@NAME", name);
                    dbo.addStringPar("@DESCRIPTION", description);
                    dbo.executeNonQuery();

                    id = dbo.seqCurval("poi");
                }
                catch (System.Exception e)
                {
                    Program.Log("SQLError: " + e.ToString());
                }
                finally
                {
                    if (dbo != null)
                        dbo.Dispose();
                }
            }
            else
            {
                DBObj dbo = null;
                try
                {
                    dbo = new DBObj(@"update poi set name=@NAME, description=@DESCRIPTION where id=@ID");
                    dbo.addStringPar("@NAME", name);
                    dbo.addStringPar("@DESCRIPTION", description);
                    dbo.addIntPar("@ID", id);
                    dbo.executeNonQuery();
                }
                catch (System.Exception e)
                {
                    Program.Log("SQLError: " + e.ToString());
                }
                finally
                {
                    if (dbo != null)
                        dbo.Dispose();
                }
            }
        }

        /// <summary>
        /// Link to the parent group
        /// </summary>
        protected POIGroup parent;

        public IPOIBase Parent
        {
            get { return parent; }
            set { parent = value as POIGroup; }
        }

        /// <summary>
        /// List of children of this group
        /// </summary>
        protected LinkedList<POIGroup> children;

        public LinkedList<POIGroup> Children
        {
            get { return children; }
            set { children = value; }
        }

        /// <summary>
        /// Children that are not subgroups, but real POIs. This list can be null or contains
        /// only active (loaded) pois, not all of them.
        /// </summary>
        protected List<Bookmark> childrenPOIs = null;
        /// <summary>
        /// Children that are not subgroups, but real POIs. This list can be null or contains
        /// only active (loaded) pois, not all of them.
        /// </summary>
        public List<Bookmark> ChildrenPOIs
        {
            get { return childrenPOIs; }
            set { childrenPOIs = value; }
        }
        /// <summary>
        /// Adds child group to the group, create all necessary links
        /// </summary>
        /// <param name="child"></param>
        public void addChild(IPOIBase child)
        {
            if (child is POIGroup)
            {
                if (children == null)
                    children = new LinkedList<POIGroup>();
                children.AddLast(child as POIGroup);
            }
            else
                if (childrenPOIs != null)
                    childrenPOIs.Add(child as Bookmark);
            child.Parent = this;
        }

        /// <summary>
        /// Add child to the group placing it after the requested one. Works only for
        /// groups, for normal POIs do internal call to addChild.
        /// </summary>
        /// <param name="after"></param>
        /// <param name="child"></param>
        public void addChildAfter(IPOIBase after, IPOIBase child)
        {
            if(after == null)
            {
                addChild(child);
                return;
            }

            POIGroup childGroup = child as POIGroup;
            POIGroup afterGroup = after as POIGroup;
            if (afterGroup != null)
            {
                LinkedListNode<POIGroup> found = children.Find(afterGroup);
                if (childGroup != null && found != null)
                {
                    children.AddAfter(found, childGroup);
                    return;
                }
            }
            addChild(child);
        }

        /// <summary>
        /// Remove child from this parent
        /// </summary>
        /// <param name="child"></param>
        public void delChild(IPOIBase child)
        {
            POIGroup grp = child as POIGroup;
            if (grp != null)
            {
                if (children == null)
                    return;
                children.Remove(grp);
            }
            else
                if (childrenPOIs != null)
                    childrenPOIs.Remove(child as Bookmark);
        }

        /// <summary>
        /// Deletes all group member links where this group is parent, then insert all
        /// its children.
        /// </summary>
        public void updateChildrenLinksDB()
        {
            if (children == null)
                return;

            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from poi_group_member where parent_id=@ID and member_is_group=1");
                dbo.addIntPar("@ID", id);
                dbo.executeNonQuery();

                dbo.commandText = "insert into poi_group_member (parent_id, member_id, member_is_group) "
                                + "values(@PARENT_ID, @MEMBER_ID, 1)";

                foreach (POIGroup childnode in children)
                {
                    dbo.cmd.Parameters.Clear();
                    dbo.addIntPar("@PARENT_ID", id);
                    dbo.addIntPar("@MEMBER_ID", childnode.Id);
                    dbo.executeNonQuery();
                }
            }
            catch (System.Exception e)
            {
            	Program.Log("SQLError: " + e.ToString());
            
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        /// <summary>
        /// Reparent group to another parent. Updates DB.
        /// </summary>
        /// <param name="parentGroup"></param>
        public void reparentMeTo(IPOIBase parentGroup, IPOIBase after)
        {
            if (parentGroup == null)
                return;

            POIGroup oldParent = parent;
            if (oldParent != null)
                oldParent.delChild(this);
            parentGroup.addChildAfter(after, this);

            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"update poi_group_member set parent_id=@NEWPARENT where "
                      + "parent_id=@OLDPARENT and member_id=@CURRENT");
                dbo.addIntPar("@NEWPARENT", parentGroup.Id);
                dbo.addIntPar("@OLDPARENT", oldParent.Id);
                dbo.addIntPar("@CURRENT", id);
                dbo.executeNonQuery();
            }
            catch (System.Exception e)
            {
                Program.Log("SQLError: " + e.ToString());

            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        public bool IsGroup
        {
            get { return true; }
        }

    }
}
