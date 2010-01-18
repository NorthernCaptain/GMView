using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    partial class GMViewForm
    {
        public delegate void onKeyEvent(object sender, KeyEventArgs e);

        private Dictionary<Keys, onKeyEvent> onKeyDownDict = new Dictionary<Keys, onKeyEvent>();
        private Dictionary<Keys, onKeyEvent> onKeyUpDict = new Dictionary<Keys, onKeyEvent>();

        #region Keyboard processing handlers

        void initKeyboardHandlers()
        {
            onKeyDownDict.Add(Keys.F11, new onKeyEvent(fullScreenMI_Click));
            onKeyDownDict.Add(Keys.F12, new onKeyEvent(minimizeWindow_Key));

            //zoom In/Out
            onKeyDownDict.Add(Keys.PageUp, new onKeyEvent(zoomInBut_Click));
            onKeyDownDict.Add(Keys.PageDown, new onKeyEvent(zoomOutBut_Click));
            onKeyDownDict.Add(Keys.Oemplus, new onKeyEvent(zoomInBut_Click));
            onKeyDownDict.Add(Keys.OemMinus, new onKeyEvent(zoomOutBut_Click));
            onKeyDownDict.Add(Keys.Add, new onKeyEvent(zoomInBut_Click));
            onKeyDownDict.Add(Keys.Subtract, new onKeyEvent(zoomOutBut_Click));

            //move map around
            onKeyDownDict.Add(Keys.W, new onKeyEvent(moveMapUp_Key));
            onKeyDownDict.Add(Keys.S, new onKeyEvent(moveMapDown_Key));
            onKeyDownDict.Add(Keys.A, new onKeyEvent(moveMapLeft_Key));
            onKeyDownDict.Add(Keys.D, new onKeyEvent(moveMapRight_Key));

            onKeyDownDict.Add(Keys.Q, new onKeyEvent(rotateMapLeft_Key));
            onKeyDownDict.Add(Keys.E, new onKeyEvent(rotateMapRight_Key));

            onKeyDownDict.Add(Keys.O, new onKeyEvent(fogOfWar_Key));
            onKeyDownDict.Add(Keys.P, new onKeyEvent(trafficTbut_CheckedChanged));
            onKeyDownDict.Add(Keys.Back, new onKeyEvent(backHistoryPos_Key));

            onKeyDownDict.Add(Keys.Space, new onKeyEvent(followGPS_Key));
            onKeyDownDict.Add(Keys.Oemtilde, new onKeyEvent(miniMapMI_Click));
            onKeyDownDict.Add(Keys.Tab, new onKeyEvent(miniMapMI_Click));

            onKeyDownDict.Add(Keys.F1, new onKeyEvent(showHelp_Key));
            onKeyDownDict.Add(Keys.F2, new onKeyEvent(saveTrackSB_Click));
            onKeyDownDict.Add(Keys.F3, new onKeyEvent(loadTrackSB_Click));


            onKeyDownDict.Add(Keys.F4, new onKeyEvent(gpsInfoView_Key));
            onKeyDownDict.Add(Keys.F5, new onKeyEvent(trackInfo_Key));
            onKeyDownDict.Add(Keys.F6, new onKeyEvent(zoomInfo_Key));
            onKeyDownDict.Add(Keys.F7, new onKeyEvent(changeMapType_Key));

            onKeyDownDict.Add(Keys.F8, new onKeyEvent(gpsTrackOnAir_CheckedChanged));
            onKeyDownDict.Add(Keys.F9, new onKeyEvent(showGPSTrack_Key));

            onKeyDownDict.Add(Keys.Y, new onKeyEvent(rotateMapByGPS_Key));
            onKeyDownDict.Add(Keys.B, new onKeyEvent(showBmarks_Key));
            onKeyDownDict.Add(Keys.T, new onKeyEvent(showTracks_Key));
            onKeyDownDict.Add(Keys.V, new onKeyEvent(showView_Key));
            onKeyDownDict.Add(Keys.N, new onKeyEvent(markWaypoint_Key));

            onKeyDownDict.Add(Keys.M, new onKeyEvent(nightViewToggle_Key));
            onKeyDownDict.Add(Keys.C, new onKeyEvent(quickPOI_Key));
        }

        /// <summary>
        /// Quickly add new POI to the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void quickPOI_Key(object sender, KeyEventArgs e)
        {
            addBookMI_Click(sender, e);
        }

        void nightViewToggle_Key(object sender, KeyEventArgs e)
        {
            Program.opt.isNightView = Program.opt.isNightView ? false : true;
            GML.device.repaint();
        }

        void minimizeWindow_Key(object sender, KeyEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void showHelp_Key(object sender, KeyEventArgs e)
        {
            Forms.HelpForm hlp = new Forms.HelpForm();
            hlp.Owner = this;
            hlp.Visible = true;
        }

        void markWaypoint_Key(object sender, KeyEventArgs e)
        {
            if (mode == UserAction.Navigate)
            {
                if (gtrack.markLastPoint(ncGeo.NMEA_LL.PointType.MWP))
                    repaintMap();
            }
        }

        void showTracks_Key(object sender, KeyEventArgs e)
        {
            trackStripMenuItem.DropDown.Visible = trackStripMenuItem.DropDown.Visible ? false : true;
        }

        void showView_Key(object sender, KeyEventArgs e)
        {
            ViewMI.DropDown.Visible = ViewMI.DropDown.Visible ? false : true;
        }

        void showBmarks_Key(object sender, KeyEventArgs e)
        {
            poiMI.DropDown.Visible = poiMI.DropDown.Visible ? false : true;
        }

        void backHistoryPos_Key(object sender, KeyEventArgs e)
        {
            if (!pos_stack.empty())
            {
                PositionStack.PositionInfo pinfo = pos_stack.pop();
                pos_stack_fwd.push(new PositionStack.PositionInfo(upos.Lon, upos.Lat, mapo.zoom, opt.mapType));
                upos.setLonLat(pinfo.lon, pinfo.lat);
                mapo.CenterMapLonLat(pinfo.lon, pinfo.lat);
                repaintMap();
            }
        }

        void gpsInfoView_Key(object sender, KeyEventArgs e)
        {
            gpsInfoViewMI_Click(sender, e);
            if (opt.show_gps_info)
            {
                infoMessage("GPS information is ON");
            }
            else
                infoMessage("GPS information is Off");
        }

        void trackInfo_Key(object sender, KeyEventArgs e)
        {
            trackInfoMI_Click(sender, e);
            if (satInfoMI.Checked)
            {
                infoMessage("Track information is ON");
            }
            else
                infoMessage("Track information is Off");
        }

        void zoomInfo_Key(object sender, KeyEventArgs e)
        {
            zoomDashMI_Click(sender, e);
            if (showZoomMI.Checked)
            {
                infoMessage("Zoom information is ON");
            }
            else
                infoMessage("Zoom information is Off");
        }

        void rotateMapByGPS_Key(object sender, KeyEventArgs e)
        {
            if (opt.gps_rotate_map)
            {
                opt.gps_rotate_map = false;
                mapo.angle = 0.0;
                ((IGML)drawPane).angle = mapo.angle;
                onDashboardStateChanged();
                infoMessage("'GPS rotate map' is Off");
            }
            else
            {
                infoMessage("'GPS rotate map' is ON!");
                opt.gps_rotate_map = true;
                onDashboardStateChanged();
                gpsFollowMapTBut.Checked = true;
            }
            repaintMap();
        }

        void changeMapType_Key(object sender, KeyEventArgs e)
        {
            int val = (int)opt.mapType + 1;
            if (val > (int)ncGeo.MapTileType.YandexSat)
                val = 0;
            mapTypeSCombo.SelectedIndex = val;
            infoMessage("Map type: " + opt.MapTileTypeString);
        }

        void fogOfWar_Key(object sender, KeyEventArgs e)
        {
            opt.fog_of_war = opt.fog_of_war ? false : true;
            repaintMap();
        }

        void minusX_Key(object sender, KeyEventArgs e)
        {
            Program.plus_x -= 1.0f;
            repaintMap();
        }

        const int delta_x_move = 60;
        const int delta_y_move = 60;

        void moveMapUp_Key(object sender, KeyEventArgs e)
        {
            Point pt = new Point(0, delta_y_move);
            pt = GML.translateToScene(pt);
            mapo.MoveMapByScreenPoint(pt);
            if (gpsFollowMapTBut.Checked)
            {
                gpsFollowMapTBut.Checked = false;
            }
            else
                repaintMap();
        }

        void moveMapDown_Key(object sender, KeyEventArgs e)
        {
            Point pt = new Point(0, -delta_y_move);
            pt = GML.translateToScene(pt);
            mapo.MoveMapByScreenPoint(pt);
            if (gpsFollowMapTBut.Checked)
            {
                gpsFollowMapTBut.Checked = false;
            }
            else
                repaintMap();
        }

        void moveMapLeft_Key(object sender, KeyEventArgs e)
        {
            mapo.MoveMapByScreenPoint(GML.translateToScene(new Point(delta_x_move, 0)));
            if (gpsFollowMapTBut.Checked)
            {
                gpsFollowMapTBut.Checked = false;
            }
            else
                repaintMap();
        }

        void moveMapRight_Key(object sender, KeyEventArgs e)
        {
            mapo.MoveMapByScreenPoint(GML.translateToScene(new Point(-delta_x_move, 0)));
            if (gpsFollowMapTBut.Checked)
            {
                gpsFollowMapTBut.Checked = false;
            }
            else
                repaintMap();
        }

        const double delta_angle = 5.0; //in degrees
        void rotateMapLeft_Key(object sender, KeyEventArgs e)
        {
            mapo.angle += delta_angle;
            ((IGML)drawPane).angle = mapo.angle;
            if (gpsFollowMapTBut.Checked)
            {
                opt.gps_follow_map = false;
                gpsFollowMapTBut.Checked = false;
            }
            else
                repaintMap();
            gpsdash.curTrack_onTrackChanged();
        }

        void rotateMapRight_Key(object sender, KeyEventArgs e)
        {
            mapo.angle -= delta_angle;
            ((IGML)drawPane).angle = mapo.angle;
            if (gpsFollowMapTBut.Checked)
            {
                opt.gps_follow_map = false;
                gpsFollowMapTBut.Checked = false;
            }
            else
                repaintMap();
        }

        void followGPS_Key(object sender, KeyEventArgs e)
        {
            if (opt.gps_follow_map)
            {
                opt.gps_follow_map = false;
                infoMessage("'GPS follow map' is Off");
            }
            else
            {
                opt.gps_follow_map = true;
                infoMessage("'GPS follow map' is ON");
            }
            gpsFollowMapTBut.Checked = opt.gps_follow_map;
        }

        void showGPSTrack_Key(object sender, KeyEventArgs e)
        {
            if (gpsTrackTBut.Checked)
            {
                gpsTrackTBut.Checked = false;
                infoMessage("'Show GPS' is Off");
            }
            else
            {
                gpsTrackTBut.Checked = true;
                infoMessage("'Show GPS' is ON");
            }
            gpsTrackTBut_CheckedChanged(sender, e);
        }

        #endregion
    }
}
