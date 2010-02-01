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
                    poi.IsAutoShow = !value;
                }
                shown = value;
            }
        }

        /// <summary>
        /// Need for treeView compatibility calls
        /// </summary>
        public bool IsShownCentered
        {
            get { return shown; }
            set { IsShown = value; }
        }


        private bool isDisabled = false;

        /// <summary>
        /// Get or set disabled flag. On set do this recursively for all sub-items
        /// </summary>
        public bool IsDisabled
        {
            get { return isDisabled; }
            set 
            { 
                isDisabled = value;
                ncUtils.DBConnPool.singleton.beginThreadTransaction();
                try
                {
                    List<Bookmark> pois = Owner.PoiFactory.loadByParent(this.id, false);
                    if (pois != null)
                    {
                        foreach (Bookmark poi in pois)
                        {
                            poi.IsDisabled = value;
                        }
                    }

                    if (children != null)
                    {
                        foreach (POIGroup sub in children)
                        {
                            sub.IsDisabled = value;
                        }
                    }
                    updateDB();
                }
                finally
                {
                    ncUtils.DBConnPool.singleton.commitThreadTransaction();
                }
                
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
            isDisabled = (reader.GetInt32(3) == 1);
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
                    dbo = new DBObj(@"update poi set name=@NAME, description=@DESCRIPTION, is_disabled=@ISDISABLED where id=@ID");
                    dbo.addStringPar("@NAME", name);
                    dbo.addStringPar("@DESCRIPTION", description);
                    dbo.addIntPar("@ISDISABLED", (isDisabled ? 1 : 0));
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

        /// <summary>
        /// Deletes Group from DB, calls itself recursively for deleting subgroups
        /// </summary>
        public void deleteFromDB()
        {
            if (children != null)
            {
                POIGroup[] arr = new POIGroup[children.Count];
                children.CopyTo(arr, 0);

                foreach (POIGroup subgrp in arr)
                {
                    subgrp.deleteFromDB();
                }
            }

            this.IsShown = false;

            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"delete from poi_spatial where id in "
                        + "(select member_id from poi_group_member where parent_id=@PARENT_ID)");
                dbo.addIntPar("@PARENT_ID", id);
                dbo.executeNonQuery();

                dbo.commandText = "delete from poi where id in (select member_id from poi_group_member where parent_id=@PARENT_ID)";
                dbo.addIntPar("@PARENT_ID", id);
                dbo.executeNonQuery();

                dbo.commandText = "delete from poi_group_member where parent_id=@PARENT_ID";
                dbo.addIntPar("@PARENT_ID", id);
                dbo.executeNonQuery();

                dbo.commandText = "delete from poi where id=@ID";
                dbo.addIntPar("@ID", id);
                dbo.executeNonQuery();

                if(childrenPOIs != null)
                {
                    Bookmark[] childrenCopy = childrenPOIs.ToArray();
                    foreach (Bookmark poi in childrenCopy)
                    {
                        poi.unregisterMe();
                    }
                }
                unregisterMe();
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

        public void unregisterMe()
        {
            if (parent != null)
                parent.delChild(this);

            if (owner != null)
                owner.unregister(this);
        }

        public bool IsGroup
        {
            get { return true; }
        }


        private POIGroupFactory owner;
        

        /// <summary>
        /// Owner of this group. Owner is a factory that creates and manages this object
        /// </summary>
        public POIGroupFactory Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// Return full path to the group limited by tillGroup parent
        /// </summary>
        /// <param name="tillGroup"></param>
        /// <returns></returns>
        public string getPathTill(POIGroup tillGroup)
        {
            if (tillGroup == this || parent == null)
                return string.Empty;

            StringBuilder buf = new StringBuilder(name);

            POIGroup cur = parent;
            while (cur != null && cur != tillGroup)
            {
                buf.Insert(0, cur.name + "/");
                cur = cur.Parent as POIGroup;
            }
            return buf.ToString();
        }

        private readonly char[] groupSep = new char[] { '/' };
        /// <summary>
        /// Find a subgroup by the given path string. If there is no such group, then creates
        /// it and all its parents
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public POIGroup getSubGroupByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return this;

            string[] elements = path.Split(groupSep);

            POIGroup current = this;
            for (int i = 0; i < elements.Length; i++)
            {
                current = current.findOrCreateSubGroup(elements[i]);
            }
            return current;
        }

        /// <summary>
        /// Find subgroup by name or create it if not found in the children list
        /// </summary>
        /// <param name="subname"></param>
        /// <returns></returns>
        private POIGroup findOrCreateSubGroup(string subname)
        {
            if (string.IsNullOrEmpty(subname))
                return this;

            POIGroup foundGroup = null;

            if (children != null)
            {
                foreach (POIGroup sub in children)
                {
                    if (sub.name.Equals(subname))
                    {
                        return sub;
                    }
                }
            }

            foundGroup = new POIGroup(subname);
            foundGroup.updateDB();

            this.addChild(foundGroup);
            owner.register(foundGroup);
            
            this.updateChildrenLinksDB();
            
            return foundGroup;
        }
    }
}
