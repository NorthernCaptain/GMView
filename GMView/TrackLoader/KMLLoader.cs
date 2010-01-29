using System;
using System.Collections.Generic;
using System.Text;
using ncGeo;
using System.Xml;
using System.IO;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Class implements loading and saving in KML (google) file format
    /// </summary>
    public class KMLLoader: ITrackLoader, IPOILoader
    {
        private Bookmarks.POIGroupFactory groupFactory;
        private BookMarkFactory poiFactory;

        #region ITrackLoader Members

        public void preLoad(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads GPSTrack from Google Earth KML file. Throws an ApplicationException if something goes wrong
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public GPSTrack load(GMView.GPS.TrackFileInfo fi)
        {
            XmlDocument doc = fi.openXml();

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
            nsm.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

            if (doc.DocumentElement.Name != "kml")
                throw new ApplicationException("Not a valid KML file! Could not find kml root tag.");

            { //retrieve xmlns
                XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (xnsnode != null)
                    nsm.AddNamespace("kml", xnsnode.Value);
                else
                {
                    nsm.AddNamespace("kml", "");
                }
            }


            GPSTrack track = new GPSTrack();

            string tname;

            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
            {
                string dirname = Path.GetDirectoryName(fi.fileOrBuffer);
                tname = dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                      + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
                track.track_name = "Track: " + tname;
                track.wayObject.name = "Route: " + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
            }
            else
            {
                tname = "KML buffer " + DateTime.Now.ToShortDateString()
                    + " " + DateTime.Now.ToShortTimeString();
                track.track_name = "Track: " + tname;
                track.wayObject.name = "Route: " + tname;
            }


            XmlNode folder = selectKMLFolders(doc, nsm);
            XmlNode titleNode = null;

            if (folder == null)
            {
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");
            }

            XmlNodeList nlist = folder.SelectNodes("./kml:Placemark", nsm);
            string nodeval = "";
            foreach (XmlNode xnode in nlist)
            {

                XmlNodeList coordlist;
                coordlist = xnode.SelectNodes("./kml:LineString/kml:coordinates", nsm);
                if (coordlist.Count == 0)
                    coordlist = xnode.SelectNodes("./*/kml:LineString/kml:coordinates", nsm);
                foreach (XmlNode cnode in coordlist)
                {
                    loadKMLPoints(track, cnode.InnerText);
                    if (titleNode == null)
                        titleNode = xnode.SelectSingleNode("./kml:name", nsm);
                }
            }

            if (track.trackPointData.Count == 0)
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");
            XmlNode docTitleNode = folder.SelectSingleNode("./kml:name", nsm);
            if (docTitleNode != null)
                titleNode = docTitleNode;

            if (titleNode != null)
            {
                nodeval = titleNode.InnerText;
                if (nodeval.Length > 7 && nodeval.Substring(0, 7) == "Track: ")
                    nodeval = nodeval.Substring(7);
                track.track_name = "Track: " + nodeval;
                track.wayObject.name = "Route: " + nodeval;
                tname = nodeval;
            }

            subloadBookmarks(nlist, nsm, track.track_name);

            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
                track.fileName = fi.fileOrBuffer;
            else
                track.fileName = tname + ".kml";

            track.calculateParameters();
            track.lastNonZeroPos = track.lastData;

            return track;
        }

        /// <summary>
        /// Searches for Folder tags and return first folder that contains coordinates
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nsm"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        private XmlNode selectKMLFolders(XmlDocument doc, XmlNamespaceManager nsm)
        {
            string nodeval = "";
            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//kml:Folder", nsm);
            foreach (XmlNode xnode in nlist)
            {
                if (ncUtils.XmlHelper.xmltag(xnode, "./kml:Placemark/kml:LineString/kml:coordinates", nsm, ref nodeval)
                    || ncUtils.XmlHelper.xmltag(xnode, "./kml:Placemark/*/kml:LineString/kml:coordinates", nsm, ref nodeval))
                    return xnode;
            }
            XmlNode res = doc.DocumentElement.SelectSingleNode("./kml:Document", nsm);
            if (res != null && (res.SelectSingleNode("./kml:Placemark/*/kml:LineString/kml:coordinates", nsm) != null
                            || res.SelectSingleNode("./kml:Placemark/kml:LineString/kml:coordinates", nsm) != null))
                return res;
            return null;
        }

        private void loadKMLPoints(GPSTrack track, string nodeval)
        {
            double lon, lat, hei;
            char[] sep = new char[] { ' ', '\n', '\r', '\t' };
            string[] tuples = nodeval.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            DateTime now = DateTime.Now.ToUniversalTime();
            foreach (string tuple in tuples)
            {
                if (splitKMLCoordTuple(tuple, out lon, out lat, out hei))
                {
                    NMEA_RMC rmc = new NMEA_RMC();
                    rmc.lat = lat;
                    rmc.lon = lon;
                    rmc.height = hei;
                    rmc.state = NMEACommand.Status.DataOK;
                    rmc.utc_time = now;
                    rmc.ptype = NMEA_LL.PointType.TP;

                    track.trackPointData.AddLast(rmc);
                    track.lastData = track.LastTrackPos = track.lastNonZeroPos = rmc;
                }
            }
        }

        #endregion

        #region IFormatLoader Members

        public bool isOurFormat(GMView.GPS.TrackFileInfo fi)
        {
            string first_line = string.Empty;

            System.IO.TextReader reader;
            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
            {
                reader = new System.IO.StreamReader(fi.fileOrBuffer);
            }
            else
                reader = new System.IO.StringReader(fi.fileOrBuffer);

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    first_line = reader.ReadLine();
                    if (string.IsNullOrEmpty(first_line))
                        continue;
                    if (first_line.Contains("<kml"))
                        return true;
                }
            }
            catch (System.Exception)
            {
            }
            finally
            {
                reader.Close();
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Parse KML tuple with coordinates in format: 'lon, lat, alt' or 'lon, lat'
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="hei"></param>
        /// <returns></returns>
        public static bool splitKMLCoordTuple(string tuple, out double lon, out double lat, out double hei)
        {
            char[] sep = new char[] { ',' };

            string[] elements = tuple.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            lon = lat = hei = 0.0;

            if (elements.Length < 2)
                return false;
            lon = NMEACommand.getDouble(elements[0]);
            lat = NMEACommand.getDouble(elements[1]);
            if (elements.Length > 2)
                hei = NMEACommand.getDouble(elements[2]);
            return true;
        }


        #region IPOILoader Members

        /// <summary>
        /// Import POI into bookmarks group
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

            if (doc.DocumentElement.Name != "kml")
                throw new ApplicationException("Not a valid KML file! Could not find kml root tag.");

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);

            { //retreive xmlns
                XmlNode xnsnode = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (xnsnode != null)
                    nsm.AddNamespace("kml", xnsnode.Value);
                else
                {
                    nsm.AddNamespace("kml", "");
                }
            }

            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//kml:Placemark", nsm);
            if (nlist.Count == 0)
                return 0;
            string gname = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ?
                            Path.GetFileNameWithoutExtension(fi.fileOrBuffer) :
                            "kml-buffer-" + DateTime.Now.ToShortDateString() +
                            "-" + DateTime.Now.ToShortTimeString());

            int count = subloadBookmarks(nlist, nsm, gname);
            if (fi.stype == GPS.TrackFileInfo.SourceType.StringBuffer)
                fi.fileOrBuffer = gname; //write our new name back to the buffer
            return count;
        }

        private int subloadBookmarks(XmlNodeList nlist, XmlNamespaceManager nsm, string groupname)
        {
            int count = 0;
            NMEA_RMC rmc = new NMEA_RMC();
            double lon, lat, hei;
            DateTime startTime = DateTime.Now;

            if (string.IsNullOrEmpty(groupname))
                groupname = "Imported";

            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            try
            {
                Bookmarks.POIGroup parentGroup = groupFactory.rootGroup.getSubGroupByPath(groupname);
                foreach (XmlNode node in nlist)
                {
                    XmlNode xnode = node.SelectSingleNode("./kml:Point/kml:coordinates", nsm);
                    if (xnode != null)
                    { //we have POI here, lets add it to our bookmarks
                        if (!splitKMLCoordTuple(xnode.InnerText, out lon, out lat, out hei))
                            continue;
                        Bookmark bmark = new Bookmark();
                        bmark.IsDbChange = false;

                        bmark.longitude = lon;
                        bmark.latitude = lat;
                        bmark.altitude = hei;

                        xnode = node.SelectSingleNode("./kml:name", nsm);
                        if (xnode == null)
                            continue;
                        bmark.Name = xnode.InnerText;
                        xnode = node.SelectSingleNode("./kml:description", nsm);
                        if (xnode != null)
                            bmark.Description = xnode.InnerText;

                        bmark.IsDbChange = true;
                        bmark.updateDB();

                        bmark.addLinkDB(parentGroup);

                        count++;
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                ncUtils.DBConnPool.singleton.commitThreadTransaction();
            }

            TimeSpan dTime = DateTime.Now - startTime;
            Program.Log("Loaded " + count + " POI's in seconds: " + dTime.TotalSeconds.ToString("F3"));
            return count;
        }

        public void exportPOIs(GMView.GPS.TrackFileInfo fileInfo, BookMarkFactory pFactory, 
                               LinkedList<GMView.Bookmarks.POIGroup> groups, List<Bookmark> poilist,
                               GMView.Bookmarks.POIGroup parentGroup)
        {
            throw new NotImplementedException("Export POI into KML files not implemented yet.");
        }

        #endregion
    }
}
