using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GMView
{
    public class DashBoardContainer: ISprite
    {
        public enum Justify { Right=1, Left=2, Top=4, Bottom=8 };
        int delta_x, delta_y;
        int width;
        Justify dir_pos;
        Size vis_size = new Size();
        bool shown = false;
        List<IDashBoard> boards = new List<IDashBoard>();
        int x1, x2, y1, y2;

        public int total_dash_height = 0;
        public const int minimized_height = 70;

        /// <summary>
        /// Delegate provides calling interface for state changing of dashboard
        /// </summary>
        public delegate void StateChangedDelegate();
        /// <summary>
        /// Event occures when dashboard state changes, e.g. minimized or normal
        /// </summary>
        public event StateChangedDelegate onStateChanged;

        /// <summary>
        /// Is our dashboard fully minimized?
        /// </summary>
        /// <returns></returns>
        public bool isMinimized() { return total_dash_height < minimized_height; }

        public DashBoardContainer(Justify dir, int idelta_x, int idelta_y, int iwidth)
        {
            dir_pos = dir;
            delta_x = idelta_x;
            delta_y = idelta_y;
            width = iwidth;
        }

        /// <summary>
        /// Sets the new position for the dashboard container. Asks for redraw.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="idelta_x"></param>
        /// <param name="idelta_y"></param>
        public void setPosition(Justify dir, int idelta_x, int idelta_y)
        {
            dir_pos = dir;
            delta_x = idelta_x;
            delta_y = idelta_y;
        }

        public void setVisibleSize(Size sz)
        {
            vis_size = sz;
            if ((dir_pos & Justify.Right) == Justify.Right)
            {
                x2 = vis_size.Width - delta_x;
                x1 = x2 - width;
            }
            else
            {
                x1 = delta_x;
                x2 = x1 + width;
            }

            if ((dir_pos & Justify.Bottom) == Justify.Bottom)
            {
                y2 = vis_size.Height - delta_y;
                y1 = 0;
            }
            else
            {
                y1 = delta_y;
                y2 = vis_size.Height - 1;
            }

            if (onStateChanged != null)
                onStateChanged();
        }

        public void addDashBoard(IDashBoard dash)
        {
            boards.Add(dash);
            updateTotalHeight();
        }

        public void delDashBoard(IDashBoard dash)
        {
            boards.Remove(dash);
            updateTotalHeight();
        }

        /// <summary>
        /// Calculates total dashboard height according to the widget heights
        /// </summary>
        private void updateTotalHeight()
        {
            bool lastState = isMinimized();

            total_dash_height = 0;
            foreach (IDashBoard dash in boards)
            {
                total_dash_height += dash.height +1;
            }

            if ( lastState != isMinimized() && onStateChanged != null )
            {
                onStateChanged();
            }
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            throw new NotImplementedException();
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void glDraw(int centerx, int centery)
        {
            total_dash_height = 0;
            if (!shown)
                return;

            int up_y = y1;
            int down_y = y2;

            GML.device.pushMatrix();
            GML.device.identity();

            foreach (IDashBoard dash in boards)
            {
                Justify j = dash.justify;
                int x = ((j & Justify.Right) == Justify.Right) ? x2 - dash.width : x1;
                GML.device.color(Color.White);
                if ((j & Justify.Bottom) == Justify.Bottom)
                {
                    down_y -= (dash.height + 1);
                    dash.draw(x - centerx, centery - down_y);
                }
                else
                {
                    dash.draw(x - centerx, centery - up_y);
                    up_y += (dash.height + 1);
                }
                total_dash_height += dash.height + 1;
            }

            GML.device.popMatrix();
            GML.device.color(Color.White);
        }

        public int dLevel
        {
            get
            {
                return 3;
            }
            set
            {
            
            }
        }

        public void show()
        {
            shown = true;
        }

        public void hide()
        {
            shown = false;
        }

        public bool mouseDown(Point xy)
        {
            if (!shown)
                return false;

            int up_y = y1;
            int down_y = y2;

            foreach (IDashBoard dash in boards)
            {
                Justify j = dash.justify;
                int x = ((j & Justify.Right) == Justify.Right) ? x2 - dash.width : x1;
                int y = 0;
                if ((j & Justify.Bottom) == Justify.Bottom)
                {
                    down_y -= (dash.height + 1);
                    y = down_y;
                }
                else
                {
                    y = up_y;
                    up_y += (dash.height + 1);
                }

                if (xy.X >= x && xy.X < x + dash.width && xy.Y >= y && xy.Y < y + dash.height)
                {
                    //convert to dash relative coordinates
                    xy.X -= x;
                    xy.Y -= y;
                    if (dash.mouseDown(xy))
                        return true;
                }
            }
            return false;
        }

        public bool mouseClick(Point xy)
        {
            if (!shown)
                return false;

            int up_y = y1;
            int down_y = y2;

            foreach (IDashBoard dash in boards)
            {
                Justify j = dash.justify;
                int x = ((j & Justify.Right) == Justify.Right) ? x2 - dash.width : x1;
                int y = 0;
                if ((j & Justify.Bottom) == Justify.Bottom)
                {
                    down_y -= (dash.height + 1);
                    y = down_y;
                }
                else
                {
                    y = up_y;
                    up_y += (dash.height + 1);
                }

                if (xy.X >= x && xy.X < x + dash.width && xy.Y >= y && xy.Y < y + dash.height)
                {
                    //convert to dash relative coordinates
                    xy.X -= x;
                    xy.Y -= y;
                    if (dash.mouseClick(xy))
                    {
                        updateTotalHeight();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool mouseDoubleClick(Point xy)
        {
            if (!shown)
                return false;

            int up_y = y1;
            int down_y = y2;

            foreach (IDashBoard dash in boards)
            {
                Justify j = dash.justify;
                int x = ((j & Justify.Right) == Justify.Right) ? x2 - dash.width : x1;
                int y = 0;
                if ((j & Justify.Bottom) == Justify.Bottom)
                {
                    down_y -= (dash.height + 1);
                    y = down_y;
                }
                else
                {
                    y = up_y;
                    up_y += (dash.height + 1);
                }

                if (xy.X >= x && xy.X < x + dash.width && xy.Y >= y && xy.Y < y + dash.height)
                {
                    //convert to dash relative coordinates
                    xy.X -= x;
                    xy.Y -= y;
                    if (dash.mouseDoubleClick(xy))
                        return true;
                }
            }
            return false;
        }

        public bool mouseUp(Point xy)
        {
            if (!shown)
                return false;

            int up_y = y1;
            int down_y = y2;

            foreach (IDashBoard dash in boards)
            {
                Justify j = dash.justify;
                int x = ((j & Justify.Right) == Justify.Right) ? x2 - dash.width : x1;
                int y = 0;
                if ((j & Justify.Bottom) == Justify.Bottom)
                {
                    down_y -= (dash.height + 1);
                    y = down_y;
                }
                else
                {
                    y = up_y;
                    up_y += (dash.height + 1);
                }

                if (xy.X >= x && xy.X < x + dash.width && xy.Y >= y && xy.Y < y + dash.height)
                {
                    //convert to dash relative coordinates
                    xy.X -= x;
                    xy.Y -= y;
                    if (dash.mouseUp(xy))
                    {
                        updateTotalHeight();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool mouseMove(Point xy)
        {
            if (!shown)
                return false;

            int up_y = y1;
            int down_y = y2;

            foreach (IDashBoard dash in boards)
            {
                Justify j = dash.justify;
                int x = ((j & Justify.Right) == Justify.Right) ? x2 - dash.width : x1;
                int y = 0;
                if ((j & Justify.Bottom) == Justify.Bottom)
                {
                    down_y -= (dash.height + 1);
                    y = down_y;
                }
                else
                {
                    y = up_y;
                    up_y += (dash.height + 1);
                }

                if (xy.X >= x && xy.X < x + dash.width && xy.Y >= y && xy.Y < y + dash.height)
                {
                    //convert to dash relative coordinates
                    xy.X -= x;
                    xy.Y -= y;
                    if (dash.mouseMove(xy))
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
}
