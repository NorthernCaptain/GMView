using System;
using System.Collections.Generic;
using System.Text;
using ncUtils;
using System.Data.Common;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class holds and manages all POI Group objects. Read them from DB, but tree structure.
    /// </summary>
    public class POIGroupFactory
    {
        private static volatile POIGroupFactory instance = null;

        /// <summary>
        /// Return single instance of the factory
        /// </summary>
        /// <returns></returns>
        public static POIGroupFactory singleton()
        {
            if (instance == null)
                instance = new POIGroupFactory();
            return instance;
        }

        private List<POIGroup> allGroups = new List<POIGroup>();
        private Dictionary<int, POIGroup> idGroups = new Dictionary<int, POIGroup>();

        /// <summary>
        /// List of all POI groups loaded from DB.
        /// </summary>
        public List<POIGroup> AllGroups
        {
            get { return allGroups; }
            set { allGroups = value; }
        }


        private POIGroupFactory()
        {
            loadGroups();
        }

        /// <summary>
        /// Load all groups from DB into memory
        /// </summary>
        private void loadGroups()
        {
            Program.Log("Loading POI groups");
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select id, name, description from poi where is_group=1");
                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
            		//DO each item processing
                    addGroup(new POIGroup(reader));
                }

                reader.Close();

                dbo.commandText = @"select pgm.parent_id, pgm.member_id from poi_group_member pgm, "
                                 + "poi where poi.id = pgm.member_id and poi.is_group=1 order by 1,2";
                reader = dbo.cmd.ExecuteReader();
                while(reader.Read())
                {
                    int parent_id = reader.GetInt32(0);
                    int child_id = reader.GetInt32(1);
                    POIGroup parent, child;
                    if(!idGroups.TryGetValue(parent_id, out parent))
                    {
                        Program.Log("ERROR in loading POI group hierarchy: parent not found: " + parent_id);
                        continue;
                    }
                    if (!idGroups.TryGetValue(child_id, out child))
                    {
                        Program.Log("ERROR in loading POI group hierarchy: child not found: " + child_id);
                        continue;
                    }

                    parent.addChild(child);
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
        /// Find first group with the given name and return it. If group is not found then
        /// return null.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public POIGroup findByName(string groupName)
        {
            foreach (POIGroup pg in allGroups)
            {
                if (pg.Name.Equals(groupName))
                    return pg;
            }
            return null;
        }

        /// <summary>
        /// Create a simple group by a given name nested directly into 'root' group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public POIGroup createSimpleGroup(string groupName)
        {
            POIGroup pgroup = new POIGroup(groupName);
            pgroup.updateDB();

            addGroup(pgroup);

            POIGroup root = findByName("root");
            root.addChild(pgroup);
            root.updateChildrenLinksDB();
            return pgroup;
        }

        /// <summary>
        /// Register new group in the factory
        /// </summary>
        /// <param name="newGroup"></param>
        /// <returns></returns>
        public POIGroup addGroup(POIGroup newGroup)
        {
            allGroups.Add(newGroup);
            idGroups.Add(newGroup.Id, newGroup);
            return newGroup;
        }
    }
}
