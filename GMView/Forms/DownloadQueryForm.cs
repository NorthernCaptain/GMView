using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMView
{
    public partial class DownloadQueryForm : Form
    {
        private MapObject mapo;
        private ImgCollector.LoadTask loadqueue;

        public DownloadQueryForm(MapObject imapo)
        {
            mapo = imapo;
            loadqueue = new ImgCollector.LoadTask(mapo);
            InitializeComponent();
        }

        private void cancelBut_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void init(double lon1, double lat1, double lon2, double lat2)
        {
            fromLonNT.Value = (decimal)lon1;
            fromLatNT.Value = (decimal)lat1;
            toLonNT.Value = (decimal)lon2;
            toLatNT.Value = (decimal)lat2;
            int zlvl = Program.opt.cur_zoom_lvl + 1;
            if (zlvl >= Program.opt.max_zoom_lvl)
                return;

            if (zlvl < 5)
                zlvl = 5;

            for (int z = 5; z <= Program.opt.max_zoom_lvl; z++)
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
                default:
                    break;
            }

            recalcParams();
        }

        private void recalcParams()
        {
            double lon1, lat1, lon2, lat2;
            bool was_limited = false;

            lon1 = (double)fromLonNT.Value;
            lat1 = (double)fromLatNT.Value;
            lon2 = (double)toLonNT.Value;
            lat2 = (double)toLatNT.Value;


            loadqueue.tiles.Clear();

            BaseGeo geo = Program.opt.getGeoSystem();
            foreach (int zoom_idx in zoomCheckList.CheckedIndices)
            {
                int zlvl = Program.opt.max_zoom_lvl - zoom_idx;
                Point nxny1, nxny2;

                geo.getNXNYByLonLat(lon1, lat1, zlvl, out nxny1);
                geo.getNXNYByLonLat(lon2, lat2, zlvl, out nxny2);

                if ((nxny2.X - nxny1.X) * (nxny2.Y - nxny1.Y) > 40000)
                {
                    zoomCheckList.SetItemChecked(zoom_idx, false);
                    was_limited = true;
                    continue;
                }

                if (needMapCb.Checked)
                {
                    BaseGeo lgeo = Program.opt.getGeoSystem(MapTileType.MapOnly);
                    lgeo.getNXNYByLonLat(lon1, lat1, zlvl, out nxny1);
                    lgeo.getNXNYByLonLat(lon2, lat2, zlvl, out nxny2);
                    fillOneLevel(nxny1, nxny2, zlvl, MapTileType.MapOnly);
                }
                if (needStreetCb.Checked)
                {
                    BaseGeo lgeo = Program.opt.getGeoSystem(MapTileType.SatStreet);
                    lgeo.getNXNYByLonLat(lon1, lat1, zlvl, out nxny1);
                    lgeo.getNXNYByLonLat(lon2, lat2, zlvl, out nxny2);
                    fillOneLevel(nxny1, nxny2, zlvl, MapTileType.SatStreet);
                    needSatCb.Checked = true;
                }
                if (needSatCb.Checked)
                {
                    BaseGeo lgeo = Program.opt.getGeoSystem(MapTileType.SatMap);
                    lgeo.getNXNYByLonLat(lon1, lat1, zlvl, out nxny1);
                    lgeo.getNXNYByLonLat(lon2, lat2, zlvl, out nxny2);
                    fillOneLevel(nxny1, nxny2, zlvl, MapTileType.SatMap);
                }
                if (needTerrainCb.Checked)
                {
                    BaseGeo lgeo = Program.opt.getGeoSystem(MapTileType.TerMap);
                    lgeo.getNXNYByLonLat(lon1, lat1, zlvl, out nxny1);
                    lgeo.getNXNYByLonLat(lon2, lat2, zlvl, out nxny2);
                    fillOneLevel(nxny1, nxny2, zlvl, MapTileType.TerMap);
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

        private void fillOneLevel(Point nxny1, Point nxny2, int zlvl, MapTileType type)
        {
            for (int x = nxny1.X; x <= nxny2.X; x++)
            {
                for (int y = nxny1.Y; y <= nxny2.Y; y++)
                {
                    ImgTile tile = Program.opt.newImgTile(x, y, zlvl, type);
                    //if (!tile.haveOnDisk())
                        loadqueue.tiles.Enqueue(tile);
                }
            }
        }

        private void needMapCb_CheckedChanged(object sender, EventArgs e)
        {
            recalcParams();
        }

        private void needSatCb_CheckedChanged(object sender, EventArgs e)
        {
            recalcParams();
        }

        private void needStreetCb_CheckedChanged(object sender, EventArgs e)
        {
            recalcParams();
        }

        private void needTerrainCb_CheckedChanged(object sender, EventArgs e)
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
    }
}
