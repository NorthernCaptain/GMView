using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Globalization;
using ncGeo;

namespace GMView.TrackLoader
{
    public class OziMapExporter: IExporter
    {
        private double lon1, lat1, lon2, lat2;
        private string  filepath;

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
                finalizeWork();
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
                image = new Bitmap(len << 8, hei << 8, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                graphics = Graphics.FromImage(image);
                graphics.Clear(Color.DimGray);
            }
            catch (System.Exception ex)
            {
                image = null;	
            }
        }

        public void finalizeWork()
        {
            if (image == null)
                return;

            string path = Path.GetDirectoryName(filepath);
            string fname = Path.GetFileNameWithoutExtension(filepath);
            string ext = Path.GetExtension(filepath);

            fname += "-" + lastTile.zoom.ToString();

            string fullname = Path.Combine(path, fname);

            string fullImageName = fullname + ext;
            try
            {
                File.Delete(fullImageName);
                if(ext.ToLower().Equals(@".jpg"))
                    image.Save(fullImageName,System.Drawing.Imaging.ImageFormat.Jpeg);
                else
                    image.Save(fullImageName, System.Drawing.Imaging.ImageFormat.Png);
                writeOziMapFile(fullname + @".map", fullImageName);
            }
            catch (System.Exception ex)
            {
            	
            }
            image.Dispose();
            image = null;
        }

