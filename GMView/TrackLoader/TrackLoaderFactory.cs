using System;
using System.Collections.Generic;
using System.Text;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Class holds a list of track loaders for different file formats.
    /// </summary>
    public class TrackLoaderFactory
    {
        private Dictionary<string, IFormatLoader> trackLoaders = new Dictionary<string, IFormatLoader>();


        private static volatile TrackLoaderFactory instance;

        /// <summary>
        /// Return single instance of the factory
        /// </summary>
        public static TrackLoaderFactory singleton
        {
            get
            {
                if (instance == null)
                    instance = new TrackLoaderFactory();
                return instance;
            }
        }

        private TrackLoaderFactory()
        {
            registerLoaders();
        }

        /// <summary>
        /// Register track loaders for different types in the factory
        /// </summary>
        private void registerLoaders()
        {
            trackLoaders.Add("gpx", new GPXLoader());
            trackLoaders.Add("kml", new KMLLoader());
            trackLoaders.Add("nmea", new NMEALoader());
            trackLoaders.Add("txt", new NMEALoader());
            trackLoaders.Add("bmark", new OldBmarkLoader());
            trackLoaders.Add("plt", new OZIPltLoader());
            trackLoaders.Add("wpt", new OZIWptLoader());
        }

        /// <summary>
        /// Gets a list of file filters for loading tracks
        /// </summary>
        /// <returns></returns>
        public List<ncFileControls.FileFilter> getTrackLoadFilters()
        {
            List<ncFileControls.FileFilter> filters = new List<ncFileControls.FileFilter>();
            foreach (IFormatLoader fldr in trackLoaders.Values)
            {
                ITrackLoader ldr = fldr as ITrackLoader;
                if (ldr == null) continue;
                if (ldr.trackLoadFileFilter() == null)
                    continue;
                if (filters.Contains(ldr.trackLoadFileFilter()))
                    continue;
                filters.Add(ldr.trackLoadFileFilter());
            }
            return filters;
        }

        /// <summary>
        /// Gets a list of file filters for saving tracks
        /// </summary>
        /// <returns></returns>
        public List<ncFileControls.FileFilter> getTrackSaveFilters()
        {
            List<ncFileControls.FileFilter> filters = new List<ncFileControls.FileFilter>();
            foreach (IFormatLoader fldr in trackLoaders.Values)
            {
                ITrackLoader ldr = fldr as ITrackLoader;
                if (ldr == null) continue;
                if (ldr.trackSaveFileFilter() == null)
                    continue;
                if (filters.Contains(ldr.trackSaveFileFilter()))
                    continue;
                filters.Add(ldr.trackSaveFileFilter());
            }
            return filters;
        }

        /// <summary>
        /// Gets a list of file filters for loading pois
        /// </summary>
        /// <returns></returns>
        public List<ncFileControls.FileFilter> getPOILoadFilters()
        {
            List<ncFileControls.FileFilter> filters = new List<ncFileControls.FileFilter>();
            foreach (IFormatLoader fldr in trackLoaders.Values)
            {
                IPOILoader ldr = fldr as IPOILoader;
                if (ldr == null) continue;
                if (ldr.poiLoadFileFilter() == null)
                    continue;
                if (filters.Contains(ldr.poiLoadFileFilter()))
                    continue;
                filters.Add(ldr.poiLoadFileFilter());
            }
            return filters;
        }

        /// <summary>
        /// Gets a list of file filters for saving pois
        /// </summary>
        /// <returns></returns>
        public List<ncFileControls.FileFilter> getPOISaveFilters()
        {
            List<ncFileControls.FileFilter> filters = new List<ncFileControls.FileFilter>();
            foreach (IFormatLoader fldr in trackLoaders.Values)
            {
                IPOILoader ldr = fldr as IPOILoader;
                if (ldr == null) continue;
                if (ldr.poiSaveFileFilter() == null)
                    continue;
                if (filters.Contains(ldr.poiSaveFileFilter()))
                    continue;
                filters.Add(ldr.poiSaveFileFilter());
            }
            return filters;
        }

        /// <summary>
        /// Return loader by a given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFormatLoader getLoaderByName(string name)
        {
            IFormatLoader res = null;
            trackLoaders.TryGetValue(name, out res);
            return res;
        }

        /// <summary>
        /// Finds and returns POILoader object for the given file or buffer
        /// Return null if not found
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public IPOILoader getPOILoader(GPS.TrackFileInfo fileInfo)
        {
            foreach (KeyValuePair<string, IFormatLoader> pair in trackLoaders)
            {
                if(pair.Value.isOurFormat(fileInfo))
                {
                    fileInfo.FileType = pair.Key;
                    return pair.Value as IPOILoader;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds and returns TrackLoader object for the given file or buffer
        /// Return null if not found
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public ITrackLoader getTrackLoader(GPS.TrackFileInfo fileInfo)
        {
            foreach (KeyValuePair<string, IFormatLoader> pair in trackLoaders)
            {
                if (pair.Value.isOurFormat(fileInfo))
                {
                    fileInfo.FileType = pair.Key;
                    return pair.Value as ITrackLoader;
                }
            }
            return null;
        }
    }
}
