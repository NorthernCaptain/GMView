using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ncGeo;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Implements GPX file format for loading and saving operations.
    /// </summary>
    public class GPXLoader: IPOILoader, ITrackLoader
    {
        private Bookmarks.POIGroupFactory groupFactory;
        private BookMarkFactory poiFactory;

        /// <summary>
        /// Public constructor
        /// </summary>
        public GPXLoader()
        {

        }

        #region IPOILoader Members

        public bool isOurFormat(GMView.GPS.TrackFileInfo fi)
        {
            string first_line = string.Empty;

            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
            {
                System.IO.StreamReader reader;
                reader = new System.IO.StreamReader(fi.fileOrBuffer);
                reader.ReadLine(); //skip ?xml? definition
                first_line = reader.ReadLine();
                reader.Close();
            } else
            {
                System.IO.StringReader reader;
                reader = new System.IO.StringReader(fi.fileOrBuffer);
                reader.ReadLine();
                first_line = reader.ReadLine();
                reader.Close();
            }
            return first_line.Contains("gpx");
        }

        /// <summary>
        /// Import POI from GPX file into the given factory.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="intoFactory"></param>
        /// <param name="groupFactory"></param>
        /// <returns></returns>
        public int importPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory intoFactory, 
                    GMView.Bookmarks.POIGroupFactory groupFactory)
        {
            XmlDocument doc = fi.openXml();

            poiFactory = intoFactory;
            this.groupFactory = groupFactory;

            if (doc.DocumentElement.Name != "gpx")
                throw new ApplicationException("Not a valid GPX file! Could not find gpx root tag.");

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);

            { //retreive xmlns
                XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (xnsnode != null)
                    nsm.AddNamespace("gpx", xnsnode.Value);
                else
                {
                    nsm.AddNamespace("gpx", "");
                }
            }

            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//gpx:wpt", nsm);
            if (nlist.Count == 0)
                return 0;

            return subloadBookmarks(nlist, nsm, Path.GetFileNameWithoutExtension(fi.fileOrBuffer));
        }

        private int subloadBookmarks(XmlNodeList nlist, XmlNamespaceManager nsm, string groupname)
        {
            int count = 0;
            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            NMEA_RMC rmc = new NMEA_RMC();

            if (string.IsNullOrEmpty(groupname))
                groupname = "Imported";

            try
            {
                Bookmarks.POIGroup parentGroup = groupFactory.rootGroup.getSubGroupByPath(groupname);
                foreach (XmlNode node in nlist)
                {
                    Bookmark bmark = new Bookmark();
                    bmark.IsDbChange = false;

                    bmark.lat = NMEACommand.getDouble(node.Attributes.GetNamedItem("lat").Value);
                    bmark.lon = NMEACommand.getDouble(node.Attributes.GetNamedItem("lon").Value);

                    XmlNode xnode = node.SelectSingleNode("./gpx:time", nsm);
                    if (xnode != null)
                    {
                        try
                        {
                            rmc.utc_time = DateTime.Parse(xnode.InnerText).ToUniversalTime();
                        }
                        catch
                        {
                            rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                        }

                    }
                    else
                        rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                    bmark.Created = rmc.utc_time.ToLocalTime();

                    xnode = node.SelectSingleNode("./gpx:name", nsm);
                    if (xnode == null)
                        continue;
                    bmark.Name = xnode.InnerText.Trim();

                    xnode = node.SelectSingleNode("./gpx:desc", nsm);
                    if (xnode != null)
                        bmark.Description = xnode.InnerText.Trim();

                    xnode = node.SelectSingleNode("./gpx:cmt", nsm);
                    if (xnode != null)
                        bmark.Comment = xnode.InnerText.Trim();

                    //Empty description but non-empty comment
                    if (string.IsNullOrEmpty(bmark.Description) &&
                        !string.IsNullOrEmpty(bmark.Comment))
                        bmark.Description = bmark.Comment;

                    xnode = node.SelectSingleNode("./gpx:ele", nsm);
                    if (xnode != null)
                    {
                        bmark.altitude = NMEACommand.getDouble(xnode.InnerText.Trim());
                    }

                    xnode = node.SelectSingleNode("./gpx:extensions/gpx:group", nsm);
                    if (xnode != null)
                    {
                        bmark.group = xnode.InnerText.Trim();
                    }

                    xnode = node.SelectSingleNode("./gpx:sym", nsm);
                    if (xnode != null)
                    {
                        bmark.PtypeS = xnode.InnerText.Trim();
                    }

                    bmark.IsDbChange = true;
                    bmark.updateDB();

                    bmark.addLinkDB(parentGroup.getSubGroupByPath(bmark.group));

                    count++;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                ncUtils.DBConnPool.singleton.commitThreadTransaction();
            }

            return count;
        }


        #endregion

        #region ITrackLoader Members

        public void preLoad(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        public GPSTrack load(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
