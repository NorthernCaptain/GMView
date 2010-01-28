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
        
    }
}
