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
                    fileInfo.fileType = pair.Key;
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
                    fileInfo.fileType = pair.Key;
                    return pair.Value as ITrackLoader;
                }
            }
            return null;
        }
    }
}
