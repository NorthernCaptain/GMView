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

        public GPSTrack load(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
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
                            "kml-buffer-" + DateTime.Now.ToShortDateString());

            return subloadBookmarks(nlist, nsm, gname);
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
