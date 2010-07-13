using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ncGeo;
using GMView.UIHelper;

namespace GMView
{
    enum UserAction { Unknown, Navigate, Zoom, SelectArea, ManualTrack, MaxUserAction };

    public partial class GMViewForm : Form
    {
        internal MapObject mapo;
        internal MapObject minimapo;
        internal MapForm miniform;

        private Options opt;

        private UserAction mode = UserAction.Unknown;
        private MouseBaseProc[] modes = new MouseBaseProc[(int)UserAction.MaxUserAction];
        private UIHelper.NavigateMode naviMode;
        private UIHelper.MouseBaseProc currentMode;

        private NMEAThread nmea_thread;
        private SatelliteCollection satellites;
        private SatelliteForm satForm;
        private int com_lamp_state = 0;

        internal GPSTrack gtrack;
        private GPSTrack gtrack_mini;

        public LogWin logWin = new LogWin();

        private DateTime lastRepaint = DateTime.Now;
        private UserControl drawPane;

        private Forms.IntroForm intro;

        private DashBoardContainer boards;
        private GPSInfoDash gpsdash;
        private TrackInfoDash trackdash;
        private MiniTilesDash zoomdash;

        private bool inAutoScroll = false;

        internal GPS.TrackPositionInformer trackinformer;
        private CenterPositioning centerPos = null;

        private XnGFL.ExifViewControl geoTagger;
        private Forms.GeoTagForm geoTagForm;

        private WindRose wind_rose;

        #region Our events
        private delegate void onStartDownloadDelegate(int total_pieces);
        private delegate void onProgressDownloadDelegate(ImgTile tile, double percent);
        private delegate void onEndDownloadDelegate();

        public delegate void OnLogDelegate(string str);

        #endregion

        public GMViewForm()
        {
            opt = Program.opt;

            intro = new GMView.Forms.IntroForm();
            intro.showIntro();
            intro.doInit();

            //Initializing GFL library
            XnGFL.Common.LibraryInit();

            mapo = new MapObject();
            mapo.onCenterChanged += new MapObject.onLonLatChange(mapo_onCenterChanged);
            ImgCacheManager.singleton.map = mapo;
            ImgCacheManager.singleton.onVisualChanged += repaintMap;

            GPSTrackFactory.onRemove = gtrform_onRemove;
            GPSTrackFactory.onRecord = gtrform_onRecord;
            GPSTrackFactory.onMenuClick = trackChoose_Click;

            minimapo = new MapObject(mapo);

            miniform = new MapForm(minimapo);
            miniform.onHide += new MethodInvoker(miniMap_onHide);
            miniform.KeyDown += new KeyEventHandler(GMViewForm_KeyDown);
            miniform.KeyUp += new KeyEventHandler(GMViewForm_KeyUp);
            miniform.drawPane.MouseDoubleClick += new MouseEventHandler(miniform_MouseDoubleClick);
            opt.onChanged += new Options.OnChangedDelegate(miniform.repaintMap);

            nmea_thread = new NMEAThread();
            nmea_thread.onLogNMEAString += new NMEAThread.OnNMEAStingDelegate(nmea_thread_onLogNMEAString);
            nmea_thread.onNMEACommand += new NMEAThread.OnNMEACommandDelegate(nmea_thread_onNMEACommand);

            satellites = new SatelliteCollection();
            GPSTrackFactory.singleton.currentSatCollection = satellites;
            gtrack = new GPSTrack(mapo);
            GPSTrackFactory.singleton.recordingTrack = gtrack;
            gtrack_mini = new GPSTrack(minimapo, new ImageDot(global::GMView.Properties.Resources.gps_point_small, 7, 22));
            gtrack_mini.need_arrows = false;
            satForm = new SatelliteForm(-140, 5, satellites); //-1,3
            trackinformer = new GMView.GPS.TrackPositionInformer(mapo);

            InitializeComponent();

            this.drawPane = opt.newMapDrawControl(mapo, satForm, null);
            ((IGML)this.drawPane).onInitGML += new EventHandler(initGLData);
            ((IGML)this.drawPane).onResizeGML += new EventHandler(GMViewForm_onResizeGML);
            ((IGML)this.drawPane).onRenderGML += new EventHandler(GMViewForm_onRenderGML);
            ((IGML)this.drawPane).onCurrentGML += new EventHandler(GMViewForm_onCurrentGML);

            this.drawPane.Dock = DockStyle.Fill;
            this.mainPanel.Controls.Add(drawPane, 0, 0);
            this.Text = Options.program_full_name + " v." + Options.program_version;

            if (opt.is_full_screen)
                enterFullScreen();

            mapo.SetVisibleSize(drawPane.Size);
            mapo.CenterMapLonLat(opt.lon, opt.lat);
            mapTypeSCombo.SelectedIndex = (int)opt.mapType;
            windRoseMI.Checked = opt.show_wind_rose;

            initMap();

            nmea_thread.start();

            if (opt.gps_follow_map)
                gpsFollowMapTBut.Checked = true;

            miniform.Owner = this;
            if (opt.show_mini_map)
            {
                miniMapMI.Checked = true;
                showMiniMap();
            }

            changeMode(UserAction.Navigate);

            initKeyboardHandlers();
            initManualTrackMode();

            centerPos = new CenterPositioning(deltaCenterTimer);

            if (opt.useGML == GML.GMLType.direct3D)
                initGLData(drawPane, EventArgs.Empty);

            ((IGML)this.drawPane).onLostDevice += new EventHandler(GMViewForm_onLostDevice);
            ((IGML)this.drawPane).onReinitDevice += new EventHandler(GMViewForm_onReinitDevice);

            BookMarkFactory.singleton.map = mapo;
            BookMarkFactory.singleton.fillMenuItems(poiMI.DropDown.Items);

            initGeoTagger();
        }

