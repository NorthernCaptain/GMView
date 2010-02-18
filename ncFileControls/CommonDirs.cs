using System;
using System.Collections.Generic;
using System.Text;

namespace ncFileControls
{
    /// <summary>
    /// Holds common used directories like My computer, My Documents and so on
    /// </summary>
    public class CommonDirs
    {
        /// <summary>
        /// "My computer" folder
        /// </summary>
        public class MyComputerBook : IDirBookmark
        {
            #region IDirBookmark Members

            public string directory
            {
                get { return Environment.GetFolderPath(Environment.SpecialFolder.MyComputer); }
            }
            #endregion
        }

        /// <summary>
        /// "My Documents" folder
        /// </summary>
        public class MyDocumentsBook : IDirBookmark
        {
            #region IDirBookmark Members

            public string directory
            {
                get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
            }
            #endregion
        }

        public class MyDesktopBook : IDirBookmark
        {
            #region IDirBookmark Members

            public string directory
            {
                get { return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }
            }

            #endregion
        }
    }
}
