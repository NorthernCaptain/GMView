﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView.GPS
{
    /// <summary>
    /// Class for describing source of the track - file, string buffer.
    /// This class also contains info that we acquire from track file on pre-loading stage
    /// </summary>
    public class TrackFileInfo
    {
        /// <summary>
        /// Type of the source we are using
        /// </summary>
        public enum SourceType { FileName, StringBuffer };

        public SourceType stype = SourceType.FileName;

        public string fileOrBuffer;
        /// <summary>
        /// Type of the track file we are using. Usually it is like a file extension: GPX, KML ...
        ///
        /// </summary>
        public string fileType;

        /// <summary>
        /// Desired track color
        /// </summary>
        public Color trackColor;

        /// <summary>
        /// Constructor for buffer and type, color set by default
        /// </summary>
        /// <param name="ifile"></param>
        /// <param name="itype"></param>
        public TrackFileInfo(string ifile, SourceType itype)
        {
            fileOrBuffer = ifile;
            stype = itype;
            trackColor = Color.DarkOrange;
        }

        /// <summary>
        /// Constructor for buffer and type and color
        /// </summary>
        /// <param name="ifile"></param>
        /// <param name="itype"></param>
        /// <param name="icolor"></param>
        public TrackFileInfo(string ifile, SourceType itype, Color icolor)
        {
            fileOrBuffer = ifile;
            stype = itype;
            trackColor = icolor;
        }
    }
}