        /// <summary>
        /// Initialize Geo tagger objects
        /// </summary>
        void initGeoTagger()
        {
            geoTagger = new XnGFL.ExifViewControl();
            geoTagForm = new GMView.Forms.GeoTagForm(geoTagger);
            geoTagForm.Owner = this;
            geoTagger.needCenteringLonLat += centerMapLonLat;
            mapo.onCenterChanged += geoTagger.setLonLatFromMap;
            this.onClickLonLat += geoTagger.clickLonLat;
        }

        void GMViewForm_onLostDevice(object sender, EventArgs e)
        {
            GML.device = (IGML)sender;
            mapo.ResetCache();
        }

        void GMViewForm_onReinitDevice(object sender, EventArgs e)
        {
            GML.device = (IGML)sender;
            gtrack.initGLData();
            satForm.initGLData();

            foreach (MouseBaseProc modep in modes)
            {
                if (modep != null)
                {
                    modep.Boards = boards;
                    modep.initGL();
                }
            }

            RunMeOnce.singleton.runMeOnce(new MethodInvoker(repaintMap), 1000);
        }

        void GMViewForm_onCurrentGML(object sender, EventArgs e)
        {
            GML.device = (IGML)sender;
        }

        private void initMap()
        {
            initModes();

            wind_rose = new WindRose();
            mapo.addSub(wind_rose);
            mapo.addSub(gtrack);

            minimapo.addSub(gtrack_mini);

            opt.onChanged += new Options.OnChangedDelegate(opt_onChanged);

            mapo.onEndDownloadTask += delegate() 
            {
                try
                {
                    this.Invoke(new onEndDownloadDelegate(this.EndProgress));
                }
                catch { }
            };
            mapo.onStartDownloadTask += delegate(int total_pieces)
            {
                try
                {
	                this.Invoke(new onStartDownloadDelegate(this.InitProgress),
	                                    new Object[] { total_pieces });
                }
                catch (System.Exception ex)
                {
                	
                }
            };
            mapo.onProgressDownloadTask += delegate(ImgTile tile, double percent)
            {
                try
                {
                    this.Invoke(new onProgressDownloadDelegate(this.DoProgress),
                        new Object[] { tile, percent });
                }
                catch
                {
                }
            };


            mapo.onSchedEndDownloadTask += delegate() 
            {
                try
                {
                    this.Invoke(new onEndDownloadDelegate(this.EndSchedProgress));
                }
                catch { }
            };
            mapo.onSchedStartDownloadTask += delegate(int total_pieces)
            {
                this.Invoke(new onStartDownloadDelegate(this.InitSchedProgress),
                    new Object[] { total_pieces });
            };
            mapo.onSchedProgressDownloadTask += delegate(ImgTile tile, double percent)
            {
                try
                {
                    this.Invoke(new onProgressDownloadDelegate(this.DoSchedProgress),
                        new Object[] { tile, percent });
                }
                catch { }
            };

        }

        void initGLData(object sender, EventArgs e)
        {
            FontFactory.singleton.initGLData();
            TextureFactory.singleton.initGLData();

            GML.device.deltaCenter = centerPos.deltaCenter;

            createDashBoards();

            foreach(MouseBaseProc modep in modes)
            {
                if(modep != null)
                {
                    modep.Boards = boards;
                    modep.initGL();
                }
            }

            if (opt.command_line_args.Length > 0)
                loadTrack(new GPS.TrackFileInfo(opt.command_line_args[0].ToString(), 
                                            GPS.TrackFileInfo.SourceType.FileName));
            else
                GPSTrackFactory.singleton.currentTrack = gtrack;

            GPSTrackFactory.singleton.addTrack(gtrack);
            GPSTrackFactory.singleton.rebuildMenuStrip(trackStripMenuItem.DropDown.Items);

            FrameTimer.singleton.onUpdated += new EventHandler(frametimer_onUpdated);
            FrameTimer.singleton.Start();
            gpsdash.start();

            wind_rose.initGLData();

            if (opt.show_wind_rose)
                wind_rose.show();

            initFollowUP();
            wind_rose.follower = followConnect;
            BookMarkFactory.singleton.startAutoShowWorker(this);
        }

        /// <summary>
        /// Initializes user navigation modes
        /// </summary>
        private void initModes()
        {
            naviMode = new NavigateMode(this, drawPane);
            modes[(int)UserAction.Navigate] = naviMode;
            modes[(int)UserAction.Zoom] = new ZoomAreaMode(this, drawPane);
            modes[(int)UserAction.SelectArea] = new DownloadAreaMode(this, drawPane);
        }

