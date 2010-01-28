using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    public interface IPOILoader: IFormatLoader
    {
        /// <summary>
        /// Import POIs from File into the bookmark factory and create subgroups
        /// in the groupFactory.
        /// Return number of imported POIs
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="intoFactory"></param>
        /// <param name="groupFactory"></param>
        /// <returns></returns>
        int importPOIs(GPS.TrackFileInfo fi, BookMarkFactory intoFactory,
                        Bookmarks.POIGroupFactory groupFactory);
        
        /// <summary>
        /// Export given list of POIs and group into the file or buffer
        /// Return buffer as fileInfo.fileOrBuffer filled with data
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="pFactory"></param>
        /// <param name="groups"></param>
        /// <param name="poilist"></param>
        /// <param name="parentGroup"></param>
        void exportPOIs(GPS.TrackFileInfo fileInfo, BookMarkFactory pFactory,
                        LinkedList<Bookmarks.POIGroup> groups,
                        List<Bookmark> poilist, Bookmarks.POIGroup parentGroup);
    }
}
