using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ncGeo;

namespace GMView
{
    public partial class DownloadQueryForm : Form
    {
        private MapObject mapo;
        private ImgCollector.LoadTask loadqueue;
        private IGPSTrack track = null;
        private bool force = false;

        public DownloadQueryForm(MapObject imapo)
        {
            mapo = imapo;
            loadqueue = new ImgCollector.LoadTask(mapo);
            InitializeComponent();
            nearbyNT.Value = (mapo.tilePosNXNY.Width + 3) / 2;
        }

        private void cancelBut_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        /// <summary>
        /// Init square area for downloading tiles in background
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        public void init(double lon1, double lat1, double lon2, double lat2)
        {
            track = null;
            followTrackCB.Checked = false;
            followTrackCB.Enabled = false;
            nearbyNT.Enabled = false;

            fromLonNT.Value = (decimal)lon1;
            fromLatNT.Value = (decimal)lat1;
            toLonNT.Value = (decimal)lon2;
            toLatNT.Value = (decimal)lat2;

            int zlvl = Program.opt.cur_zoom_lvl + 1;
            if (zlvl >= Program.opt.max_zoom_lvl)
                return;

            createOziBut.Enabled = true;
            common_init();
        }

        /// <summary>
        /// Initializes download dialog for downloading along the path from the given track
        /// </summary>
        /// <param name="itrack"></param>
        public void init(IGPSTrack itrack)
        {
            track = itrack;
            followTrackCB.Checked = true;

            fromLonNT.Enabled = false;
            fromLatNT.Enabled = false;
            toLatNT.Enabled = false;
            toLonNT.Enabled = false;

            modeLbl.Text = track.ToString();

            int zlvl = Program.opt.cur_zoom_lvl + 1;
            if (zlvl >= Program.opt.max_zoom_lvl)
                return;

            createOziBut.Enabled = false;
            common_init();
        }

        /// <summary>
        /// Common initialization routine
        /// </summary>
        private void common_init()
        {
            int zlvl = Program.opt.cur_zoom_lvl + 1;
            if (zlvl >= Program.opt.max_zoom_lvl)
                return;

            if (zlvl < 8)
                zlvl = 8;

            for (int z = 8; z <= Program.opt.max_zoom_lvl; z++)
                zoomCheckList.SetItemChecked(Program.opt.max_zoom_lvl - z, z >= zlvl);

            switch (Program.opt.mapType)
            {
                case MapTileType.MapOnly:
                    needMapCb.Checked = true;
                    break;
                case MapTileType.SatMap:
                    needSatCb.Checked = true;
                    break;
                case MapTileType.SatStreet:
                    needStreetCb.Checked = true;
                    needSatCb.Checked = true;
                    break;
                case MapTileType.TerMap:
                    needTerrainCb.Checked = true;
                    break;
                case MapTileType.YandexMap:
                    needYamapCB.Checked = true;
                    break;
                case MapTileType.OSMMapnik:
                    needOSMCB.Checked = true;
                    break;
                case MapTileType.OSMRenderer:
                    needOSMRendCB.Checked = true;
                    break;
                default:
                    break;
            }

            recalcParams();
        }