        /// <summary>
        /// Called when state of the dashboard has been changed, moves center position accordingly
        /// </summary>
        private void onDashboardStateChanged()
        {
            centerPos.deltaCenter = new Point(boards.isMinimized() ? 0 : 
                (opt.dash_right_side ? -100 : 100), opt.gps_rotate_map ? -50 : 0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ImgCacheManager.singleton.control = this;
            ImgCacheManager.singleton.updateVisualForced();
            naviMode.upos.setLonLat(mapo.centerPos.lon, mapo.centerPos.lat);
            intro.hideIntro();
            intro.Dispose();
            intro = null;
            Program.logger = logWin;
        }

        /// <summary>
        /// Creates a dashboard with all widgets
        /// </summary>
        private void createDashBoards()
        {
            if (boards != null)
                return;
            GML.tranBegin();

            boards = opt.dash_right_side ?
                new DashBoardContainer(DashBoardContainer.Justify.Right, 1, 2, 273) :
                new DashBoardContainer(DashBoardContainer.Justify.Left, 1, 2, 273);
            boards.setVisibleSize(drawPane.ClientSize);
            boards.onStateChanged += onDashboardStateChanged;

            zoomdash = new MiniTilesDash(mapo, naviMode.upos);
            zoomdash.onCenterMapXY += new MiniTilesDash.onCenterMapXYDelegate(idash_onCenterMapXY);
            zoomdash.mode = DashMode.Normal;
            zoomdash.onZoomIn += this.zoomIn;
            zoomdash.onZoomOut += this.zoomOut;
            boards.addDashBoard(zoomdash);

            gpsdash = new GPSInfoDash();
            gpsdash.mode = DashMode.Wrapped;
            gpsdash.control = this;
            boards.addDashBoard(gpsdash);

            trackdash = new TrackInfoDash();
            trackdash.mode = DashMode.Wrapped;
            trackdash.onCenterWaypoint += new TrackInfoDash.OnCenterWaypointDelegate(idtrack_onCenterWaypoint);
            boards.addDashBoard(trackdash);

            mapo.addSub(boards);
            boards.show();

            gpsInfoViewMI.Checked = !opt.show_gps_info;
            gpsInfoViewMI_Click(null, EventArgs.Empty);
            satInfoMI.Checked = !opt.show_sat_info;
            trackInfoMI_Click(null, EventArgs.Empty);
            showZoomMI.Checked = !opt.show_zoom_panel;
            zoomDashMI_Click(null, EventArgs.Empty);

            mapo.addSub(trackinformer);
            trackinformer.show();

            GML.tranEnd();
        }

        void idtrack_onCenterWaypoint(WayBase.WayPoint wp)
        {
            if (wp == null)
                return;
            mapo.CenterMapLonLat(wp.point.lon, wp.point.lat);
        }

        
        void idash_onCenterMapXY(int x, int y, int zoom)
        {
            if (mapo.zoom != zoom)
                return;

            naviMode.backPush(mapo.centerPos);
            mapo.CenterMapAbsXY(new Point(x, y));
            repaintMap();
            miniform.doSync = true;
            miniform.repaintMap();
        }

        void frametimer_onUpdated(object sender, EventArgs e)
        {
            repaintMap();
        }

        void GMViewForm_onRenderGML(object sender, EventArgs e)
        {
            mapo.glDraw(drawPane.Size.Width / 2, drawPane.Size.Height / 2);
        }

        void GMViewForm_onResizeGML(object sender, EventArgs e)
        {
            UserControl uc = (UserControl)sender;
            if (mapo != null)
            {
                mapo.SetVisibleSize(uc.ClientSize);
                mapo.recenterMap();
                uc.Invalidate();
            }
            if (boards != null)
                boards.setVisibleSize(uc.ClientSize);
        }

        #region Download Task progress methods
        private void InitProgress(int total)
        {
            ProgBar.Value = 0;
            ProgBar.Maximum = total;
            ProgBar.Enabled = true;
            ProgBar.Invalidate();
            StatusLb.Text = "Downloading";
        }

        private void DoProgress(ImgTile img, double perc)
        {
            ProgBar.Increment(1);
            repaintMap();
        }

        private void EndProgress()
        {
            repaintMap();
            miniform.repaintMap();
            ProgBar.Value = 0;
            ProgBar.Enabled = false;
            ProgBar.Invalidate();
            StatusLb.Text = "Done";
            ImgCacheManager.singleton.updateVisualForced();
        }

        private int sched_perc = 0;
        private void InitSchedProgress(int total)
        {
            bgProgBar.Value = 0;
            bgProgBar.Maximum = total;
            bgProgBar.Enabled = true;
            bgProgBar.Invalidate();
            bgStatusLb.Text = "Downloading";
            sched_perc = 0;
        }

        private void DoSchedProgress(ImgTile img, double perc)
        {
            bgProgBar.Increment(1);
            if ((int)(perc / 10.0) > sched_perc)
            {
                sched_perc = (int)(perc / 10.0);
                ImgCacheManager.singleton.updateVisualForced();
            }
        }

        private void EndSchedProgress()
        {
            repaintMap();
            bgProgBar.Value = 0;
            bgProgBar.Enabled = false;
            bgProgBar.Invalidate();
            bgStatusLb.Text = "Done";
            ImgCacheManager.singleton.updateVisualForced();
        }

        #endregion


        public void infoMessage(string text)
        {
            IGLFont fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Big22B);
            FadingText fdt = new FadingText(10, drawPane.Size.Height - 40, text, Color.Red, fnt);
            fdt.onDone += new EventHandler(fdt_onDone);
            mapo.addSub(fdt);
        }

        /// <summary>
        /// Fading text comletely hidden, let's remove it from our draw cycle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fdt_onDone(object sender, EventArgs e)
        {
            ISprite sprite = sender as ISprite;
            mapo.delSub(sprite);
        }

        /// <summary>
        /// Center map on screen according to lon/lat in options
        /// </summary>
        public void centerMap()
        {
            Point xy;
            double lon, lat;

            mapo.getXYByLonLat(opt.lon, opt.lat, out xy);
            mapo.getLonLatByXY(xy, out lon, out lat);
            mapo.CenterMapLonLat(opt.lon, opt.lat);
            repaintMap();
            miniform.repaintMap();
        }

        /// <summary>
        /// Centers map by given Lon, Lat
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        internal void centerMapLonLat(double lon, double lat)
        {
            naviMode.upos.setLonLat(lon, lat);
            naviMode.upos_mini.setLonLat(lon, lat);
            mapo.CenterMapLonLat(lon, lat);
            repaintMap();
            miniform.repaintMap();
        }

        public void repaintMap()
        {
            GML.repaint();
        }

        private void changeMode(UserAction newmode)
        {
            MouseBaseProc oldone = currentMode;

            if (newmode == mode)
                return;

            if (currentMode != null)
                currentMode.modeLeave();

            currentMode = modes[(int)newmode];

            if(currentMode != null)
            {
                currentMode.modeEnter(oldone);
                modeSLbl.Text = currentMode.name();
            }

            mode = newmode;

            repaintMap();
        }

        #region Various event handlers for our form objects

        private void zoomIn(Point mouse_release_p)
        {
            if (opt.cur_zoom_lvl == opt.max_zoom_lvl)
                return;

            opt.cur_zoom_lvl++;
            mapo.ZoomMapXY(mouse_release_p, opt.cur_zoom_lvl);
            repaintMap();
        }

