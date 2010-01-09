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


        public POIGroupFactory()
        {
            loadGroups();
        }

        /// <summary>
        /// Load all groups from DB into memory
        /// </summary>
        private void loadGroups()
        {
            DBObj dbo = null;
            try
            {
                dbo = new DBObj(@"select id, name, description from poi where is_group=1");
                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
            		//DO each item processing
                    POIGroup grp = new POIGroup(reader);
                    allGroups.Add(grp);
                    idGroups.Add(grp.Id, grp);
                }

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
    }
}