        private void writeOziMapFile(string fullname, string imagename)
        {
            using (StreamWriter writer = new StreamWriter(fullname, false, Encoding.Default))
            {
                writer.WriteLine("OziExplorer Map Data File Version 2.2");
                writer.WriteLine(Path.GetFileNameWithoutExtension(fullname));
                writer.WriteLine(imagename);
                writer.WriteLine("1 ,Map Code,");
                writer.WriteLine("WGS 84,WGS 84,   0.0000,   0.0000,WGS 84");
                writer.WriteLine("Reserved 1");
                writer.WriteLine("Reserved 2");
                writer.WriteLine("Magnetic Variation,,,E");
                writer.WriteLine("Map Projection,(UTM) Universal Transverse Mercator,PolyCal,No,AutoCalOnly,No,BSBUseWPX,No");

                BaseGeo geo = Program.opt.getGeoSystem(lastTile.map_type);
                geo.zoomLevel = lastTile.zoom;

                Point xy1 = new Point(startx << 8, starty << 8);
                Point xy2 = new Point(((startx + len) << 8) - 1, ((starty + hei) << 8) - 1);

                double plon1, plat1;
                geo.getLonLatByXY(xy1, out plon1, out plat1);

                double plon2, plat2;
                geo.getLonLatByXY(xy2, out plon2, out plat2);

                writePoint(writer, "Point01",             0,             0, plon1, plat1);
                writePoint(writer, "Point02", xy2.X - xy1.X,             0, plon2, plat1);
                writePoint(writer, "Point03", xy2.X - xy1.X, xy2.Y - xy1.Y, plon2, plat2);
                writePoint(writer, "Point04",             0, xy2.Y - xy1.Y, plon1, plat2);

                Point xy3 = new Point((xy1.X + xy2.X) / 2, (xy1.Y + xy2.Y) / 2);
                geo.getLonLatByXY(xy3, out plon1, out plat1);
                writePoint(writer, "Point05", xy3.X - xy1.X, xy3.Y - xy1.Y, plon1, plat1);

                int x4 = (xy2.X - xy1.X) / 4;
                int y4 = (xy2.Y - xy1.Y) / 4;

                xy3 = new Point(xy1.X + x4, xy1.Y + y4);
                geo.getLonLatByXY(xy3, out plon1, out plat1);
                writePoint(writer, "Point06", xy3.X - xy1.X, xy3.Y - xy1.Y, plon1, plat1);

                xy3 = new Point(xy1.X + 3*x4, xy1.Y + y4);
                geo.getLonLatByXY(xy3, out plon1, out plat1);
                writePoint(writer, "Point07", xy3.X - xy1.X, xy3.Y - xy1.Y, plon1, plat1);

                xy3 = new Point(xy1.X + 3*x4, xy1.Y + 3*y4);
                geo.getLonLatByXY(xy3, out plon1, out plat1);
                writePoint(writer, "Point08", xy3.X - xy1.X, xy3.Y - xy1.Y, plon1, plat1);

                xy3 = new Point(xy1.X + x4, xy1.Y + 3*y4);
                geo.getLonLatByXY(xy3, out plon1, out plat1);
                writePoint(writer, "Point09", xy3.X - xy1.X, xy3.Y - xy1.Y, plon1, plat1);

                for (int i = 10; i <= 30; i++)
                {
                    writer.WriteLine("Point" + i.ToString() + ",xy, , ,in, deg, , ,N, , ,W, grid, , , ,N");
                }

                writer.WriteLine("Projection Setup,,,,,,,,,,");
                writer.WriteLine("Map Feature = MF ; Map Comment = MC     These follow if they exist");
                writer.WriteLine("Track File = TF      These follow if they exist");
                writer.WriteLine("Moving Map Parameters = MM?    These follow if they exist");
                writer.WriteLine("MM0,Yes");
                writer.WriteLine("MMPNUM,4");
                writer.WriteLine("MMPXY,1,0,0");
                writer.WriteLine("MMPXY,2,{0},{1}", xy2.X - xy1.X, 0);
                writer.WriteLine("MMPXY,3,{0},{1}", xy2.X - xy1.X, xy2.Y - xy1.Y);
                writer.WriteLine("MMPXY,4,{0},{1}", 0, xy2.Y - xy1.Y);
                writer.WriteLine("MMPLL,1,{0,10},{1,10}", 
                        lon1.ToString("F6", CultureInfo.InvariantCulture),
                        lat1.ToString("F6",CultureInfo.InvariantCulture)
                        );
                writer.WriteLine("MMPLL,2,{0,10},{1,10}",
                        lon2.ToString("F6", CultureInfo.InvariantCulture),
                        lat1.ToString("F6", CultureInfo.InvariantCulture)
                        );
                writer.WriteLine("MMPLL,3,{0,10},{1,10}",
                        lon2.ToString("F6", CultureInfo.InvariantCulture),
                        lat2.ToString("F6", CultureInfo.InvariantCulture)
                        );
                writer.WriteLine("MMPLL,4,{0,10},{1,10}",
                        lon1.ToString("F6", CultureInfo.InvariantCulture),
                        lat2.ToString("F6", CultureInfo.InvariantCulture)
                        );
                writer.WriteLine("MM1B,{0}", 
                            (ncGeo.CommonGeo.getDistanceByLonLat2(lon1, lat1, lon2, lat2)/
                             Math.Sqrt((xy2.X - xy1.X)*(xy2.X - xy1.X) + (xy2.Y - xy1.Y)*(xy2.X - xy1.X))*1000.0).ToString("F6", CultureInfo.InvariantCulture));
                writer.WriteLine("MOP,Map Open Position,{0},{1}", (xy2.X - xy1.X +1)/2, (xy2.Y - xy1.Y +1)/2);
                writer.WriteLine("IWH,Map Image Width/Height,{0},{1}", xy2.X - xy1.X + 1, xy2.Y - xy1.Y + 1);

                writer.Close();
            }             
        }

        /// <summary>
        /// Writes point info into map file. Point info is a line that looks like this:
        /// Point01,xy,  100,  455,in, deg,  61, 30.0000,N,   8, 35.0000,E, grid, , , ,N
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="point"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        private void writePoint(StreamWriter writer, string point, int x, int y, double lon, double lat)
        {
            double alon = Math.Abs(lon);
            double alat = Math.Abs(lat);

            StringBuilder buf = new StringBuilder(point);
            buf.Append(",xy,");
            buf.AppendFormat("{0,6},{1,6}, in, deg,", x, y);
            buf.AppendFormat(CultureInfo.InvariantCulture, "{0,4}, {1,10:F5},", (int)alat, 60.0 * (alat - Math.Truncate(alat)));
            buf.Append(lat >= 0 ? "N" : "S");
            buf.AppendFormat(CultureInfo.InvariantCulture, ",{0,4}, {1,10:F5},", (int)alon, 60.0 * (alon - Math.Truncate(alon)));
            buf.Append(lon >= 0 ? "E" : "W");
            buf.Append(", grid, , , ,N");
            writer.WriteLine(buf.ToString());
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

        #region IExporter Members

        public void startWork()
        {
            // nothing to do here for Ozi
        }

        #endregion
    }
}
