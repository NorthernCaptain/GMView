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
        private Dictionary<string, ITrackLoader> trackLoaders = new Dictionary<string, ITrackLoader>();


        private volatile TrackLoaderFactory instance;

        /// <summary>
        /// Return single instance of the factory
        /// </summary>
        public TrackLoaderFactory singleton
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
            trackLoaders.Add("GPX", null);
            trackLoaders.Add("KML", null);
            trackLoaders.Add("NMEA", null);
        }
    }
}