        /// <summary>
        /// Recalculate tiles that we need to download according to the paramaters set in dialog
        /// </summary>
        private void recalcParams()
        {
            double lon1, lat1, lon2, lat2;
            bool was_limited = false;

            lon1 = (double)fromLonNT.Value;
            lat1 = (double)fromLatNT.Value;
            lon2 = (double)toLonNT.Value;
            lat2 = (double)toLatNT.Value;

            loadqueue.tiles.Clear();

            List<ncGeo.MapTileType> checkedTypes = new List<ncGeo.MapTileType>();

            if (needMapCb.Checked)
                checkedTypes.Add(ncGeo.MapTileType.MapOnly);
            if (needStreetCb.Checked)
                checkedTypes.Add(ncGeo.MapTileType.SatStreet);
            if (needSatCb.Checked)
                checkedTypes.Add(ncGeo.MapTileType.SatMap);
            if (needTerrainCb.Checked)
                checkedTypes.Add(ncGeo.MapTileType.TerMap);
            if (needYamapCB.Checked)
                checkedTypes.Add(ncGeo.MapTileType.YandexMap);
            if (needOSMCB.Checked)
                checkedTypes.Add(ncGeo.MapTileType.OSMMapnik);
            if (needOSMRendCB.Checked)
                checkedTypes.Add(ncGeo.MapTileType.OSMRenderer);


            force = forceDownloadCB.Checked;

            for(int idx = zoomCheckList.CheckedIndices.Count -1; idx>=0;idx--)
            {
                int zoom_idx = zoomCheckList.CheckedIndices[idx];
                int zlvl = Program.opt.max_zoom_lvl - zoom_idx;

                if (loadqueue.tiles.Count > 50000)
                {
                    zoomCheckList.SetItemChecked(zoom_idx, false);
                    was_limited = true;
                    continue;
                }

                foreach(ncGeo.MapTileType mtype in checkedTypes)
                {
                    if (track != null)
                        fillNearByTrackOnOneLevel(track, mtype, zlvl);
                    else
                        fillSquareOnOneLevel(lon1, lat1, lon2, lat2, mtype, zlvl);
                }

            }

            numTilesLb.Text = loadqueue.tiles.Count.ToString();
            sizeLb.Text = ((int)(loadqueue.tiles.Count * 10.23)).ToString();
            if (was_limited)
            {
                MessageBox.Show("Your selected area is too large and requires many files to be downloaded.\nSelection was limited in higher zooming levels.\nIf you want to download higher zoom levels, please select smaller area.",
                    "Selection area too large", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Fills one level of the map with tiles by given square area defines by lon, lat pairs
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <param name="type"></param>
        /// <param name="zlvl"></param>
        private void fillSquareOnOneLevel(double lon1, double lat1, double lon2, double lat2, 
                    ncGeo.MapTileType mtype, int zlvl)
        {
            Point nxny1, nxny2;
            BaseGeo lgeo = Program.opt.getGeoSystem(mtype);
            lgeo.getNXNYByLonLat(lon1, lat1, zlvl, out nxny1);
            lgeo.getNXNYByLonLat(lon2, lat2, zlvl, out nxny2);

            for (int x = nxny1.X; x <= nxny2.X; x++)
            {
                for (int y = nxny1.Y; y <= nxny2.Y; y++)
                {
                    ImgTile tile = Program.opt.newImgTile(x, y, zlvl, mtype);
                    if (force)
                        tile.status = ImgStatus.ForceDownload;

                    //if (!tile.haveOnDisk())
                    loadqueue.tiles.Enqueue(tile);
                }
            }
        }

        private void needMapCb_CheckedChanged(object sender, EventArgs e)
        {
            recalcParams();
        }

        private void okBut_Click(object sender, EventArgs e)
        {
            if (loadqueue.tiles.Count == 0)
            {
                MessageBox.Show("There is nothing to download. You need to select at least one tile.", "Nothing to do");
            }
            else
            {
                recalcParams();
                mapo.schedDownloadTask(loadqueue);
                loadqueue = null;
                this.Dispose();
            }
        }

        private void copyToBut_Click(object sender, EventArgs e)
        {
            if (loadqueue.tiles.Count == 0)
            {
                MessageBox.Show("There is nothing to copy. You need to select at least one tile.", "Nothing to do");
            }
            else
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    recalcParams();
                    loadqueue.type = ImgCollector.LoadTask.Type.copyTask;
                    loadqueue.copyTo = folderBrowserDialog.SelectedPath;
                    mapo.schedDownloadTask(loadqueue);
                    loadqueue = null;
                    this.Dispose();
                }
            }
        }

        private void oziImageBut_Click(object sender, EventArgs e)
        {
            if (loadqueue.tiles.Count == 0)
            {
                MessageBox.Show("There is nothing to create. You need to select at least one tile.", "Nothing to do");
            }
            else
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    recalcParams();
                    loadqueue.type = ImgCollector.LoadTask.Type.imageMerge;
                    loadqueue.copyTo = saveFileDialog.FileName;
                    loadqueue.oziexp = new TrackLoader.OziMapExporter(loadqueue.copyTo,
                                                                      (double)fromLonNT.Value,
                                                                      (double)fromLatNT.Value,
                                                                      (double)toLonNT.Value,
                                                                      (double)toLatNT.Value);
                    mapo.schedDownloadTask(loadqueue);
                    loadqueue = null;
                    this.Dispose();
                }
            }
        }


        /// <summary>
        /// Check the cache if point was already added and if not then adds it to the load queue
        /// </summary>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="mtype"></param>
        /// <param name="zlvl"></param>
        /// <param name="cacheSet"></param>
        /// <returns></returns>
        private bool addPoint(int rx, int ry, ncGeo.MapTileType mtype, int zlvl, 
            Dictionary<ulong, int> cacheSet)
        {
            ulong hash = ImgTile.getHash(rx, ry, zlvl, mtype);
            if (cacheSet.ContainsKey(hash))
                return false;
            cacheSet.Add(hash, 1);

            ImgTile tile = Program.opt.newImgTile(rx, ry, zlvl, mtype);
            if (force)
                tile.status = ImgStatus.ForceDownload;

            loadqueue.tiles.Enqueue(tile);
            return true;
        }

