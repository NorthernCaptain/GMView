using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView.UIHelper
{
    /// <summary>
    /// Base processor of mouse events that we receive on the map window
    /// </summary>
    public class MouseBaseProc
    {
        /// <summary>
        /// Main window with map view in it
        /// </summary>
        protected GMViewForm mainform;
        public GMViewForm MainForm
        {
            set
            {
                mainform = value;
            }
        }


        /// <summary>
        /// Dashboard container, we need to send events to it
        /// </summary>
        protected DashBoardContainer boards;
        public DashBoardContainer Boards
        {
            set
            {
                boards = value;
            }
        }

        /// <summary>
        /// DrawPane with the map we operate on.
        /// </summary>
        protected UserControl drawPane;
        public UserControl DrawPane
        {
            set
            {
                drawPane = value;
            }
        }


        public virtual void initGL()
        {

        }

        /// <summary>
        /// Various mouse positions in different stages
        /// </summary>
        protected Point mouse_start_p = new Point();
        protected Point mouse_delta_p = new Point();
        protected Point mouse_press_p = new Point();
        protected Point mouse_release_p = new Point();
        internal Point mouse_last_p = new Point();

        protected bool shiftPressed = false;
        protected bool controlPressed = false;
        protected bool altPressed = false;

        /// <summary>
        /// Last mouse down pressed time in ticks
        /// </summary>
        protected long mouse_down_ticks;
        /// <summary>
        /// Number of milliseconds we consider as normal click, not more.
        /// </summary>
        protected const long normal_click_time = 500; //not more that 500 millis (half a second)
        /// <summary>
        /// Number of pixels mouse should move to start dragging
        /// </summary>
        protected const int drag_start_delta = 6;
        
        /// <summary>
        /// Actual mouse event arguments
        /// </summary>
        protected MouseEventArgs eargs;

        /// <summary>
        /// How many pixels to scroll on each step in autoscroll mode
        /// </summary>
        const int autoScrollDelta = 15;

        /// <summary>
        /// Install our processor for capturing mouse events for draw pane.
        /// </summary>
        public virtual void modeEnter(MouseBaseProc oldone)
        {
            if(oldone != null)
            {
                mouse_start_p = oldone.mouse_start_p;
                mouse_release_p = oldone.mouse_release_p;
                mouse_press_p = oldone.mouse_press_p;
                mouse_last_p = oldone.mouse_last_p;
                mouse_delta_p = oldone.mouse_delta_p;
            }

            drawPane.MouseClick += doMouseClick;
            drawPane.MouseDoubleClick += doMouseDoubleClick;
            drawPane.MouseUp += doMouseUp;
            drawPane.MouseDown += doMouseDown;
            drawPane.MouseMove += doMouseMove;
            drawPane.MouseLeave += doMouseLeave;
            drawPane.MouseWheel += doMouseWheel;
        }

        /// <summary>
        /// Uninstall processor from event capturing
        /// </summary>
        public virtual void modeLeave()
        {
            drawPane.MouseClick -= doMouseClick;
            drawPane.MouseDoubleClick -= doMouseDoubleClick;
            drawPane.MouseUp -= doMouseUp;
            drawPane.MouseDown -= doMouseDown;
            drawPane.MouseMove -= doMouseMove;
            drawPane.MouseLeave -= doMouseLeave;
            drawPane.MouseWheel -= doMouseWheel;
        }

        /// <summary>
        /// Process mouse movement, translated xy. One of the mouse buttons is pressed
        /// </summary>
        /// <param name="xy"></param>
        /// <returns>Return true, if event was processed</returns>
        protected virtual bool onMouseMoveTranslated(Point xy)
        {
            return false;
        }

        /// <summary>
        /// Process mouse movement with one button pressed
        /// </summary>
        /// <param name="xy"></param>
        /// <returns>Return true, if event was processed</returns>
        protected virtual bool onMouseMove(Point xy)
        {
            if (mouse_last_p.Y < autoScrollDelta || mouse_last_p.Y > drawPane.Size.Height - autoScrollDelta ||
                mouse_last_p.X < autoScrollDelta || mouse_last_p.X > drawPane.Size.Width - autoScrollDelta)
                autoScrollStart();
            else
                autoScrollStop();            
            return false;
        }

        protected virtual bool onMouseDown(Point xy)
        {
            mainform.trackInformerStop();
            return false;
        }

        protected virtual bool onMouseDownTranslated(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseUp(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseUpTranslated(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseClick(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseClickTranslated(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseDoubleClick(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseDoubleClickTranslated(Point xy)
        {
            return false;
        }

        protected virtual bool onMouseWheelUp()
        {
            mainform.zoomOut();
            return false;
        }

        protected virtual bool onMouseWheelDown()
        {
            mainform.zoomIn();
            return false;
        }

        protected virtual bool onMouseLeave(Point xy)
        {
            autoScrollStop();
            mainform.trackInformerStop();
            return false;
        }


        protected virtual bool onMouseMoveNoBut(Point xy)
        {
            if (mouse_last_p.Y < autoScrollDelta || mouse_last_p.Y > drawPane.Size.Height - autoScrollDelta ||
                mouse_last_p.X < autoScrollDelta || mouse_last_p.X > drawPane.Size.Width - autoScrollDelta)
                autoScrollStart();
            else
                autoScrollStop();
            return true;
        }

        /// <summary>
        /// Do we in auto scroll mode now or not?
        /// </summary>
        private bool inAutoScroll = false;

        protected virtual void autoScrollStart()
        {
            if (inAutoScroll)
                return;
            inAutoScroll = true;
            RunMeOnce.singleton.runMeOnce(autoScrollDoIt, 300);
        }

        protected virtual void autoScrollStop()
        {
            if (!inAutoScroll)
                return;

            RunMeOnce.singleton.stop();
            inAutoScroll = false;
        }


        protected virtual void autoScrollDoIt()
        {
            bool matched = false;
            if (mouse_last_p.Y < autoScrollDelta)
            {
                mainform.moveMapUp_Key(this, null);
                matched = true;
            }
            if (mouse_last_p.X < autoScrollDelta)
            {
                mainform.moveMapLeft_Key(this, null);
                matched = true;
            }
            if (mouse_last_p.Y > drawPane.Size.Height - autoScrollDelta)
            {
                mainform.moveMapDown_Key(this, null);
                matched = true;
            }
            if (mouse_last_p.X > drawPane.Size.Width - autoScrollDelta)
            {
                mainform.moveMapRight_Key(this, null);
                matched = true;
            }

            if (matched)
                RunMeOnce.singleton.runMeOnce(autoScrollDoIt, 100);
            else
                autoScrollStop();
        }

        //===========================================================================
        /// <summary>
        /// Registers mouse movements and produce necessary  actions
        /// Currently it allows us to drag map in different directions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doMouseMove(object sender, MouseEventArgs e)
        {
            eargs = e;
            fillModifiers();
            mouse_last_p.X = e.X;
            mouse_last_p.Y = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                Point mouse_cur_p = new Point(e.X, e.Y);
                mouse_delta_p.X = mouse_cur_p.X - mouse_start_p.X;
                mouse_delta_p.Y = mouse_cur_p.Y - mouse_start_p.Y;
                if (mouse_delta_p.X > drag_start_delta || mouse_delta_p.X < -drag_start_delta ||
                    mouse_delta_p.Y > drag_start_delta || mouse_delta_p.Y < -drag_start_delta)
                {
                    mouse_start_p = mouse_cur_p;
                    Program.Log("drawPane - mouse drag at pos: " + e.X + ", " + e.Y);
                    if (boards.mouseMove(mouse_cur_p))
                    {
                        GML.repaint();
                        return;
                    }

                    if (onMouseMove(mouse_delta_p))
                        return;
                    if (onMouseMoveTranslated(GML.translateToScene(mouse_delta_p)))
                        return;
                }
            }
            else
                if (e.Button == MouseButtons.None)
                {
                    onMouseMoveNoBut(mouse_delta_p);
                }
        }

        private void doMouseLeave(object sender, EventArgs e)
        {
            onMouseLeave(mouse_last_p);
        }

        private void doMouseDown(object sender, MouseEventArgs e)
        {
            eargs = e;
            fillModifiers();
            mouse_press_p.X = e.X;
            mouse_press_p.Y = e.Y;
            mainform.miniform.doSync = false;
            mouse_down_ticks = DateTime.Now.Ticks;
            GML.tranBegin();
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    mouse_start_p.X = e.X;
                    mouse_start_p.Y = e.Y;
                    Program.Log("Mouse drag start: " + e.X + ", " + e.Y + " -> " + mouse_start_p.ToString());

                    if (boards.mouseDown(mouse_start_p))
                    {
                        GML.repaint();
                        mainform.miniform.doSync = true;
                        return;
                    }

                    if (onMouseDown(mouse_start_p))
                        return;
                    if (onMouseDownTranslated(GML.translateAbsToScene(mouse_start_p)))
                        return;
                }
                else
                    if (e.Button == MouseButtons.Right)
                    {
                        if (onMouseDownTranslated(GML.translateAbsToScene(mouse_start_p)))
                            return;
                        if (onMouseDown(mouse_start_p))
                            return;
                    }
            }
            finally
            {
                GML.tranEnd();
            }
        }

        private void doMouseUp(object sender, MouseEventArgs e)
        {
            eargs = e;
            fillModifiers();
            mouse_release_p.X = e.X;
            mouse_release_p.Y = e.Y;
            mainform.miniform.doSync = false;
            if (e.Button == MouseButtons.Left)
            {
                if (boards.mouseUp(mouse_release_p))
                {
                    GML.repaint();
                    mainform.miniform.doSync = true;
                    return;
                }

                if (onMouseUpTranslated(GML.translateAbsToScene(mouse_release_p)))
                    return;
                if (onMouseUp(mouse_release_p))
                    return;

            }
            else
                if (e.Button == MouseButtons.Right)
                {
                    if (onMouseUpTranslated(GML.translateAbsToScene(mouse_release_p)))
                        return;
                    if (onMouseUp(mouse_release_p))
                        return;
                }
            mainform.miniform.doSync = true;
            mainform.miniform.repaintMap();
        }

        private void doMouseClick(object sender, MouseEventArgs e)
        {
            eargs = e;
            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                mouse_release_p.X = e.X;
                mouse_release_p.Y = e.Y;
                if (System.Math.Abs(mouse_press_p.X - mouse_release_p.X) +
                    System.Math.Abs(mouse_press_p.Y - mouse_release_p.Y) < drag_start_delta)
                {
                    if (boards.mouseClick(mouse_release_p))
                    {
                        GML.repaint();
                        return;
                    }

                    if (onMouseClickTranslated(GML.translateAbsToScene(mouse_start_p)))
                        return;
                    if (onMouseClick(mouse_start_p))
                        return;
                }
            }
        }

        private void doMouseDoubleClick(object sender, MouseEventArgs e)
        {
            eargs = e;
            mouse_release_p.X = e.X;
            mouse_release_p.Y = e.Y;
            Program.Log("Double click detected at position: " + mouse_release_p.ToString());

            if (boards.mouseDoubleClick(mouse_release_p))
            {
                GML.repaint();
                return;
            }

            if (onMouseDoubleClickTranslated(GML.translateAbsToScene(mouse_release_p)))
                return;
            if (onMouseDoubleClick(mouse_release_p))
                return;

        }

        private void doMouseWheel(object sender, MouseEventArgs e)
        {
            eargs = e;

            if (e.Delta != 0)
            {
                if (e.Delta > 0)
                    onMouseWheelUp();
                else
                    onMouseWheelDown();
            }
        }

        /// <summary>
        /// Return the name of the mode
        /// </summary>
        /// <returns></returns>
        public virtual string name()
        {
            return "base mode";
        }

        /// <summary>
        /// Gets the time in millisecond since the given time til now
        /// </summary>
        /// <param name="sinceWhen"></param>
        /// <returns></returns>
        protected long getDeltaMillis(long sinceWhenTicks)
        {
            return (DateTime.Now.Ticks - sinceWhenTicks) / 10000L;
        }

        private void fillModifiers()
        {
            shiftPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            controlPressed = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            altPressed = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;
        }
    }
}
