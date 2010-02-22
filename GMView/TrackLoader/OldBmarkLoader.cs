using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ncGeo;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Loads old fashioned bookmark list from xml file.
    /// We need this only for restoring bookmarks for Knowhere prior to 1.7.0 version
    /// </summary>
    public class OldBmarkLoader: ITrackLoader, IPOILoader
    {
        private Bookmarks.POIGroupFactory groupFactory;
        private BookMarkFactory poiFactory;

        #region IPOILoader Members

        public int importPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory intoFactory, GMView.Bookmarks.POIGroupFactory groupFactory)
        {
            XmlDocument doc = fi.openXml();

            poiFactory = intoFactory;
            this.groupFactory = groupFactory;

            if (doc.DocumentElement.Name != "BookmarkList")
                throw new ApplicationException("Not a valid Bmark file! Could not find root tag.");

            XmlNamespaceManager nsm = ncUtils.XmlHelper.getNSMforDoc(doc, "old");

            XmlNodeList nlist = doc.DocumentElement.SelectNodes("//old:bmark", nsm);
            if (nlist.Count == 0)
                return 0;

            string gname = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ?
                Path.GetFileNameWithoutExtension(fi.fileOrBuffer) :
                "bmark-buffer-" + DateTime.Now.ToShortDateString() +
                "-" + DateTime.Now.ToShortTimeString());

            int count = subloadBookmarks(nlist, nsm, gname);
            if (fi.stype == GPS.TrackFileInfo.SourceType.StringBuffer)
                fi.fileOrBuffer = gname;
            return count;

        }

        /// <summary>
        /// Sub routine for loading POIs itself
        /// </summary>
        /// <param name="nlist"></param>
        /// <param name="nsm"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        private int subloadBookmarks(XmlNodeList nlist, XmlNamespaceManager nsm, string groupname)
        {
            int count = 0;
            NMEA_RMC rmc = new NMEA_RMC();
            DateTime startTime = DateTime.Now;

            if (string.IsNullOrEmpty(groupname))
                groupname = "Imported";

            ncUtils.DBConnPool.singleton.beginThreadTransaction();
            try
            {
                Bookmarks.POIGroup parentGroup = groupFactory.rootGroup.getSubGroupByPath(groupname);
                foreach (XmlNode node in nlist)
                {
                    Bookmark bmark = new Bookmark();
                    bmark.IsDbChange = false;

                    bmark.lat = NMEACommand.getDouble(node.Attributes.GetNamedItem("lat").Value);
                    bmark.lon = NMEACommand.getDouble(node.Attributes.GetNamedItem("lon").Value);

                    string datestring = node.Attributes.GetNamedItem("created").Value;
                    if (!string.IsNullOrEmpty(datestring))
                    {
                        try
                        {
                            rmc.utc_time = DateTime.Parse(datestring).ToUniversalTime();
                        }
                        catch
                        {
                            rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();
                        }

                    }
                    else
                        rmc.utc_time = DateTime.MinValue.AddYears(2000).ToUniversalTime();

                    bmark.Created = rmc.utc_time.ToLocalTime();

                    bmark.Name = node.Attributes.GetNamedItem("name").Value;

                    XmlNode xnode = node.SelectSingleNode("./old:comment", nsm);
                    if (xnode != null)
                        bmark.Description = xnode.InnerText.Trim();

                    bmark.group = node.Attributes.GetNamedItem("group_name").Value;

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

            TimeSpan dTime = DateTime.Now - startTime;
            Program.Log("Loaded " + count + " POI's in seconds: " + dTime.TotalSeconds.ToString("F3"));

            return count;
        }




        public void exportPOIs(GMView.GPS.TrackFileInfo fileInfo, BookMarkFactory pFactory, LinkedList<GMView.Bookmarks.POIGroup> groups, List<Bookmark> poilist, GMView.Bookmarks.POIGroup parentGroup)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFormatLoader Members

        public bool isOurFormat(GMView.GPS.TrackFileInfo info)
        {
            string first_line = string.Empty;

            System.IO.TextReader reader = info.openReader();

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    first_line = reader.ReadLine();
                    if (string.IsNullOrEmpty(first_line))
                        continue;
                    if (first_line.Contains("<BookmarkList"))
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

        #region ITrackLoader Members

        public GMView.GPS.TrackFileInfo preLoad(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        public GPSTrack load(GMView.GPS.TrackFileInfo info, BookMarkFactory poiFact, GMView.Bookmarks.POIGroupFactory igroupFact)
        {
            throw new NotImplementedException();
        }

        public void save(GPSTrack track, GMView.GPS.TrackFileInfo fi, BookMarkFactory poiFact, GMView.Bookmarks.POIGroupFactory igroupFact)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new OldBmarkLoader();
        }

        #endregion

        #region ITrackLoader Members

        public ncFileControls.FileFilter trackLoadFileFilter()
        {
            return null;
        }

        public ncFileControls.FileFilter trackSaveFileFilter()
        {
            return null;
        }

        #endregion

        #region IPOILoader Members

        public ncFileControls.FileFilter poiLoadFileFilter()
        {
            return new ncFileControls.FileFilter("Old fashioned Knowhere POI file (*.xml)", "*.xml");
        }

        public ncFileControls.FileFilter poiSaveFileFilter()
        {
            return null;
        }

        #endregion
    }
}
