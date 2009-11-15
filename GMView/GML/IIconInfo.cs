using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Provides basic icon information
    /// </summary>
    public interface IIconInfo
    {
        /// <summary>
        /// Return the name of the icon
        /// </summary>
        string iconName
        {
            get;
        }

        /// <summary>
        /// Return icon hot point delta x
        /// </summary>
        int iconDeltaX
        {
            get;
        }

        /// <summary>
        /// Return icon hot point delta y
        /// </summary>
        int iconDeltaY
        {
            get;
        }
    }
}
