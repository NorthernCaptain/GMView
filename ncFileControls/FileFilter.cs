using System;
using System.Collections.Generic;
using System.Text;

namespace ncFileControls
{
    /// <summary>
    /// Class holds information about file filter - its name and filtering mask
    /// </summary>
    public class FileFilter
    {
        private string name;

        /// <summary>
        /// Display name of the filter
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string filter;

        /// <summary>
        /// Gets or sets filtering mask
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        public override string ToString()
        {
            return name;
        }

        public FileFilter(string iname, string ifilter)
        {
            name = iname;
            filter = ifilter;
        }
    }
}
