using System;
using System.Collections.Generic;
using System.Text;

namespace ncFileControls
{
    /// <summary>
    /// Common interface for retrieving directory name from bookmark
    /// </summary>
    public interface IDirBookmark
    {
        /// <summary>
        /// Directory full path name stored in this bookmark
        /// </summary>
        string directory
        {
            get;
        }
    }
}
