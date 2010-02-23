using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Implements loading and saving files in OziExplorer format
    /// WPT - is files for POI information
    /// </summary>
    public class OZIWptLoader: IPOILoader
    {
        private static readonly ncFileControls.FileFilter wptFilter =
                    new ncFileControls.FileFilter("Ozi Explorer WPT file format (*.wpt)", "*.wpt");

        #region IPOILoader Members

        public ncFileControls.FileFilter poiLoadFileFilter()
        {
            return wptFilter;
        }

        public ncFileControls.FileFilter poiSaveFileFilter()
        {
            return wptFilter;
        }

        public int importPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory intoFactory, GMView.Bookmarks.POIGroupFactory groupFactory)
        {
            throw new NotImplementedException();
        }

        public void exportPOIs(GMView.GPS.TrackFileInfo fileInfo, BookMarkFactory pFactory, LinkedList<GMView.Bookmarks.POIGroup> groups, List<Bookmark> poilist, GMView.Bookmarks.POIGroup parentGroup)
        {
            throw new NotImplementedException();
        }


        public bool isOurFormat(GMView.GPS.TrackFileInfo info)
        {
            string first_line = string.Empty;

            System.IO.TextReader reader = info.openReader();

            try
            {
                first_line = reader.ReadLine();
                if (string.IsNullOrEmpty(first_line))
                    return false;
                if (first_line.StartsWith("OziExplorer Waypoint File"))
                    return true;
            }
            catch (System.Exception)
            {
            }
            finally
            {
                reader.Close();
            }
            return false;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new OZIWptLoader();
        }

        #endregion
    }
}
