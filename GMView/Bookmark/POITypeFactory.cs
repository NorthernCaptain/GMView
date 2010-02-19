using System;
using System.Collections.Generic;
using System.Text;
using ncUtils;
using System.Data.Common;

namespace GMView.Bookmarks
{
    /// <summary>
    /// Class provides access to all known POI types. It loads them from DB and store in memory.
    /// 
    /// </summary>
    public class POITypeFactory
    {
        /// <summary>
        /// Single instance of the factory
        /// </summary>
        private static volatile POITypeFactory instance = null;

        /// <summary>
        /// Method for accessing instance of the factory
        /// </summary>
        /// <returns></returns>
        public static POITypeFactory singleton()
        {
            if (instance == null)
                instance = new POITypeFactory();
            return instance;
        }

        private List<POIType> allTypes = new List<POIType>();
        private Dictionary<string, POIType> nameTypes = new Dictionary<string, POIType>();
        private Dictionary<int, POIType> idTypes = new Dictionary<int, POIType>();

        /// <summary>
        /// Return the list of all POI types
        /// </summary>
        public List<POIType> items
        {
            get { return allTypes; }
        }

        /// <summary>
        /// Number of POI types in the factory
        /// </summary>
        public int count
        {
            get { return allTypes.Count; }
        }

        /// <summary>
        /// Return POIType by the given name. Throws exception if name does not exits
        /// </summary>
        /// <param name="tname"></param>
        /// <returns></returns>
        public POIType typeByName(string tname)
        {
            return nameTypes[tname];
        }

        /// <summary>
        /// Return POIType by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public POIType typeById(int id)
        {
            POIType ptype = null;
            idTypes.TryGetValue(id, out ptype);
            return ptype;
        }

        /// <summary>
        /// Private constructor, that loads POI types from DB
        /// </summary>
        private POITypeFactory()
        {
            loadList();
        }

        /// <summary>
        /// Loads types from DB and fills the list
        /// </summary>
        private void loadList()
        {
            DBObj dbo = null;
            Program.Log("Loading POI types");
            try
            {
                dbo = new DBObj("select id, name, description, icon, icon_cx, icon_cy, is_quick_type, "
                                + "is_auto_show, min_zoom_lvl, flags from poi_type "
                                + "where id > 0 order by sort_order desc, description");

                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
                    POIType pi = new POIType(reader);
                    registerType(pi);
                }
            }
            catch (System.Exception ex)
            {
                Program.Log("SQLError: " + ex.ToString());
            }
            finally
            {
                if (dbo != null)
                    dbo.Dispose();
            }
        }

        /// <summary>
        /// Register new POI type in the factory.
        /// </summary>
        /// <param name="newType"></param>
        public void registerType(POIType newType)
        {
            allTypes.Add(newType);
            nameTypes.Add(newType.Name, newType);
            idTypes.Add(newType.Id, newType);
        }

        /// <summary>
        /// Unregisters poi type from the factory
        /// </summary>
        /// <param name="ptype"></param>
        public void unregisterType(POIType ptype)
        {
            allTypes.Remove(ptype);
            nameTypes.Remove(ptype.Name);
            idTypes.Remove(ptype.Id);
        }

        /// <summary>
        /// Resort the list in alphabetic order
        /// </summary>
        public void resortAll()
        {
            allTypes.Clear();
            foreach (KeyValuePair<string, POIType> pair in nameTypes)
            {
                allTypes.Add(pair.Value);
            }
        }
    }
}
