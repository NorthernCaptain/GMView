using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using ncUtils;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class represents POI group, that is used for grouping POIs in user interface
    /// </summary>
    public class POIGroup
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
            set { name = value; }
        }
        protected string description;

        /// <summary>
        /// Group long description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public POIGroup()
        {

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

        public POIGroup Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// List of children of this group
        /// </summary>
        protected LinkedList<POIGroup> children;
        /// <summary>
        /// Adds child group to the group, create all necessary links
        /// </summary>
        /// <param name="child"></param>
        public void addChild(POIGroup child)
        {
            children.AddLast(child);
            child.parent = this;
        }
    }
}
