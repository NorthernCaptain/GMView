using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

namespace GMView.GPS
{
    /// <summary>
    /// Class for describing source of the track - file, string buffer.
    /// This class also contains info that we acquire from track file on pre-loading stage
    /// </summary>
    public class TrackFileInfo: IDisposable
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
        private string fileType = "gpx";

        public string FileType
        {
            get { return fileType; }
            set { fileType = value; if (string.IsNullOrEmpty(fileType)) fileType = "gpx"; }
        }

        /// <summary>
        /// Desired track color
        /// </summary>
        public Color trackColor;


        /// <summary>
        /// Not null if we have opened xml document for our file or buffer
        /// </summary>
        public XmlDocument openedXml = null;

        /// <summary>
        /// Need to load POI while loading track or not?
        /// </summary>
        public bool needPOI = true;

        /// <summary>
        /// Group name for POIs, treat it like a root group
        /// </summary>
        public string poiParentGroupName = "Imported";

        /// <summary>
        /// Number of POIs in file, detected by preload method
        /// 0 = no POI
        /// -1 = POIs are there, but number of them is unrecognized
        /// >0 = exact number of POIs
        /// </summary>
        public int preloadPOICount = 0;

        /// <summary>
        /// Number of points in the track inside the file
        /// 0 = no track
        /// -1 = track is there, but has unknown number of points
        /// >0 = number of points
        /// </summary>
        public int preloadTPointCount = 0;

        /// <summary>
        /// Number of points in the route inside the file
        /// 0 = no route in the file
        /// -1 = route is there, but has unknown number of points
        /// >0 = number of points
        /// </summary>
        public int preloadRouteCount = 0;

        /// <summary>
        /// Name of the track extracted from file contents
        /// </summary>
        public string preloadName = string.Empty;

        /// <summary>
        /// Try to open document as xml and return this XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument openXml()
        {
            if(openedXml != null)
                return openedXml;

            if(stype == SourceType.FileName)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileOrBuffer);
                openedXml = doc;
            } else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(fileOrBuffer);
                openedXml = doc;
            }

            return openedXml;
        }

        /// <summary>
        /// Opens and return TextReader for file or buffer stored in the class
        /// </summary>
        /// <returns></returns>
        public TextReader openReader()
        {
            TextReader reader;

            if (this.stype == SourceType.FileName)
            {
                reader = new System.IO.StreamReader(this.fileOrBuffer);
            }
            else
                reader = new System.IO.StringReader(this.fileOrBuffer);

            return reader;
        }

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

        #region IDisposable Members

        public void Dispose()
        {
            if (openedXml != null)
                openedXml = null;
        }

        #endregion
    }
}
