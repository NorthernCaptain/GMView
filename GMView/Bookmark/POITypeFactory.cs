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
        private static POITypeFactory instance = null;

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

            try
            {
                dbo = new DBObj("select id, name, description, icon, icon_cx, icon_cy "
                                + "from poi_type where id > 0 order by sort_order desc, description");

                DbDataReader reader = dbo.cmd.ExecuteReader();
                while (reader.Read())
                {
                    POIType pi = new POIType(reader);
                    allTypes.Add(pi);
                    nameTypes.Add(pi.Name, pi);
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
    }
}
