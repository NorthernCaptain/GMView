﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public class GPSTrackFactory
    {
        private static GPSTrackFactory instance = new GPSTrackFactory();
        public static GPSTrackFactory singleton
        {
            get { return instance; }
        }

        public static GPSTrackInfoForm.actionTrackDelegate onRemove;
        public static GPSTrackInfoForm.actionTrackDelegate onRecord;
        public static EventHandler onMenuClick;

        private List<GPSTrack> tracks = new List<GPSTrack>();
        private Dictionary<GPSTrack, GPSTrackInfoForm> infos = new Dictionary<GPSTrack,GPSTrackInfoForm>();

        public delegate void TrackChanged(GPSTrack gtrack);
        public event TrackChanged onCurrentTrackChanged;
        public event TrackChanged onRecordingTrackChanged;

        private GPSTrack curTrack; //current track
        public GPSTrack currentTrack
        {
            get { return curTrack; }
            set 
            { 
                curTrack = value;
                if (onCurrentTrackChanged != null)
                    onCurrentTrackChanged(curTrack);
            }
        }

        private GPSTrack recTrack;
        public GPSTrack recordingTrack
        {
            get { return recTrack; }
            set 
            { 
                recTrack = value;
                if (onRecordingTrackChanged != null)
                    onRecordingTrackChanged(recTrack);
            }
        }

        private SatelliteCollection curSatCollection;
        public SatelliteCollection currentSatCollection
        {
            get { return curSatCollection; }
            set
            {
                curSatCollection = value;
            }
        }

        private GPSTrackFactory()
        {
        }

        /// <summary>
        /// Return track information form
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public GPSTrackInfoForm infoForm(GPSTrack from)
        {
            return infos[from];
        }

        /// <summary>
        /// Adds new tracks to the factory and creates all neseccary objects
        /// </summary>
        /// <param name="gtrack"></param>
        public void addTrack(GPSTrack gtrack)
        {
            GPSTrackInfoForm nfo = new GPSTrackInfoForm(gtrack);
            tracks.Add(gtrack);
            infos.Add(gtrack, nfo);

            nfo.onRecord += onRecord;
            nfo.onRemove += onRemove;

            currentTrack = gtrack;
        }

        /// <summary>
        /// Removes track and its children from factory 
        /// </summary>
        /// <param name="gtrack"></param>
        public void delTrack(GPSTrack gtrack)
        {
            tracks.Remove(gtrack);
            infos.Remove(gtrack);
        }

        /// <summary>
        /// Removes all tracks from factory
        /// </summary>
        public void clearTracks(GPSTrackInfoForm.actionTrackDelegate runEachTrack)
        {
            foreach (KeyValuePair<GPSTrack, GPSTrackInfoForm> pair in infos)
            {
                runEachTrack(pair.Value);
            }
            tracks.Clear();
            infos.Clear();
        }


        /// <summary>
        /// Look through all tracks, find nearest point on each track to the given one,
        /// select and resturn nearest track and point.
        /// </summary>
        /// <param name="ctx"></param>
        public void findNearestTrack(ref GPSTrack.FindContext ctx)
        {
            GPSTrack.FindContext curctx = ctx;
            foreach (GPSTrack track in tracks)
            {
                curctx.init(ctx.lon, ctx.lat);
                track.findNearest(ref curctx);
                if (ctx.nearest == null ||
                    ctx.distance > curctx.distance)
                {
                    ctx = curctx;
                }
            }
        }

        /// <summary>
        /// Rebuild menu popup with our list of tracks. 
        /// First remove all old menu items and then add new ones
        /// </summary>
        /// <param name="tstrip"></param>
        public void rebuildMenuStrip(ToolStripItemCollection tstrip)
        {
            List<MenuItemObject> molist = new List<MenuItemObject>();

            foreach (ToolStripItem mi in tstrip)
            {
                MenuItemObject mo = mi as MenuItemObject;
                if (mo != null)
                    molist.Add(mo);
            }
            foreach (MenuItemObject mo in molist)
            {
                tstrip.Remove(mo);
            }

            foreach (GPSTrack gtr in tracks)
            {
                MenuItemObject mi = new MenuItemObject(infos[gtr]);
                mi.Click += onMenuClick;
                tstrip.Add(mi);
            }
        }

        /// <summary>
        /// Moves current track to the next track in tracks list
        /// </summary>
        /// <returns></returns>
        public GPSTrack stepToNextTrack()
        {
            int count = tracks.Count;
            int next = 0;
            for (int i = 0; i < count; i++)
            {
                if (tracks[i] == curTrack)
                {
                    next = i + 1;
                    if (next >= count)
                        next = 0;
                    break;
                }
            }

            if (next < count)
                currentTrack = tracks[next];
            return curTrack;
        }
    }
}
