using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using ncGeo;

namespace GMView.TrackLoader
{
    public class OruxMapExporter: IExporter
    {
        private const int oruxLen = 512;
        private const int oruxHei = 512;

        private double  lon1, lat1, lon2, lat2;
        private string  filepath;
        private string  filename;
        private string  currentpathname;
        private string  currentname;

        private class OruxImage: IDisposable
        {
            internal Bitmap image = null;
            internal int imageMask = 0;
            internal Graphics graphics;
            internal int imgPosX;
            internal int imgPosY;

            internal void drawTile(ImgTile tile, int dx, int dy)
            {
                graphics.DrawImage(tile.img, dx << 8, dy << 8, tile.img.Width, tile.img.Height);
                imageMask ++;
            }

            internal bool isDone()
            {
                return imageMask == 4;
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (image != null)
                {
                    graphics.Dispose();
                    image.Dispose();
                    image = null;
                }
            }

            #endregion
        }

        private Dictionary<int, OruxImage> images = new Dictionary<int, OruxImage>();
        private ImgTile lastTile = null;

        private int startx, starty;
        private int len, hei;

        public OruxMapExporter(string fname,
                                double ilon1,
                                double ilat1,
                                double ilon2,
                                double ilat2)
        {
            filepath = Path.GetDirectoryName(fname);
            filename = Path.GetFileNameWithoutExtension(fname);
            filepath = Path.Combine(filepath, filename);

            lon1 = ilon1;
            lon2 = ilon2;
            lat1 = ilat1;
            lat2 = ilat2;
        }


        #region IExporter Members

        public void startWork()
        {
            images.Clear();
            try
            {
                Directory.CreateDirectory(filepath);
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        public void processOneTile(ImgTile tile)
        {
            if (lastTile == null || lastTile.zoom != tile.zoom)
            {
                finalizeLayer();
                initNewLayer(tile);
            }

            int dx = tile.x - startx;
            int dy = tile.y - starty;

            int oruxX = dx / 2;
            int oruxY = dy / 2;
            int oruxDX = dx % 2;
            int oruxDY = dy % 2;

            OruxImage img = provideImage(oruxX, oruxY);

            if (dx < 0 || dy < 0 || img == null)
                return;

            lastTile = tile;

            tile.loadFromDisk(false);

            if (tile.status != ImgStatus.InMemory)
                return;

            img.drawTile(tile, oruxDX, oruxDY);

            if (img.isDone())
            {
                flushImage(img);
                removeImage(img);
            }
        }

        private void finalizeLayer()
        {
            foreach (KeyValuePair<int, OruxImage> img in images)
            {
                flushImage(img.Value);
                img.Value.Dispose();
            }
            images.Clear();
        }

        public void finalizeWork()
        {
            finalizeLayer();
        }


        private void initNewLayer(ImgTile tile)
        {
            BaseGeo geo = Program.opt.getGeoSystem(tile.map_type);
            geo.zoomLevel = tile.zoom;

            Point xy1;
            geo.getNXNYByLonLat(lon1, lat1, tile.zoom, out xy1);

            Point xy2;
            geo.getNXNYByLonLat(lon2, lat2, tile.zoom, out xy2);

            startx = xy1.X;
            starty = xy1.Y;

            currentname = filename + " " + tile.zoom.ToString("00");
            currentpathname = Path.Combine(filepath, currentname);
            try
            {
                Directory.CreateDirectory(currentpathname);
                Directory.CreateDirectory(Path.Combine(currentpathname, "set"));
            }
            catch (System.Exception ex)
            {

            }
        }

        private OruxImage provideImage(int oruxX, int oruxY)
        {
            int key = oruxY << 10 | oruxX;
            OruxImage img;
            if (images.TryGetValue(key, out img))
                return img;
            img = new OruxImage();
            img.image = new Bitmap(oruxLen, oruxHei, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            img.imgPosX = oruxX;
            img.imgPosY = oruxY;
            img.graphics = Graphics.FromImage(img.image);
            img.graphics.Clear(Color.DarkGray);
            images.Add(key, img);
            return img;
        }

        private void flushImage(OruxImage img)
        {
            string path = Path.Combine(currentpathname, "set");
            string fname = currentname + "_" + img.imgPosX.ToString() + "_" + img.imgPosY.ToString() + ".omc2";
            path = Path.Combine(path, fname);

            try
            {
                img.image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        private void removeImage(OruxImage img)
        {
            int key = img.imgPosY << 10 | img.imgPosX;
            images.Remove(key);
            img.Dispose();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (KeyValuePair<int, OruxImage> img in images)
            {
                img.Value.Dispose();
            }
            images.Clear();
        }

        #endregion
    }
}
