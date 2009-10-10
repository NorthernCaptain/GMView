using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    public class GPSInfoDash: DashBoardBase
    {
        ImageDot textbg;
        object texttex;
        ImageDot overlayImg;
        object overlaytex;
        IGLFont fnt;
        IGLFont speedfnt;
        IGLFont satfnt;
        Color speedcol;
        Speedometer speedometer;

        GPSTrack curTrack;
        GPSTrack.GPSInfo info;
        int antenna_dx, antenna_dy;
        object[] antennatex = new object[5];

        private int radius = 36; // satellite ball radius

        //Satellite images
        private object sat_ok_tex;
        private object sat_vi_tex;

        public GPSInfoDash()
        {
            bgImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashGPSBg);
            bgTex = TextureFactory.singleton.getTex(bgImg);
            bgImgWrapped = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashGPSWrapped);
            bgTexWrapped = TextureFactory.singleton.getTex(bgImgWrapped);
            textbg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashTextBg);
            texttex = TextureFactory.singleton.getTex(textbg);
            overlayImg = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DashGPSOverlay);
            overlaytex = TextureFactory.singleton.getTex(overlayImg);
            fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans12R);
            speedfnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Big48I);
            satfnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Mid14B);
            speedcol = Color.FromArgb(252, 165, 27); //orange color for speed digits

            ImageDot antdot = TextureFactory.singleton.getImg(TextureFactory.TexAlias.HDOP_0_Ideal);
            antenna_dx = antdot.delta_x;
            antenna_dy = antdot.delta_y;
            antennatex[0] = TextureFactory.singleton.getTex(TextureFactory.TexAlias.HDOP_0_Ideal);
            antennatex[1] = TextureFactory.singleton.getTex(TextureFactory.TexAlias.HDOP_1_Excelent);
            antennatex[2] = TextureFactory.singleton.getTex(TextureFactory.TexAlias.HDOP_2_Good);
            antennatex[3] = TextureFactory.singleton.getTex(TextureFactory.TexAlias.HDOP_3_Moderate);
            antennatex[4] = TextureFactory.singleton.getTex(TextureFactory.TexAlias.HDOP_4_Fair);

            sat_ok_tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SatOK);
            sat_vi_tex = TextureFactory.singleton.getTex(TextureFactory.TexAlias.SatVI);

            dheight = bgImg.delta_y;
            width = bgImg.delta_x;
            justif = DashBoardContainer.Justify.Top | DashBoardContainer.Justify.Right;

            addCallers();

            speedometer = new Speedometer();
            speedometer.onVisualChanged += new System.Windows.Forms.MethodInvoker(speedometer_onVisualChanged);
            GPSTrackFactory.singleton.onRecordingTrackChanged += singleton_onRecordingTrackChanged;
            singleton_onRecordingTrackChanged(GPSTrackFactory.singleton.recordingTrack);
        }

        /// <summary>
        /// Calls each time GPSTrack is updated with new position.
        /// Sends message to speedometer thread for checking and redrawing
        /// </summary>
        public void curTrack_onTrackChanged()
        {
            speedometer.curSpeed = info.curSpeed;
         /*   
            speedometer.curSpeed = (int)GML.device.angle;
            info.curSpeedS = "295";
            info.curLat = "57,3462 N";
            info.curLon = "32,6853 E";
            info.curDir = Glob.courseString(23.455);
            info.satUsed = "7/12";
            info.satHDOP = "2,4";
            info.travelDistance = "367,8";
            info.travelTime = "01:34:26";
            */
            speedometer.updateVisualForced();
        }

        /// <summary>
        /// Called when we change our recording track to a new one. Here we reinitialize our structures
        /// </summary>
        /// <param name="gtrack"></param>
        void singleton_onRecordingTrackChanged(GPSTrack gtrack)
        {
            if (curTrack != null)
                curTrack.onTrackChanged -= curTrack_onTrackChanged;

            curTrack = GPSTrackFactory.singleton.recordingTrack;
            if (curTrack != null)
            {
                curTrack.onTrackChanged += curTrack_onTrackChanged;
                info = curTrack.gpsinfo;
                curTrack_onTrackChanged();
            }            
        }

        /// <summary>
        /// Starts speedometer drawing thread
        /// </summary>
        public void start()
        {
            speedometer.startThread();
        }

        void speedometer_onVisualChanged()
        {
        }

        public Control control
        {
            get { return speedometer.control; }
            set { speedometer.control = value; }
        }

        /// <summary>
        /// Draw our dashboard on the screen
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        public override void draw(int ix, int iy)
        {
            if (dmode == DashMode.Normal)
            {
                GML.device.texDrawBegin();
                GML.device.texFilter(bgTex, TexFilter.Pixel);
                GML.device.texDraw(bgTex, ix, iy, 0, bgImg.img.Width, bgImg.img.Height);
                GML.device.texDrawEnd();
                speedometer.glDraw(ix + 7, iy - 26);
                GML.device.texDrawBegin();
                GML.device.texFilter(bgTex, TexFilter.Pixel);
                GML.device.texDraw(overlaytex, ix, iy, 0, bgImg.img.Width, bgImg.img.Height);
                GML.device.texDrawEnd();

                if (info != null)
                {
                    info.satUsed = GPSTrackFactory.singleton.currentSatCollection.satellitesInUse.ToString() + "/" +
                        GPSTrackFactory.singleton.currentSatCollection.satellites.Count.ToString();

                    GML.device.color(speedcol);
                    speedfnt.drawright(info.curSpeedS, ix + 221, iy - 108, 0);

                    GML.device.color(textcolor);

                    fnt.drawright(info.curLat, ix + 134, iy - 175, 0);
                    fnt.drawright(info.curLon, ix + 134, iy - 192, 0);
                    fnt.drawright(info.curDir, ix + 258, iy - 175, 0);
                    fnt.drawright(info.travelDistance, ix + 258, iy - 231, 0);
                    fnt.drawright(info.travelTime, ix + 134, iy - 231, 0);

                    fnt.draw(info.timeZone, ix + 64, iy - 334, 0);
                    fnt.draw(info.utcTime, ix + 25, iy - 348, 0);

                    satfnt.drawright(info.satUsed, ix + 134, iy - 270, 0);
                    satfnt.drawright(info.satHDOP, ix + 134, iy - 288, 0);
                }

                GML.device.color(Color.White);

                //Draw satellites in the sky and sat signal quality based on HDOP
                SatelliteCollection sats = GPSTrackFactory.singleton.currentSatCollection;
                GML.device.texDrawBegin();
                GML.device.texFilter(antennatex[sats.hdelusion_idx], TexFilter.Pixel);
                GML.device.texDraw(antennatex[sats.hdelusion_idx], ix + antenna_dx, iy - antenna_dy, 0, 128, 64);

                lock (sats)
                {
                    foreach (KeyValuePair<int, Satellite> satpair in sats.satellites)
                    {
                        Point xy;
                        satpair.Value.getXY(radius, out xy);
                        xy.X = ix + xy.X + 205;
                        xy.Y = iy - (301 - xy.Y);
                        if (satpair.Value.state == Satellite.State.InUse)
                        {
                            GML.device.texDraw(sat_ok_tex, xy.X, xy.Y,
                                    0, 8, 8);
                            ///TODO: draw text - satellite numbers
                            //gr.DrawString(satpair.Value.prn.ToString(), fnt, fntusebrush, new PointF(xy.X + 1, xy.Y + 1));
                        }
                        else
                        {
                            GML.device.texDraw(sat_vi_tex, xy.X, xy.Y,
                                    0, 8, 8);
                        }
                    }
                }

                GML.device.texDrawEnd();

            }
            else
                base.draw(ix, iy);
        }
    }
}
