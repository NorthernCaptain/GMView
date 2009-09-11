using System;
using System.Collections.Generic;
using System.Text;

namespace ncGeo
{
    /// <summary>
    /// Common interface for geo objects, provide geographical coordinates
    /// </summary>
    public interface IGeoCoord
    {
        /// <summary>
        /// Longitude of the object
        /// </summary>
        double longitude
        {
            get;
            set;
        }

        /// <summary>
        /// Latitude of the object
        /// </summary>
        double latitude
        {
            get;
            set;
        }

        /// <summary>
        /// Altitudeof the object
        /// </summary>
        double altitude
        {
            get;
            set;
        }
    }
}
