using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace XnGFL
{
    public partial class ExifViewControl : UserControl
    {
        public ExifViewControl()
        {
            InitializeComponent();
            items = new ListView.ListViewItemCollection(dirView);

            imageLoader = new ncUtils.GUIWorkerThread<ExifImageLoader>(this);
            imageLoader.taskCompleted += loadingComplete;
            imageLoader.start();

            batchUpdater = new EXIFBatchUpdate(this);
            batchUpdater.onBatchFinished += batchUpdateFinished;
            batchUpdater.onOneUpdate += itemUpdated;
            batchUpdater.start();

            deltaTimeTbox.ValidatingType = typeof(TimeSpan);
        }

        private ncUtils.GUIWorkerThread<ExifImageLoader> imageLoader = null;

        /// <summary>
        /// Puts all loaded images into the ListView on task completion
        /// </summary>
        /// <param name="task"></param>
        private void loadingComplete(ExifImageLoader task)
        {
            loadingPartiallyDone(task, task.resultList);
            filesGBox.Text = "Files [" + items.Count + "]:";
        }

        /// <summary>
        /// We are loaded part of the images, lets show them
        /// </summary>
        /// <param name="task"></param>
        /// <param name="doneList"></param>
        private void loadingPartiallyDone(ExifImageLoader task, List<Image> doneList)
        {
            dirView.BeginUpdate();
            foreach (Image img in doneList)
            {
                itemList.Add(img);
                items.Add(img);
            }
            dirView.EndUpdate();
            filesGBox.Text = "Files [Loading " + items.Count + "...]:";
        }


        /// <summary>
        /// Object that process all updates to the files that were modified
        /// </summary>
        private EXIFBatchUpdate batchUpdater = null;

        /// <summary>
        /// After every file update we move progress bar
        /// </summary>
        /// <param name="dat"></param>
        private void itemUpdated(EXIFData dat)
        {
            batchProgBar.Value++;
        }

        /// <summary>
        /// Update of files finished, reenable controls
        /// </summary>
        private void batchUpdateFinished()
        {
            applyFilesBut.Enabled = true;
            cancelShedBut.Enabled = true;
            dirView.Invalidate();
            progressLbl.Text = "Done";
        }

        /// <summary>
        /// Opens dialog for choosing directory with images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openDirBut_Click(object sender, EventArgs e)
        {
            if (dirTBox.Text.Length > 0)
            {
                openFileDialog.InitialDirectory = dirTBox.Text;
            }

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            dirTBox.Text = Path.GetDirectoryName(openFileDialog.FileName);

            refreshDirBut_Click(sender, e);
        }

        /// <summary>
        /// CLears ListView and frees all image resources
        /// </summary>
        private void clearList()
        {
            dirView.Clear();
            foreach (Image img in itemList)
            {
                img.Dispose();
            }
            itemList.Clear();
        }

        private ListView.ListViewItemCollection items;
        private List<Image> itemList = new List<Image>();
        private Dictionary<string, XnGFL.Image> imageMap = new Dictionary<string, XnGFL.Image>();

        #region Draw methods and variables
        private int imgWidth = 160;
        private int imgHeight = 120;

        private Font textFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
        private Font textFont2 = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
        private Brush cellBrush = Brushes.Yellow;
        private Brush textBrush1 = Brushes.WhiteSmoke;
        private Brush textBrush2 = new SolidBrush(Color.FromArgb(230, 230, 120));
        private Brush textBrush3 = new SolidBrush(Color.FromArgb(255, 192, 128));
        private Pen focusPen = new Pen(Brushes.Yellow, 2);
        private Pen selectPen = new Pen(Brushes.White, 2);

        private int lonTextWidth = 0;
        /// <summary>
        /// Custom draw method for ListView. We draw thumbnail + exif info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dirView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            XnGFL.Image img = (XnGFL.Image) e.Item;
            e.DrawBackground();
            int x = e.Bounds.X + imgWidth + 4;
            int y = e.Bounds.Y + 2;
            e.Graphics.DrawImage(img.image, e.Bounds.X, e.Bounds.Y, imgWidth, imgHeight);
            e.Graphics.DrawString(img.Text, textFont, textBrush1, (float)x, (float)y);
            y += textFont.Height;
            
            if (img.exif != null)
            {
                e.Graphics.DrawString(img.exif.dateTimeOriginal.ToString("dd-MM-yyyy HH:mm:ss"), textFont2, textBrush2,
                    (float)x, (float)y);
                y += textFont2.Height;
                e.Graphics.DrawString(img.exif.exposureInfo, textFont2, textBrush2,
                    (float)x, (float)y);
                y += textFont2.Height;
                e.Graphics.DrawString(img.exif.flash, textFont2, textBrush2,
                    (float)x, (float)y);
                y += textFont2.Height;

                //GPS

                if (img.exif.hasGPS)
                {

                    if (lonTextWidth == 0)
                        lonTextWidth = (int)e.Graphics.MeasureString("Longitude:WW", textFont2, 16384, System.Drawing.StringFormat.GenericTypographic).Width;

                    e.Graphics.DrawString("GPS info: ", textFont2, textBrush3,
                        (float)x, (float)y);
                    e.Graphics.DrawString(img.exif.gpsVersion, textFont2, textBrush3,
                        (float)x + lonTextWidth, (float)y);
                    y += textFont2.Height;
                    e.Graphics.DrawString("Latitude: ", textFont2, textBrush3,
                        (float)x, (float)y);
                    e.Graphics.DrawString(img.exif.gpsLatString, textFont2, textBrush3,
                        (float)x + lonTextWidth, (float)y);
                    y += textFont2.Height;
                    e.Graphics.DrawString("Longitude: ", textFont2, textBrush3,
                        (float)x, (float)y);
                    e.Graphics.DrawString(img.exif.gpsLonSting, textFont2, textBrush3,
                        (float)x + lonTextWidth, (float)y);
                    y += textFont2.Height;
                    e.Graphics.DrawString("Altitude: ", textFont2, textBrush3,
                        (float)x, (float)y);
                    e.Graphics.DrawString(img.exif.gpsAltString, textFont2, textBrush3,
                        (float)x + lonTextWidth, (float)y);
                    y += textFont2.Height;
                }
                else
                {
                    e.Graphics.DrawString("No GPS info", textFont2, Brushes.Red,
                        (float)x, (float)y);
                    y += textFont2.Height;
                }

                Bitmap icon;
                if (img.exif.hasModified)
                    icon = global::XnGFL.Properties.Resources.lamp;
                else
                    if (img.exif.hasGPS)
                        icon = global::XnGFL.Properties.Resources.gtk_ok;
                    else
                        icon = global::XnGFL.Properties.Resources.gtk_no;

                e.Graphics.DrawImage(icon, e.Bounds.Right - 22, e.Bounds.Y + 4);
            }

            if (img.Focused)
                e.Graphics.DrawRectangle(focusPen, e.Bounds.X +1, e.Bounds.Y +1, e.Bounds.Width - 2, e.Bounds.Height - 2);
            else
            if (img.Selected)
                e.Graphics.DrawRectangle(selectPen, e.Bounds.X +1, e.Bounds.Y +1, e.Bounds.Width - 2, e.Bounds.Height - 2);
        }

        #endregion

        private int abs(int val)
        {
            return (int)Math.Abs(val);
        }

        /// <summary>
        /// Sets time delta interval from GPS and Shot date difference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setDeltaFromGPSBut_Click(object sender, EventArgs e)
        {
            long gpsticks = gpsDatePicker.Value.Ticks / 10000000L;
            long shotticks = shotDatePicker.Value.Ticks / 10000000L;

            TimeSpan delta = new TimeSpan((gpsticks - shotticks) * 10000000L);

            setDeltaTime(delta);
        }

        /// <summary>
        /// Sets delta time as diff between GPS and shot time
        /// </summary>
        /// <param name="delta"></param>
        private void setDeltaTime(TimeSpan delta)
        {
            string valstr;
            if (delta.Ticks < 0)
            {
                valstr = "-" + abs(delta.Days).ToString("D2");
            }
            else
                valstr = abs(delta.Days).ToString("D3");
            valstr += abs(delta.Hours).ToString("D2");
            valstr += abs(delta.Minutes).ToString("D2");
            valstr += abs(delta.Seconds).ToString("D2");
            deltaTimeTbox.Text = valstr;
            deltaTime = delta;
        }

        /// <summary>
        /// Holds time span between GPS time and picture shot time
        /// </summary>
        private TimeSpan deltaTime = new TimeSpan();

        private void deltaTimeTbox_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                MessageBox.Show("Invalid format of time span. Please, enter Days.Hours:Minutes:Seconds");
                return;
            }
            deltaTime = (TimeSpan)e.ReturnValue;
        }

        /// <summary>
        /// Sets time span via combo box with predefine delta hours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hourSetCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int start_idx = -8;

            setDeltaTime(new TimeSpan((long)(start_idx + hourSetCB.SelectedIndex) * 3600L * 10000000L));
        }

        /// <summary>
        /// Called whem we double click on the item in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dirView_DoubleClick(object sender, EventArgs e)
        {
            Image img = dirView.FocusedItem as Image;
            if (img == null)
                return;

            manualDatePicker.Value = img.exif.dateTimeOriginal;
            shotDatePicker.Value = img.exif.dateTimeOriginal;

            if (img.exif.hasGPS)
            {
                setLonLatFromMap(img.exif.gpsLon, img.exif.gpsLat);
                if (needCenteringLonLat != null)
                    needCenteringLonLat(img.exif.gpsLon, img.exif.gpsLat);
            }
        }

        /// <summary>
        /// We store modified entries here
        /// </summary>
        private Dictionary<string, EXIFData> modifiedList = new Dictionary<string, EXIFData>();

        /// <summary>
        /// Adds or update information in the modified list (items that need to be saved)
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private bool addModified(Image img)
        {
            if (!img.exif.isModified)
                return false;

            if (modifiedList.ContainsKey(img.filename))
            {
                modifiedList[img.filename] = img.exif;
            }
            else
                modifiedList.Add(img.filename, img.exif);
            return true;
        }

        /// <summary>
        /// Here we will assign new values from the manual tab page to the selected images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assignManualBut_Click(object sender, EventArgs e)
        {
            DateTime dtnew = manualDatePicker.Value;
            bool needShotDate = needShotDateCB.Checked;
            double lon = 0, lat = 0;
            bool needGPS = false;

            try
            {
                lon = ncUtils.Glob.parseLonLat(manualLonTBox.Text);
                lat = ncUtils.Glob.parseLonLat(manualLatTBox.Text);
                needGPS = needGPSCb.Checked;
            }
            catch { }

            if (!needGPS && !needShotDate)
            {
                MessageBox.Show("Please, check one of the boxes on the right, GPS or Shot Date.", "Nothing to assign", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (ListViewItem item in dirView.SelectedItems)
            {
                Image img = item as Image;
                if (img == null)
                    continue;
                if(needShotDate)
                    img.exif.dateTimeOriginal = dtnew;
                if (needGPS)
                {
                    img.exif.setGPS(lon, lat, 0);
                }

                addModified(img);
            }

            progressLbl.Text = "Scheduled: " + modifiedList.Count + " images";
            dirView.Invalidate();
        }

        /// <summary>
        /// Executes batch update for modified files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyFilesBut_Click(object sender, EventArgs e)
        {
            int count = batchUpdater.addBatch(modifiedList);
            if (count > 0)
            {
                modifiedList = new Dictionary<string, EXIFData>();
                batchProgBar.Maximum = count;
                batchProgBar.Value = 0;
                applyFilesBut.Enabled = false;
                cancelShedBut.Enabled = false;
                progressLbl.Text = "Updating: " + count + " images";
            }
        }

        /// <summary>
        /// Execute on shutdown the application, stops all threads
        /// </summary>
        public void shutdown()
        {
            batchUpdater.stop();
            imageLoader.stop();
        }

        /// <summary>
        /// Delegate for calling when we need to center map on given lon, lat position
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public delegate void needCenteringLonLatDelegate(double lon, double lat);
        /// <summary>
        /// Event fired, when we need to center map on given lon, lat
        /// </summary>
        public event needCenteringLonLatDelegate needCenteringLonLat;

        /// <summary>
        /// LOn, lat coordinates for manual tab
        /// </summary>
        private double manual_lon, manual_lat;

        /// <summary>
        /// Sets lon, lat coordinates in manual tab for later assigning
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public void setLonLatFromMap(double lon, double lat)
        {
            manual_lat = lat;
            manual_lon = lon;
            manualLatTBox.Text = lat.ToString("F6", ncUtils.Glob.numformat);
            manualLonTBox.Text = lon.ToString("F6", ncUtils.Glob.numformat);

            ncGeo.IGPSTrack track = trackListCB.SelectedItem as ncGeo.IGPSTrack;
            if (track != null)
            {
                ncGeo.FindNearestPointByDistance ctx = new ncGeo.FindNearestPointByDistance(lon, lat);
                track.findNearest(ctx);
                if (ctx.distance < 20.0 && ctx.resultPoint != null)
                {
                    gpsDatePicker.Value = ctx.resultPoint.Value.utc_time.ToLocalTime();
                }
            }
        }

        /// <summary>
        /// Cancel all scheduled, but not executed tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelShedBut_Click(object sender, EventArgs e)
        {
            modifiedList.Clear();
            progressLbl.Text = "Cleared";
        }

        /// <summary>
        /// Fills in the POI combobox
        /// </summary>
        /// <param name="dic"></param>
        public void setPOIs(ncGeo.IGeoCoord[] items)
        {
            bookmarkCB.Items.Clear();
            if(items != null)
                bookmarkCB.Items.AddRange(items);
            bookmarkCB.SelectedItem = null;
        }

        /// <summary>
        /// Fills track list combobox
        /// </summary>
        /// <param name="items"></param>
        public void setTracks(ncGeo.IGPSTrack[] items)
        {
            trackListCB.Items.Clear();
            if(items != null)
                trackListCB.Items.AddRange(items);
            trackListCB.SelectedItem = null;
        }

        private void bookmarkCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ncGeo.IGeoCoord item = bookmarkCB.SelectedItem as ncGeo.IGeoCoord;
            if (item != null && needCenteringLonLat != null)
            {
                needCenteringLonLat(item.longitude, item.latitude);
            }
        }

        /// <summary>
        /// Asiigns new coordinates to selected items by searching them on GPS track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assignGPSBut_Click(object sender, EventArgs e)
        {
            bool needDateCorrection = needDeltaTimeCB.Checked;

            ncGeo.IGPSTrack track = trackListCB.SelectedItem as ncGeo.IGPSTrack;

            if (track == null)
            {
                MessageBox.Show("Please, select the track first!");
                return;
            }

            foreach (ListViewItem item in dirView.SelectedItems)
            {
                Image img = item as Image;
                if (img == null)
                    continue;
                DateTime newDate = img.exif.dateTimeOriginal + deltaTime;

                ncGeo.FindNearestPointByTime ctx = new ncGeo.FindNearestPointByTime(newDate);
                track.findNearest(ctx);
                if (ctx.timeSpan.TotalMinutes > 1 || ctx.resultPoint == null)
                    continue;

                img.exif.setGPS(ctx.resultPoint.Value.lon, ctx.resultPoint.Value.lat, ctx.resultPoint.Value.height);

                if (needDateCorrection)
                    img.exif.dateTimeOriginal = newDate;

                addModified(img);
            }
            progressLbl.Text = "Scheduled: " + modifiedList.Count + " images";
            dirView.Invalidate();
        }

        /// <summary>
        /// Called by GUI when we select another track from the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackListCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ncGeo.IGPSTrack track = trackListCB.SelectedItem as ncGeo.IGPSTrack;
            if (track == null)
                return;

            if(track.countPoints > 0)
                gpsDatePicker.Value = track.startTime;
        }

        /// <summary>
        /// Called when clicked on map with new lon,lat
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public void clickLonLat(double lon, double lat)
        {

        }

        /// <summary>
        /// Refreshes the list view - reloads images from disk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshDirBut_Click(object sender, EventArgs e)
        {
            if (dirTBox.Text.Length == 0)
                return;

            clearList();

            ExifImageLoader loader = new ExifImageLoader(this, dirTBox.Text, imgWidth, imgHeight);
            loader.onPartialCompletion = this.loadingPartiallyDone;
            imageLoader.addTask(loader);

            filesGBox.Text = "Files: [Loading ...]";
        }

        private ImagePreview imview = new ImagePreview();

        private void dirView_MouseUp(object sender, MouseEventArgs e)
        {
            imview.hideImage();
        }

        private void dirView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < imgWidth)
            {
                Image img = dirView.GetItemAt(e.X, e.Y) as Image;
                if (img != null)
                {
                    
                    imview.showImage(img.filename);
                    imview.SetDesktopLocation(this.ParentForm.DesktopLocation.X 
                        + this.ParentForm.Size.Width - imview.Size.Width,
                        this.ParentForm.DesktopLocation.Y);
                }
            }
        }
    }
}
