using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    partial class GMViewForm
    {
        private Point mouse_start_p = new Point();
        private Point mouse_delta_p = new Point();
        private Point mouse_press_p = new Point();
        private Point mouse_release_p = new Point();
        private Point mouse_last_p = new Point();

        public delegate void onMouseActionDelegate(Point mouse_p);

        //mouse events that raise in navigation mode
        public event onMouseActionDelegate onNaviLeftClick;
        public event onMouseActionDelegate onNaviLeftDoubleClick;
        public event onMouseActionDelegate onNaviLeftMove;
        public event onMouseActionDelegate onNaviLeftDown;
        public event onMouseActionDelegate onNaviLeftUp;
        public event onMouseActionDelegate onNaviRightUp;

        public event onMouseActionDelegate onZoomLeftClick;
        public event onMouseActionDelegate onZoomLeftDoubleClick;
        public event onMouseActionDelegate onZoomLeftMove;
        public event onMouseActionDelegate onZoomLeftDown;
        public event onMouseActionDelegate onZoomLeftUp;

        public event onMouseActionDelegate onSelectLeftClick;
        public event onMouseActionDelegate onSelectLeftDoubleClick;
        public event onMouseActionDelegate onSelectLeftMove;
        public event onMouseActionDelegate onSelectLeftDown;
        public event onMouseActionDelegate onSelectLeftUp;

        public event onMouseActionDelegate onMTrackLeftClick;
        public event onMouseActionDelegate onMTrackRightClick;
        public event onMouseActionDelegate onMTrackLeftDoubleClick;
        public event onMouseActionDelegate onMTrackLeftMove;
        public event onMouseActionDelegate onMTrackLeftDown;
        public event onMouseActionDelegate onMTrackLeftUp;

        public event MethodInvoker onLeaveNaviMode;
        public event MethodInvoker onLeaveZoomMode;
        public event MethodInvoker onLeaveSelectMode;
        public event MethodInvoker onLeaveMTrackMode;

        public event MethodInvoker onEnterNaviMode;
        public event MethodInvoker onEnterZoomMode;
        public event MethodInvoker onEnterSelectMode;
        public event MethodInvoker onEnterMTrackMode;

        #region drawPane hook methods

        const int autoScrollDelta = 15;
        //===========================================================================
        /// <summary>
        /// Registers mouse movements and produce nessecary actions
        /// Currently it allows us to drag map in different directions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawPane_MouseMove(object sender, MouseEventArgs e)
        {
            mouse_last_p.X = e.X;
            mouse_last_p.Y = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                Point mouse_cur_p = new Point(e.X, e.Y);
                mouse_delta_p.X = mouse_cur_p.X - mouse_start_p.X;
                mouse_delta_p.Y = mouse_cur_p.Y - mouse_start_p.Y;
                if (mouse_delta_p.X > 8 || mouse_delta_p.X < -8 ||
                    mouse_delta_p.Y > 8 || mouse_delta_p.Y < -8)
                {
                    mouse_start_p = mouse_cur_p;
                    Program.Log("drawPane - mouse drag at pos: " + e.X + ", " + e.Y);
                    if (boards.mouseMove(mouse_cur_p))
                    {
                        repaintMap();
                        return;
                    }
                    switch (mode)
                    {
                        case UserAction.Navigate:
                            if (onNaviLeftMove != null)
                            {
                                //here we pass delta xy, not a real coordinates
                                onNaviLeftMove(GML.translateToScene(mouse_delta_p));
                                repaintMap();
                            }
                            break;
                        case UserAction.Zoom:
                            if (onZoomLeftMove != null)
                            {
                                onZoomLeftMove(GML.translateToScene(mouse_delta_p));
                                repaintMap();
                            }
                            break;
                        case UserAction.SelectArea:
                            if (onSelectLeftMove != null)
                            {
                                onSelectLeftMove(GML.translateToScene(mouse_delta_p));
                                repaintMap();
                            }
                            break;
                        case UserAction.ManualTrack:
                            if (onMTrackLeftMove != null)
                            {
                                onMTrackLeftMove(GML.translateAbsToScene(mouse_last_p));
                                repaintMap();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
                if (e.Button == MouseButtons.None)
                {
                    if (mode != UserAction.Navigate)
                    {
                        if (mouse_last_p.Y < autoScrollDelta || mouse_last_p.Y > drawPane.Size.Height - autoScrollDelta ||
                            mouse_last_p.X < autoScrollDelta || mouse_last_p.X > drawPane.Size.Width - autoScrollDelta)
                            autoScrollStart();
                        else
                            autoScrollStop();

                    }
                    else
                    {
                        mouseOverTimer.Stop();
                        mouseOverTimer.Start();
                    }
                }
        }

        void autoScrollStart()
        {
            if (inAutoScroll)
                return;
            inAutoScroll = true;
            runMeOnce(autoScrollDoIt, 300);
        }

        void autoScrollDoIt()
        {
            bool matched = false;
            if (mouse_last_p.Y < autoScrollDelta)
            {
                moveMapUp_Key(this, null);
                matched = true;
            }
            if (mouse_last_p.X < autoScrollDelta)
            {
                moveMapLeft_Key(this, null);
                matched = true;
            }
            if (mouse_last_p.Y > drawPane.Size.Height - autoScrollDelta)
            {
                moveMapDown_Key(this, null);
                matched = true;
            }
            if (mouse_last_p.X > drawPane.Size.Width - autoScrollDelta)
            {
                moveMapRight_Key(this, null);
                matched = true;
            }

            if (matched)
                runMeOnce(autoScrollDoIt, 100);
            else
                autoScrollStop();
        }

        void autoScrollStop()
        {
            if (!inAutoScroll)
                return;

            runOnceTimer.Stop();
            runOnce.Clear();
            inAutoScroll = false;
        }

        void drawPane_MouseHover(object sender, EventArgs e)
        {
        }

        void drawPane_MouseLeave(object sender, EventArgs e)
        {
            autoScrollStop();
            trackInformerStop();
        }

        private void drawPane_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_press_p.X = e.X;
            mouse_press_p.Y = e.Y;
            miniform.doSync = false;
            GML.tranBegin();
            try
            {
                trackInformerStop();
                if (e.Button == MouseButtons.Left)
                {
                    mouse_start_p.X = e.X;
                    mouse_start_p.Y = e.Y;
                    Program.Log("Mouse drag start: " + e.X + ", " + e.Y + " -> " + mouse_start_p.ToString());

                    if (boards.mouseDown(mouse_start_p))
                    {
                        repaintMap();
                        miniform.doSync = true;
                        return;
                    }

                    switch (mode)
                    {
                        case UserAction.Navigate:
                            if (onNaviLeftDown != null)
                                onNaviLeftDown(GML.translateAbsToScene(mouse_start_p));
                            break;
                        case UserAction.Zoom:
                            if (onZoomLeftDown != null)
                                onZoomLeftDown(GML.translateAbsToScene(mouse_start_p));
                            break;
                        case UserAction.SelectArea:
                            if (onSelectLeftDown != null)
                                onSelectLeftDown(GML.translateAbsToScene(mouse_start_p));
                            break;
                        case UserAction.ManualTrack:
                            if (onMTrackLeftDown != null)
                                onMTrackLeftDown(GML.translateAbsToScene(mouse_start_p));
                            break;
                    }
                }
                else
                    if (e.Button == MouseButtons.Right)
                    {
                        switch (mode)
                        {
                            case UserAction.ManualTrack:
                                if (onMTrackRightClick != null)
                                    onMTrackRightClick(GML.translateAbsToScene(mouse_start_p));
                                break;
                            default:
                                break;
                        }
                    }
            }
            finally
            {
                GML.tranEnd();
            }
        }

        private void drawPane_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_release_p.X = e.X;
            mouse_release_p.Y = e.Y;
            miniform.doSync = false;
            if (e.Button == MouseButtons.Left)
            {
                if (boards.mouseUp(mouse_release_p))
                {
                    repaintMap();
                    miniform.doSync = true;
                    return;
                }

                switch (mode)
                {
                    case UserAction.Navigate:
                        if (onNaviLeftUp != null)
                            onNaviLeftUp(GML.translateAbsToScene(mouse_release_p));
                        break;
                    case UserAction.Zoom:
                        if (onZoomLeftUp != null)
                            onZoomLeftUp(GML.translateAbsToScene(mouse_release_p));
                        break;
                    case UserAction.SelectArea:
                        if (onSelectLeftUp != null)
                            onSelectLeftUp(GML.translateAbsToScene(mouse_release_p));
                        break;
                    case UserAction.ManualTrack:
                        if (onMTrackLeftUp != null)
                            onMTrackLeftUp(GML.translateAbsToScene(mouse_release_p));
                        break;

                }
            } else
                if (e.Button == MouseButtons.Right)
                {
                    switch (mode)
                    {
                        case UserAction.Navigate:
                            if(onNaviRightUp != null)
                                onNaviRightUp(GML.translateAbsToScene(mouse_release_p));
                            break;
                        default:
                            break;
                    }
                }
            miniform.doSync = true;
            miniform.repaintMap();
        }

        private void drawPane_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                mouse_release_p.X = e.X;
                mouse_release_p.Y = e.Y;
                if (System.Math.Abs(mouse_press_p.X - mouse_release_p.X) +
                    System.Math.Abs(mouse_press_p.Y - mouse_release_p.Y) < 8)
                {
                    if (boards.mouseClick(mouse_release_p))
                    {
                        repaintMap();
                        return;
                    }

                    switch (mode)
                    {
                        case UserAction.Navigate:
                            {
                                if (onNaviLeftClick != null)
                                {
                                    onNaviLeftClick(GML.translateAbsToScene(mouse_release_p));
                                    repaintMap();
                                }

                                double lon, lat;
                                Program.Log("Single click detected at position: " + mouse_release_p.ToString());
                                mapo.getLonLatByVisibleXY(mouse_release_p, out lon, out lat);
                                lonlatSLab.Text = "Lat: " + lat.ToString("F3") + " Lon: " + lon.ToString("F3");
                            }
                            break;
                        case UserAction.Zoom:
                            if (onZoomLeftClick != null)
                            {
                                onZoomLeftClick(GML.translateAbsToScene(mouse_release_p));
                            }
                            break;
                        case UserAction.SelectArea:
                            if (onSelectLeftClick != null)
                            {
                                onSelectLeftClick(GML.translateAbsToScene(mouse_release_p));
                            }
                            break;
                        case UserAction.ManualTrack:
                            if (onMTrackLeftClick != null)
                                onMTrackLeftClick(GML.translateAbsToScene(mouse_release_p));
                            break;
                    }
                }
            }

        }

        void miniform_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double lon, lat;
            Point xy = new Point();
            xy.X = e.X;
            xy.Y = e.Y;
            minimapo.getLonLatByVisibleXY(xy, out lon, out lat);
            mapo.CenterMapLonLat(lon, lat);
            repaintMap();
        }

        private void drawPane_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            mouse_release_p.X = e.X;
            mouse_release_p.Y = e.Y;
            Program.Log("Double click detected at position: " + mouse_release_p.ToString());

            if (boards.mouseDoubleClick(mouse_release_p))
            {
                repaintMap();
                return;
            }

            switch (mode)
            {
                case UserAction.Navigate:
                    if (onNaviLeftDoubleClick != null)
                    {
                        onNaviLeftDoubleClick(GML.translateAbsToScene(mouse_release_p));
                        repaintMap();
                    }
                    break;
                case UserAction.Zoom:
                    if (onZoomLeftDoubleClick != null)
                    {
                        onZoomLeftDoubleClick(GML.translateAbsToScene(mouse_release_p));
                    }
                    break;
                case UserAction.SelectArea:
                    if (onSelectLeftDoubleClick != null)
                    {
                        onSelectLeftDoubleClick(GML.translateAbsToScene(mouse_release_p));
                    }
                    break;
                case UserAction.ManualTrack:
                    if (onMTrackLeftDoubleClick != null)
                        onMTrackLeftDoubleClick(GML.translateAbsToScene(mouse_release_p));
                    break;
            }

        }

        private void drawPane_Resize(object sender, EventArgs e)
        {
            mapo.SetVisibleSize(drawPane.Size);
            mapo.recenterMap();
            repaintMap();
        }

        #endregion

    }
}