        internal void zoomIn()
        {
            if (opt.cur_zoom_lvl == opt.max_zoom_lvl)
                return;

            opt.cur_zoom_lvl++;
            mapo.ZoomMapCentered(opt.cur_zoom_lvl);
            repaintMap();
        }

        private void zoomOut(Point centerp)
        {
            if (opt.cur_zoom_lvl == 1)
                return;

            opt.cur_zoom_lvl--;
            mapo.ZoomMapXY(centerp, opt.cur_zoom_lvl);
            repaintMap();
        }

        internal void zoomOut()
        {
            if (opt.cur_zoom_lvl == 1)
                return;

            opt.cur_zoom_lvl--;
            mapo.ZoomMapCentered(opt.cur_zoom_lvl);
            repaintMap();
        }

        
        private void OptionsMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDlg dlg = new OptionsDlg();
            dlg.Show();
        }

        private void opt_onChanged()
        {
            boards.setPosition(opt.dash_right_side ? 
                DashBoardContainer.Justify.Right :
                DashBoardContainer.Justify.Left,
                1, 2);
            boards.setVisibleSize(drawPane.ClientSize);
            repaintMap();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            opt.Save();
            this.Close();
            this.Dispose();
        }

        private void GMViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            opt.Save();
        }

        private Forms.SetLonLatForm lonlatForm = null;
        private void setLLBut_Click(object sender, EventArgs e)
        {
            if (lonlatForm == null)
            {
                lonlatForm = new Forms.SetLonLatForm();
                lonlatForm.Owner = this;
            }

            try
            {
                if (lonlatForm.ShowDialog() != DialogResult.OK)
                    return;

                double lon, lat;
                lon = lonlatForm.longitude;
                lat = lonlatForm.latitude;
                opt.lon = lon;
                opt.lat = lat;
                opt.Updated();
                centerMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please fill in Lon and Lat fields with float numbers first.\n\nError: " + ex.Message,
                    "Error setting Lon/Lat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }

        private void mapTypeSCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            opt.mapType = (MapTileType)mapTypeSCombo.SelectedIndex;
            mapo.recenterMap();
            minimapo.recenterMap();
            repaintMap();
            miniform.repaintMap();
            mapo.ResetCache();
            minimapo.ResetCache();
        }

        private void selectRectDownSBut_CheckedChanged(object sender, EventArgs e)
        {
            if (selectRectDownSBut.Checked)
            {
                viewSBut.Checked = false;
                zoomSBut.Checked = false;
                pinMarkSBut.Checked = false;
                drawPane.Cursor = Cursors.Cross;
                changeMode(UserAction.SelectArea);
            }
            else
            {
                drawPane.Cursor = Cursors.Default;
                changeMode(UserAction.Navigate);
            }
        }

        private void zoomSBut_CheckedChanged(object sender, EventArgs e)
        {
            if (zoomSBut.Checked)
            {
                viewSBut.Checked = false;
                selectRectDownSBut.Checked = false;
                pinMarkSBut.Checked = false;
                drawPane.Cursor = Cursors.Hand;
                changeMode(UserAction.Zoom);
            }
            else
            {
                drawPane.Cursor = Cursors.Default;
                changeMode(UserAction.Navigate);
            }
        }

        private void viewSBut_CheckedChanged(object sender, EventArgs e)
        {
            if (viewSBut.Checked)
            {
                zoomSBut.Checked = false;
                selectRectDownSBut.Checked = false;
                pinMarkSBut.Checked = false;
                drawPane.Cursor = Cursors.Default;
                changeMode(UserAction.Navigate);
            }
            else
            {
            }
        }

        private void pinMarkSBut_Click(object sender, EventArgs e)
        {
            changeMode(pinMarkSBut.Checked ? UserAction.ManualTrack : UserAction.Navigate);
            drawPane.Cursor = pinMarkSBut.Checked ? Cursors.Cross : Cursors.Default;
            if (pinMarkSBut.Checked)
            {
                zoomSBut.Checked = false;
                selectRectDownSBut.Checked = false;
                viewSBut.Checked = false;
                if (manualMode.manualTrack.countPoints > 0)
                {
                    if(MessageBox.Show("Continue editing current track?\n\nYes - will continue current track\nNo - clear previous track and start new one", "Record track",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.No)
                        manualMode.resetTrack();
                    infoMessage("'Manual edit track' is ON");
                }
            }
        }

        private void zoomOutBut_Click(object sender, EventArgs e)
        {
            zoomOut();
            repaintMap();
        }

        private void zoomInBut_Click(object sender, EventArgs e)
        {
            zoomIn();
            repaintMap();
        }

        private void comStateSLb_DoubleClick(object sender, EventArgs e)
        {
            OptionsDlg dlg = new OptionsDlg();
            dlg.Show();
            dlg.setActiveTab("nmeaPage");
        }
 
        private void gpsLampSLb_DoubleClick(object sender, EventArgs e)
        {
        }

        private void gpsStatSLb_DoubleClick(object sender, EventArgs e)
        {
            NMEA_LL nmeall = gtrack.lastData;
            if (nmeall == null)
                return;
            naviMode.upos.setLonLat(nmeall.lon, nmeall.lat);
            naviMode.upos_mini.setLonLat(nmeall.lon, nmeall.lat);
            mapo.CenterMapLonLat(nmeall.lon, nmeall.lat);
            repaintMap();
        }

        private void satInfoSLb_DoubleClick(object sender, EventArgs e)
        {
            satForm.Visible = true;
        }

        /// <summary>
        /// Show GPS track on map or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gpsTrackTBut_CheckedChanged(object sender, EventArgs e)
        {
            if (gpsTrackTBut.Checked)
            {
                gtrack.show();
                gtrack_mini.show();
            }
            else
            {
                gtrack.hide();
                gtrack_mini.hide();
            }
            repaintMap();
        }

        /// <summary>
        /// Turn on or off recording of the track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gpsTrackOnAir_CheckedChanged(object sender, EventArgs e)
        {
            bool newTrack = true;
            if (!gtrack.on_air)
            {
                // Continue previous track?
                if (gtrack.countPoints > 0)
                {
                    if (MessageBox.Show("Continue recording current track?\n\nYes - will continue current track\nNo - clear previous track and start new one", "Record track",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.No)
                        gtrack.resetTrackData();
                    else
                        newTrack = false;
                }

                if (newTrack)
                {
                    //Ask start and dest location name and form filename
                    Forms.TrackDestFrom destForm = new Forms.TrackDestFrom();
                    destForm.Owner = this;
                    destForm.StartPosition = FormStartPosition.CenterScreen;
                    if (destForm.ShowDialog() == DialogResult.OK)
                    {
                        opt.autosavefile = destForm.trackName;
                        gtrack.fileName = opt.autosavefile;
                        gtrack.track_name = destForm.trackName;
                    }
                    else
                    {
                        opt.autosavefile = "auto-" + DateTime.Now.ToString("yyyy-MM-dd_HHmm");
                        gtrack.fileName = opt.autosavefile;
                        gtrack.track_name = "Auto generated at " + DateTime.Now.ToString("yyyy-MM-dd_HHmm");
                    }
                    destForm.Dispose();
                }
                gtrack.on_air = true;
                infoMessage("'Record track' is ON");
                gpsdash.mode = DashMode.Normal;
                trackdash.mode = DashMode.Normal;
                zoomdash.mode = DashMode.Wrapped;
                gpsTrackTBut.Checked = true;
                gpsFollowMapTBut.Checked = true;
            }
            else
            {
                gtrack.on_air = false;
                infoMessage("'Record track' is Off");
            }
            gpsTrackOnAir.Checked = gtrack.on_air;
            recordTrackMI.Checked = gtrack.on_air;
            repaintMap();
        }

        /// <summary>
        /// Center map to the ongoing GPS position.
        /// We do not do anything here becouse we check the state of this button on every redraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gpsFollowMapTBut_CheckedChanged(object sender, EventArgs e)
        {
            opt.gps_follow_map = gpsFollowMapTBut.Checked;
            if (opt.gps_follow_map && gtrack.lastData != null)
            {
                mapo.CenterMapLonLat(gtrack.lastData.lon, gtrack.lastData.lat);
            }
            repaintMap();
            opt.Save();
        }

        private void gpsInfoViewMI_Click(object sender, EventArgs e)
        {
            if (gpsInfoViewMI.Checked)
            {
                gpsInfoViewMI.Checked = false;
                opt.show_gps_info = false;
                gpsdash.mode = DashMode.Wrapped;
            }
            else
            {
                gpsdash.mode = DashMode.Normal;
                gpsInfoViewMI.Checked = true;
                opt.show_gps_info = true;
            }
            repaintMap();
            opt.Save();
        }

        private void trackInfoMI_Click(object sender, EventArgs e)
        {
            if (satInfoMI.Checked)
            {
                trackdash.mode = DashMode.Wrapped;
                satInfoMI.Checked = false;
                opt.show_sat_info = false;
            }
            else
            {
                trackdash.mode = DashMode.Normal;
                satInfoMI.Checked = true;
                opt.show_sat_info = true;
            }
            repaintMap();
            opt.Save();
        }

        private void openLogMI_Click(object sender, EventArgs e)
        {
            logWin.Visible = true;
        }

        private void GMViewForm_KeyDown(object sender, KeyEventArgs e)
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "Alt", e.Alt);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Control", e.Control);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyCode", e.KeyCode);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyValue", e.KeyValue);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyData", e.KeyData);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Modifiers", e.Modifiers);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Shift", e.Shift);
            messageBoxCS.AppendLine();

            logWin.Log("KeyDown: " + messageBoxCS.ToString());

            onKeyEvent evt;
            if (onKeyDownDict.TryGetValue(e.KeyCode, out evt))
            {
                drawPane.Focus();
                evt(sender, e);
            }

        }

        private void GMViewForm_KeyUp(object sender, KeyEventArgs e)
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "Alt", e.Alt);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Control", e.Control);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyCode", e.KeyCode);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyValue", e.KeyValue);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyData", e.KeyData);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Modifiers", e.Modifiers);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Shift", e.Shift);
            messageBoxCS.AppendLine();

            logWin.Log("KeyUP: " + messageBoxCS.ToString());

            onKeyEvent evt;
            if (onKeyUpDict.TryGetValue(e.KeyCode, out evt))
            {
                evt(sender, e);
            }
        }

        private void enterFullScreen()
        {
            fullScreenMI.Checked = true;
            drawPane.Margin = new Padding(0);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
            this.WindowState = FormWindowState.Maximized;
            toolStrip.Visible = false;
            statusStrip.Visible = false;
            menuStrip.Visible = false;
            opt.is_full_screen = true;
        }

        private void leaveFullScreen()
        {
            fullScreenMI.Checked = false;
            drawPane.Margin = new Padding(2);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            toolStrip.Visible = true;
            statusStrip.Visible = true;
            menuStrip.Visible = true;
            opt.is_full_screen = false;
        }

        private void fullScreenMI_Click(object sender, EventArgs e)
        {
            if (fullScreenMI.Checked)
            {
                leaveFullScreen();
                opt.Save();
            }
            else
            {
                enterFullScreen();
                opt.Save();
            }
        }

        private void miniMapMI_Click(object sender, EventArgs e)
        {
            miniMapMI.Checked = miniMapMI.Checked ? false : true;
            if (miniMapMI.Checked)
                showMiniMap();
            else
            {
                miniform.Visible = false;
                miniMap_onHide();
            }
        }

        private void showMiniMap()
        {
            opt.show_mini_map = true;
            miniform.Location = opt.mini_position;
            miniform.StartPosition = FormStartPosition.Manual;
            miniform.Size = opt.mini_size;
            miniform.Visible = true;
            this.Activate();
        }

        private void miniMap_onHide()
        {
            miniMapMI.Checked = false;
            opt.show_mini_map = false;
            opt.mini_position = miniform.Location;
            opt.mini_size = miniform.Size;
            opt.Save();
        }

        void mapo_onCenterChanged(double lon, double lat)
        {
            if (opt.show_mini_map)
            {
                if (miniMapTimer.Enabled)
                    miniMapTimer.Stop();
                miniMapTimer.Start();
            }
            opt.lon = lon;
            opt.lat = lat;
        }


        private void resetCacheMI_Click(object sender, EventArgs e)
        {
            mapo.ResetCache();
            minimapo.ResetCache();
            logWin.reset();
        }

        private void miniMapTimer_Tick(object sender, EventArgs e)
        {
            miniform.repaintMap();
            miniMapTimer.Stop();
        }


        #region GPSTrack various handlers
        /// <summary>
        /// Saves the current track to the disk file in xml format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveTrackSB_Click(object sender, EventArgs e)
        {
            GPSTrack track = gtrack;
            if (mode == UserAction.ManualTrack)
                track = manualMode.manualTrack;

            saveTrackWithDialog(track);
        }

        /// <summary>
        /// Loads track from disk by the given file name
        /// This method is called when we start app with track as parameter
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="col"></param>
        private void loadTrack(GPS.TrackFileInfo trackInfo)
        {
            GPSTrack gtr;
            try
            {
                gtr = GPSTrack.loadFrom(trackInfo, BookMarkFactory.singleton, Bookmarks.POIGroupFactory.singleton());
                gtr.need_arrows = false;
                gtr.trackMode = GPSTrack.TrackMode.ViewSaved;
                gtr.map = mapo;

                gtr.trackColor = trackInfo.trackColor;

                mapo.addSub(gtr);
                gtr.initGLData();
                gtr.show();
                mapo.CenterMapLonLat(gtr.lastData.lon, gtr.lastData.lat);
                repaintMap();

                GPSTrackFactory.singleton.addTrack(gtr);
                GPSTrackFactory.singleton.infoForm(gtr).Visible = true;
                GPSTrackFactory.singleton.rebuildMenuStrip(trackStripMenuItem.DropDown.Items);

                GPSTrackFactory.singleton.currentTrack = gtr;
            }
            catch (Exception ex)
            {
                if(trackInfo.stype == GPS.TrackFileInfo.SourceType.FileName)
                    MessageBox.Show("Could not load track:\n" + trackInfo.fileOrBuffer + "\nReason:\n" +
                        ex.ToString(), "Error loading track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Could not load track from the buffer\nReason:\n" +
                        ex.ToString(), "Error loading track", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        /// <summary>
        /// Loads track from disk, registers it and shows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadTrackSB_Click(object sender, EventArgs e)
        {
            Forms.TrackLoadDlg openDlg = new Forms.TrackLoadDlg();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                try    
                {
                    List<GPSTrack> glist = GPSTrack.loadTracks(openDlg.FileInfo);
                    if (glist.Count > 1)
                    {
                        folderBrowserDialog.Description = "Your file was splitted into " + glist.Count + " tracks.\nDo you wish to save them into separate files?\nIf so then choose a folder.";
                        folderBrowserDialog.SelectedPath = Path.GetDirectoryName(trackOpenFileDialog.FileName);
                        if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        {
                            string path = folderBrowserDialog.SelectedPath;
                            foreach (GPSTrack gt in glist)
                            {
                                string fnameonly = Path.GetFileName(gt.track_name);
                                string fname = Path.Combine(path, fnameonly+".gpx");
                                gt.save(new GPS.TrackFileInfo(fname, GPS.TrackFileInfo.SourceType.FileName),
                                    BookMarkFactory.singleton, Bookmarks.POIGroupFactory.singleton());
                            }
                        }
                    }

                    Color base_color = openDlg.FileInfo.trackColor;

                    foreach (GPSTrack gtr in glist)
                    {
                        gtr.need_arrows = false;
                        gtr.trackMode = GPSTrack.TrackMode.ViewSaved;
                        gtr.map = mapo;
                        gtr.trackColor = base_color;

                        base_color = MapDrawControl.screwColor(base_color);

                        mapo.addSub(gtr);
                        gtr.initGLData();
                        gtr.show();

                        mapo.CenterMapLonLat(gtr.lastData.lon, gtr.lastData.lat);
                        repaintMap();

                        GPSTrackFactory.singleton.addTrack(gtr);
                        if(openDlg.FileInfo.showInfoForm)
                            GPSTrackFactory.singleton.infoForm(gtr).Visible = true;
                        GPSTrackFactory.singleton.rebuildMenuStrip(trackStripMenuItem.DropDown.Items);

                        opt.gps_follow_map = false;
                        gpsFollowMapTBut.Checked = false;
                        GPSTrackFactory.singleton.currentTrack = gtr;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load track:\n" + trackOpenFileDialog.FileName + "\nReason:\n" +
                        ex.ToString(), "Error loading track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            openDlg.Dispose();
        }

        /// <summary>
        /// Start recording this track, i.e. make it current
        /// and continue records
        /// </summary>
        /// <param name="to_record"></param>
        void gtrform_onRecord(GPSTrackInfoForm to_record)
        {
            GPSTrack gtr = to_record.track;
            gtr.trackColor = Color.Red;
            gtr.trackMode = GPSTrack.TrackMode.RecordingTrack;
            gtr.need_arrows = true;
            gpsTrackTBut.Checked = true;
            gpsTrackOnAir.Checked = true;
            recordTrackMI.Checked = true;
            gtr.show();
            gtrack.hide();
            mapo.delSub(gtrack);
            ///TODO: we need also change gtrack_mini for minimap
            gtrack = gtr;
            GPSTrackFactory.singleton.currentTrack = gtr;
            GPSTrackFactory.singleton.recordingTrack = gtr;
            mapo.CenterMapLonLat(gtr.lastData.lon, gtr.lastData.lat);
            repaintMap();
        }

        /// <summary>
        /// Handler that remove only one track according to the track info window
        /// from that its called
        /// </summary>
        /// <param name="to_remove"></param>
        void gtrform_onRemove(GPSTrackInfoForm to_remove)
        {
            GPSTrackFactory.singleton.delTrack(to_remove.track);
            GPSTrackFactory.singleton.rebuildMenuStrip(trackStripMenuItem.DropDown.Items);
            gtrform_onRemoveAll(to_remove);
            repaintMap();
        }

        void gtrform_OnRemoveByTrack(GPSTrack track)
        {
            gtrform_onRemove(GPSTrackFactory.singleton.infoForm(track));
        }

        /// <summary>
        /// Removes single track from our factory and others.
        /// Used for removing all tracks repeatedly
        /// </summary>
        /// <param name="to_remove"></param>
        void gtrform_onRemoveAll(GPSTrackInfoForm to_remove)
        {
            if (to_remove.track == gtrack ||
                to_remove.track == manualMode.manualTrack)
                return;

            to_remove.track.hide();
            to_remove.track.Dispose();
            mapo.delSub(to_remove.track);
            if (GPSTrackFactory.singleton.currentTrack == to_remove.track)
                GPSTrackFactory.singleton.currentTrack = gtrack;
            if (GPSTrackFactory.singleton.recordingTrack == to_remove.track)
                GPSTrackFactory.singleton.recordingTrack = gtrack;
            to_remove.Dispose();
        }

        /// <summary>
        /// Handler that is called when we choose track from menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackChoose_Click(object sender, EventArgs e)
        {
            MenuItemObject mo = sender as MenuItemObject;
            if (mo != null)
            {
                GPSTrack gtr = ((GPSTrackInfoForm)mo.data_object).track;
                gtr.textInfo.fill_all_info(gtr);
                GPSTrackFactory.singleton.currentTrack = gtr;
                if(gtr.lastData != null)
                    mapo.CenterMapLonLat(gtr.lastData.lon, gtr.lastData.lat);
                repaintMap();
                ((GPSTrackInfoForm)mo.data_object).initData();
                ((GPSTrackInfoForm)mo.data_object).Visible = true;
            }
        }

        /// <summary>
        /// Handler that calls when we press Remove track button,
        /// and it removes our track.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeTracksSB_Click(object sender, EventArgs e)
        {
            GPSTrackFactory.singleton.clearTracks(gtrform_onRemoveAll);
            GPSTrackFactory.singleton.addTrack(gtrack);
            GPSTrackFactory.singleton.addTrack(manualMode.manualTrack);
            GPSTrackFactory.singleton.rebuildMenuStrip(trackStripMenuItem.DropDown.Items);
            repaintMap();
            GPSTrackFactory.singleton.currentTrack = gtrack;
        }

        #endregion

        private void autoClearTimer_Tick(object sender, EventArgs e)
        {
            Program.Log("Auto-cleaning event: clear map resources and logs if necessary");
            mapo.autoClear(100); //clear all tiles except those which were used in last 100 updates
            logWin.autoClear(3000); //clear after 3000 debug lines
            Program.Log("Auto-cleaning event: done");
        }

        private void rebuildAllMI_Click(object sender, EventArgs e)
        {
            ImgCacheManager.singleton.rebuildCacheAll();
        }

        private void rebuildMapTypeMI_Click(object sender, EventArgs e)
        {
            ImgCacheManager.singleton.rebuildCacheType(opt.mapType);
        }

        private void rebuildMapOnlyMI_Click(object sender, EventArgs e)
        {
            ImgCacheManager.singleton.cache(opt.mapType, opt.cur_zoom_lvl).rebuildCache();
        }

        private void backwardSBut_Click(object sender, EventArgs e)
        {
            backHistoryPos_Key(null, null);
        }

        private void forwardSBut_Click(object sender, EventArgs e)
        {
            naviMode.fwdHistoryPosition();
        }

        private void showBlockFNameMI_Click(object sender, EventArgs e)
        {
            opt.show_fname_on_image = showBlockFNameMI.Checked;
            mapo.ResetCache();
            repaintMap();
        }

        private void reloadFromSiteMI_Click(object sender, EventArgs e)
        {
            mapo.reloadScreen();
            repaintMap();
        }

        private void hideAllBMarksMI_Click(object sender, EventArgs e)
        {
            foreach (BookMarkFactory.TStripBookItem bitem in BookMarkFactory.singleton.menuItems)
            {
                if (bitem != null)
                {
                    bitem.Checked = false;
                    BookMarkFactory.singleton.tstrip_Click(bitem, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Adds new POI to the map, opens Create POI dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBookMI_Click(object sender, EventArgs e)
        {
            double lon, lat, alt;
            bool isFullMode = true;

            if (mode != UserAction.Navigate)
                return;

            GML.tranBegin();

            if (gtrack.on_air && gtrack.lastData != null)
            {
                lon = gtrack.lastData.lon;
                lat = gtrack.lastData.lat;
                alt = gtrack.lastData.height;
                isFullMode = false;
            }
            else
            {
                lon = naviMode.upos.Lon;
                lat = naviMode.upos.Lat;
                alt = 0;
                centerMapLonLat(lon, lat);
            }

            Bookmark mypoi = new Bookmark(lon, lat, alt);
            BookMarkFactory.singleton.register(mypoi);
            mypoi.IsShown = true;
            mypoi.IsAutoShow = false;
            repaintMap();

            GML.tranEnd();

            if (isFullMode)
            {
                Forms.AddPOIForm addpoi = new Forms.AddPOIForm(Bookmarks.POIGroupFactory.singleton(),
                                                        BookMarkFactory.singleton, mypoi);
                addpoi.Owner = this;
                if (addpoi.ShowDialog() != DialogResult.OK)
                    mypoi.unregisterMe();
            }
            else
            {
                Forms.QuickPOIForm qpoi = new Forms.QuickPOIForm(mypoi);
                qpoi.Owner = this;
                if (qpoi.ShowDialog() != DialogResult.OK)
                {
                    mypoi.unregisterMe();
                }

            }
            repaintMap();
        }

        private void editBMarksMI_Click(object sender, EventArgs e)
        {
            Forms.EditBooks editbooks = new GMView.Forms.EditBooks(BookMarkFactory.singleton,
                                                                Bookmarks.POIGroupFactory.singleton());
            editbooks.Visible = true;
        }

        private void windRoseMI_Click(object sender, EventArgs e)
        {
            opt.show_wind_rose = windRoseMI.Checked;
            opt.Save();
            if (opt.show_wind_rose)
                wind_rose.show();
            else
                wind_rose.hide();
            repaintMap();
        }

        private void zoomDashMI_Click(object sender, EventArgs e)
        {
            if (showZoomMI.Checked)
            {
                showZoomMI.Checked = false;
                zoomdash.mode = DashMode.Wrapped;
                opt.show_zoom_panel = false;
            }
            else
            {
                showZoomMI.Checked = true;
                zoomdash.mode = DashMode.Normal;
                opt.show_zoom_panel = true;
            }
            repaintMap();
            opt.Save();
        }

        private void reinitGPSMI_Click(object sender, EventArgs e)
        {
            nmea_thread.opt_onChanged(); //reinit com port again
        }

        private void exportPOIMI_Click(object sender, EventArgs e)
        {
            trackSaveFileDialog.FileName = "points_of_interest.gpx";
            trackSaveFileDialog.Title = "Save POI to a file";
            trackSaveFileDialog.Filter = "GPX unified files|*.gpx|KML google files|*.kml|All files|*.*";
            trackSaveFileDialog.DefaultExt = "gpx";
            if (trackSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                GPS.TrackFileInfo finfo = new GPS.TrackFileInfo(trackSaveFileDialog.FileName,
                                            GMView.GPS.TrackFileInfo.SourceType.FileName);
                finfo.FileType = Path.GetExtension(trackSaveFileDialog.FileName).ToLower();
                if (string.IsNullOrEmpty(finfo.FileType))
                    finfo.FileType = "gpx";
                else
                    finfo.FileType = finfo.FileType.Remove(0, 1);

                try
                {
                    BookMarkFactory.singleton.exportTo(finfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not export POIs:\n" + trackSaveFileDialog.FileName + "\nReason:\n" +
                        ex.ToString(), "Error exporting POIs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void importPOIMI_Click(object sender, EventArgs e)
        {
            Forms.POILoadDlg poiLoadDlg = new Forms.POILoadDlg();
            if (poiLoadDlg.ShowDialog() == DialogResult.OK)
            {
                string fname = poiLoadDlg.FileInfo.FileName;
                try
                {
                    int imported = BookMarkFactory.singleton.importFrom(poiLoadDlg.FileInfo);
                    if (imported > 0)
                    {
                        MessageBox.Show("Successfully loaded " + imported + " items from \n" +
                        Path.GetFileName(fname) + "\ninto " + Path.GetFileNameWithoutExtension(poiLoadDlg.FileInfo.fileOrBuffer)
                                        + " POI group", "POI loading results",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show(
                                (imported == 0 ? "There is no POI information in the file."
                                : "Unrecognized format of the given file."), "POI loading results",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load waypoints:\n" + fname + "\nReason:\n" +
                        ex.ToString(), "Error loading POIs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            poiLoadDlg.Dispose();
        }

        #endregion

        private void mouseOverTimer_Tick(object sender, EventArgs e)
        {
            trackinformer.doAction(GML.translateAbsToScene(currentMode.mouse_last_p));
        }

        internal void trackInformerStop()
        {
            mouseOverTimer.Stop();
            trackinformer.hide();
            GML.repaint();
        }

        private void trafficTbut_CheckedChanged(object sender, EventArgs e)
        {
            opt.showTraffic = !opt.showTraffic;
            trafficTbut.Checked = opt.showTraffic;
            trafficTbut.Image = opt.showTraffic ? global::GMView.Properties.Resources.trafOn
                : global::GMView.Properties.Resources.trafOff;
            mapo.ResetCache();
            minimapo.ResetCache();
            repaintMap();
        }

        /// <summary>
        /// Opens GeoTagging window. Window where we select photos and assign GPS posititon to them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeoTagMI_Click(object sender, EventArgs e)
        {
            geoTagForm.show(this.DesktopLocation.X + this.Size.Width - geoTagForm.Size.Width,
                    this.DesktopLocation.Y + this.Size.Height - geoTagForm.Size.Height);
        }

        private void pasteFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string clipbuffer = Clipboard.GetText();

                loadTrack(new GPS.TrackFileInfo(clipbuffer, GPS.TrackFileInfo.SourceType.StringBuffer));
            }
        }

        private void pastePOIFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                try
                {
                    GPS.TrackFileInfo info = new GPS.TrackFileInfo(Clipboard.GetText(),
                                                 GPS.TrackFileInfo.SourceType.StringBuffer);
                    int imported = BookMarkFactory.singleton.importFrom(info);
                    if (imported > 0)
                    {
                        MessageBox.Show("Successfully loaded " + imported + " items from Clipboard\ninto '"
                                + info.fileOrBuffer
                                + "' POI group", "POI loading results",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                                (imported == 0 ? "There is no POI information in the clipboard."
                                : "Unrecognized format of the input buffer."), "POI loading results",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load POI from clipboard:\nReason:\n" +
                        ex.ToString(), "Error loading POIs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal void setOnClickLonLat(double lon, double lat)
        {
            lonlatSLab.Text = "Lat: " + lat.ToString("F3") + " Lon: " + lon.ToString("F3");
            if (onClickLonLat != null)
                onClickLonLat(lon, lat);
        }

    }
}
