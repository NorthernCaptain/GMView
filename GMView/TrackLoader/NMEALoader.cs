using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ncGeo;

namespace GMView.TrackLoader
{
    /// <summary>
    /// Class loads NMEA protocol files into a track.
    /// Does not support loading POIs - there are no POIs in NMEA files
    /// </summary>
    public class NMEALoader: ITrackLoader, IPOILoader
    {
        #region ITrackLoader Members

        public void preLoad(GMView.GPS.TrackFileInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads NMEA messages into GPSTrack and return it.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public GPSTrack load(GMView.GPS.TrackFileInfo fi)
        {
            GPSTrack track = new GPSTrack();

            string tname;
            System.IO.TextReader reader;
            if (fi.stype == GPS.TrackFileInfo.SourceType.FileName)
            {
                reader = new System.IO.StreamReader(fi.fileOrBuffer);
                string dirname = Path.GetDirectoryName(fi.fileOrBuffer);
                tname = dirname.Substring(dirname.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                      + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
                track.track_name = "Track: " + tname;
                track.wayObject.name = "Route: " + Path.GetFileNameWithoutExtension(fi.fileOrBuffer);
            }
            else
            {
                reader = new System.IO.StringReader(fi.fileOrBuffer);
                tname = "NMEA buffer " + DateTime.Now.ToShortDateString()
                    + " " + DateTime.Now.ToShortTimeString();
                track.track_name = "Track: " + tname;
                track.wayObject.name = "Route: " + tname;
                tname += ".nmea";
            }

            string buf;

            try
            {
                while ((buf = reader.ReadLine()) != null)
                {
                    if (buf.Substring(0, 3) == "$GP" || buf.Substring(0, 6) == "$GPGSA")
                    {
                        NMEACommand cmd = NMEAThread.parse_command(new NMEAString(buf));
                        if (cmd.state == NMEACommand.Status.DataOK && cmd is NMEA_RMC)
                        {
                            NMEA_RMC rmc = cmd as NMEA_RMC;
                            track.addGPSDataInternal(rmc);
                        }
                    }
                }
            }
            finally
            {
                reader.Close();
            }

            if (track.trackPointData.Count == 0)
                throw new ApplicationException("This file does not have any tracks or routes! Check file content");

            track.fileName = (fi.stype == GPS.TrackFileInfo.SourceType.FileName ? fi.fileOrBuffer
                    : tname);
            track.calculateParameters();
            track.lastNonZeroPos = track.lastData;

            fi.fileOrBuffer = tname;

            return track;
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
                    if (first_line.Contains("$GP") || first_line.Contains("$PS"))
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

        #region IPOILoader Members

        public int importPOIs(GMView.GPS.TrackFileInfo fi, BookMarkFactory intoFactory, GMView.Bookmarks.POIGroupFactory groupFactory)
        {
            throw new NotImplementedException("NMEA files could not contain POI information");
        }

        public void exportPOIs(GMView.GPS.TrackFileInfo fileInfo, BookMarkFactory pFactory, LinkedList<GMView.Bookmarks.POIGroup> groups, List<Bookmark> poilist, GMView.Bookmarks.POIGroup parentGroup)
        {
            throw new NotImplementedException("NMEA files could not contain POI information");
        }

        #endregion
    }
}
