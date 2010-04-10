using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using ncGeo;

namespace GMView.TrackLoader
{
    public class OziMapExporter: IDisposable
    {
        private double lon1, lat1, lon2, lat2;
        string  filepath;

        private Bitmap image = null;
        private ImgTile lastTile = null;

        private int startx, starty;
        private int len, hei;
        private Graphics graphics;

        public OziMapExporter(string fname,
                                double ilon1,
                                double ilat1,
                                double ilon2,
                                double ilat2)
        {
            filepath = fname;
            lon1 = ilon1;
            lon2 = ilon2;
            lat1 = ilat1;
            lat2 = ilat2;
        }

        public void processOneTile(ImgTile tile)
        {
            if (lastTile == null || lastTile.zoom != tile.zoom)
            {
                writeImage();
                createImage(tile);
            }

            int dx = tile.x - startx;
            int dy = tile.y - starty;

            if (dx < 0 || dy < 0 || dx >= len || dy >= hei || image == null)
                return;

            lastTile = tile;

            tile.loadFromDisk(false);

            if (tile.status != ImgStatus.InMemory)
                return;

            dx <<= 8;
            dy <<= 8;

            graphics.DrawImage(tile.img, dx, dy, tile.img.Width, tile.img.Height);
        }

        private void createImage(ImgTile tile)
        {
            BaseGeo geo = Program.opt.getGeoSystem(tile.map_type);
            geo.zoomLevel = tile.zoom;

            Point xy1;
            geo.getNXNYByLonLat(lon1, lat1, tile.zoom, out xy1);

            Point xy2;
            geo.getNXNYByLonLat(lon2, lat2, tile.zoom, out xy2);

            startx = xy1.X;
            starty = xy1.Y;

            len = xy2.X - startx + 1;
            hei = xy2.Y - starty + 1;

            if (len > 30)
                len = 30;

            if (hei > 30)
                hei = 30;
            try
            {
                image = new Bitmap(len << 8, hei << 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                graphics = Graphics.FromImage(image);
                graphics.Clear(Color.DimGray);
            }
            catch (System.Exception ex)
            {
                image = null;	
            }
        }

        public void writeImage()
        {
            if (image == null)
                return;

            string path = Path.GetDirectoryName(filepath);
            string fname = Path.GetFileNameWithoutExtension(filepath);
            string ext = Path.GetExtension(filepath);

            fname += "-" + lastTile.zoom.ToString();

            string fullname = Path.Combine(path, fname + ext);

            try
            {
                File.Delete(fullname);
                image.Save(fullname);
            }
            catch (System.Exception ex)
            {
            	
            }
            image.Dispose();
            image = null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
        }

        #endregion
    }
}
