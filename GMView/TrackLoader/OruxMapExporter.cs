using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using ncGeo;
using System.Xml;

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

        private int maxLvlX;
        private int maxLvlY;
        private int minZoom;

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

        private BaseGeo geo;

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
            minZoom = 100;
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

            if (maxLvlX < oruxX)
                maxLvlX = oruxX;
            if (maxLvlY < oruxY)
                maxLvlY = oruxY;

            if (minZoom > tile.zoom)
                minZoom = tile.zoom;

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

            writeLayerFile();
        }

        public void finalizeWork()
        {
            finalizeLayer();
            writeMainFile();
        }


        private void initNewLayer(ImgTile tile)
        {
            geo = Program.opt.getGeoSystem(tile.map_type);
            geo.zoomLevel = tile.zoom;

            startx = tile.x;
            starty = tile.y;

            maxLvlX = maxLvlY = 0;

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


        private void writeMainFile()
        {
            XmlTextWriter writer = null;

            if (currentname == null)
                return;

            string fname = Path.Combine(filepath, filename + ".otrk2.xml");
            try
            {
                writer = new XmlTextWriter(fname, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("OruxTracker");
                writer.WriteAttributeString("versionCode", "2.1");
                writer.WriteAttributeString("xmlns:orux", @"http://oruxtracker.com/app/res/calibration");


                {
                    writer.WriteStartElement("MapCalibration");
                    writer.WriteAttributeString("layers", "true");
                    writer.WriteAttributeString("layerLevel", "0");
                    writer.WriteStartElement("MapName");
                    writer.WriteCData(filename);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch (System.Exception ex)
            {
                Program.Err("Could not create main OruxMaps xml file: " + fname + "\nError:" + ex);
                if (writer != null)
                    writer.Close();
            }
        }

        private void writeLayerFile()
        {
            XmlTextWriter writer = null;

            if (currentname == null)
                return;

            string fname = Path.Combine(currentpathname, currentname + ".otrk2.xml" );
            try
            {
                writer = new XmlTextWriter(fname, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("OruxTracker");
                writer.WriteAttributeString("versionCode", "2.1");
                writer.WriteAttributeString("xmlns:orux", @"http://oruxtracker.com/app/res/calibration");


                {
                    writer.WriteStartElement("MapCalibration");
                    writer.WriteAttributeString("layers", "false");
                    writer.WriteAttributeString("layerLevel", geo.zoomLevel.ToString());
                    writer.WriteStartElement("MapName");
                    writer.WriteCData(currentname);
                    writer.WriteEndElement();

                    {
                        writer.WriteStartElement("MapChunks");
                        writer.WriteAttributeString("xMax", (maxLvlX + 1).ToString());
                        writer.WriteAttributeString("yMax", (maxLvlY + 1).ToString());
                        writer.WriteAttributeString("datum", "WGS84");
                        writer.WriteAttributeString("projection", "Mercator");
                        writer.WriteAttributeString("img_height", "512");
                        writer.WriteAttributeString("img_width", "512");
                        writer.WriteAttributeString("file_name", currentname);
                        writer.WriteEndElement();

                        writer.WriteStartElement("MapDimensions");
                        writer.WriteAttributeString("height", ((maxLvlY + 1) * 512 - 1).ToString());
                        writer.WriteAttributeString("width", ((maxLvlX + 1) * 512 - 1).ToString());
                        writer.WriteEndElement();

                        geo.getLonLatByXY(new Point(startx << 8, starty << 8), -1, out lon1, out lat1);
                        {
                            int realx2, realy2;

                            realx2 = startx + (maxLvlX + 1) * 2;
                            realx2 = (realx2 << 8) - 1;
                            realy2 = starty + (maxLvlY + 1) * 2;
                            realy2 = (realy2 << 8) - 1;

                            geo.getLonLatByXY(new Point(realx2, realy2), -1, out lon2, out lat2);
                        }

                        writer.WriteStartElement("MapBounds");
                        writer.WriteAttributeString("minLon", lon1.ToString("F6", ncUtils.Glob.numformat));
                        writer.WriteAttributeString("maxLon", lon2.ToString("F6", ncUtils.Glob.numformat));
                        writer.WriteAttributeString("maxLat", lat1.ToString("F6", ncUtils.Glob.numformat));
                        writer.WriteAttributeString("minLat", lat2.ToString("F6", ncUtils.Glob.numformat));
                        writer.WriteEndElement();

                        writer.WriteStartElement("CalibrationPoints");
                        {
                            writer.WriteStartElement("CalibrationPoint");
                            writer.WriteAttributeString("corner", "TL");
                            writer.WriteAttributeString("lon", lon1.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteAttributeString("lat", lat1.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteEndElement();
                        }
                        {
                            writer.WriteStartElement("CalibrationPoint");
                            writer.WriteAttributeString("corner", "TR");
                            writer.WriteAttributeString("lon", lon2.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteAttributeString("lat", lat1.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteEndElement();
                        }
                        {
                            writer.WriteStartElement("CalibrationPoint");
                            writer.WriteAttributeString("corner", "BL");
                            writer.WriteAttributeString("lon", lon1.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteAttributeString("lat", lat2.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteEndElement();
                        }
                        {
                            writer.WriteStartElement("CalibrationPoint");
                            writer.WriteAttributeString("corner", "BR");
                            writer.WriteAttributeString("lon", lon2.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteAttributeString("lat", lat2.ToString("F6", ncUtils.Glob.numformat));
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch (System.Exception ex)
            {
            	Program.Err("Could not create OruxMaps xml file: " + fname + "\nError:" + ex);
                if (writer != null)
                    writer.Close();
            }
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