        /// <summary>
        /// Fills square area around the given point
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="mtype"></param>
        /// <param name="zlvl"></param>
        /// <param name="cachedSet"></param>
        /// <param name="nearByTiles"></param>
        private void fillEdgePoint(Point xy, ncGeo.MapTileType mtype, int zlvl, 
                Dictionary<ulong, int> cachedSet, int nearByTiles)
        {
            int nearByTiles2 = nearByTiles * 2 + 1;
            for (int y = 0; y < nearByTiles2; y++)
            {
                for (int x = 0; x < nearByTiles2; x++)
                {
                    addPoint(xy.X + x - nearByTiles, xy.Y + y - nearByTiles, mtype, zlvl, cachedSet);
                }
            }
        }

        /// <summary>
        /// Follow track path, calculate tiles and fills downloading queue for one level of the map
        /// </summary>
        /// <param name="track"></param>
        /// <param name="mtype"></param>
        /// <param name="zlvl"></param>
        private void fillNearByTrackOnOneLevel(IGPSTrack track, ncGeo.MapTileType mtype, int zlvl )
        {
            LinkedList<NMEA_LL> points = track.trackPointData;
            BaseGeo lgeo = Program.opt.getGeoSystem(mtype);

            if(track.countPoints < 2)
                return;

            int nearByTiles = (int)nearbyNT.Value;
            int nearByTiles2 = nearByTiles * 2 + 1;

            Dictionary<ulong, int> cachedSet = new Dictionary<ulong, int>();

            Point oldxy;
            LinkedListNode<NMEA_LL> oldnode, curnode;
            oldnode = points.First;

            lgeo.getNXNYByLonLat(oldnode.Value.lon, oldnode.Value.lat, zlvl, out oldxy);
            curnode = oldnode.Next;
            fillEdgePoint(oldxy, mtype, zlvl, cachedSet, nearByTiles);

            while(curnode != null)
            {
                Point curxy;
                lgeo.getNXNYByLonLat(curnode.Value.lon, curnode.Value.lat, zlvl, out curxy);

                //Skip points with the same tile coordinates as old one
                if(curxy.Equals(oldxy))
                {
                    curnode = curnode.Next;
                    continue;
                }

                int dx = curxy.X - oldxy.X;
                int dy = curxy.Y - oldxy.Y;
                int adx = Math.Abs(dx);
                int ady = Math.Abs(dy);
                int stepx = dx < 0 ? -1 : 1;
                int stepy = dy < 0 ? -1 : 1;
                int adx2 = adx / 2;
                int ady2 = ady / 2;

                if(adx > ady)
                { //Horizontal line
                    for(int x = 1; x < adx; x++)
                    {
                        int rx = oldxy.X + stepx * x;
                        int ry = oldxy.Y + stepy * (ady * x + adx2) / adx;

                        for(int y = 0; y < nearByTiles2; y++)
                        {
                            addPoint(rx, ry - nearByTiles + y, mtype, zlvl, cachedSet);
                        }
                    }
                } else
                { //Vertical line
                    for(int y = 1; y < ady; y++)
                    {
                        int ry = oldxy.Y + stepy * y;
                        int rx = oldxy.X + stepx * (adx * y + ady2) / ady;

                        for (int x = 0; x < nearByTiles2; x++)
                        {
                            addPoint(rx - nearByTiles + x, ry, mtype, zlvl, cachedSet);
                        }
                    }
                }

                fillEdgePoint(curxy, mtype, zlvl, cachedSet, nearByTiles);

                oldxy = curxy;
                curnode = curnode.Next;
            }
        }

        private void allNoneCB_CheckedChanged(object sender, EventArgs e)
        {
            int zlvl = Program.opt.cur_zoom_lvl + 1;
            if (zlvl >= Program.opt.max_zoom_lvl)
                return;

            if (zlvl < 8)
                zlvl = 8;

            for (int z = zlvl; z <= Program.opt.max_zoom_lvl; z++)
                zoomCheckList.SetItemChecked(Program.opt.max_zoom_lvl - z, allNoneCB.Checked);
            recalcParams();
        }

    }
}
