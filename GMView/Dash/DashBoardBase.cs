using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public abstract class DashBoardBase: IDashBoard
    {

        #region DashRect and callers
        protected class DashRect
        {
            int x, y, width, height;
            internal delegate bool OnMatchDelegate(Point xy);
            OnMatchDelegate onMatch;

            internal DashRect(int ix, int iy, int iw, int ih, OnMatchDelegate imatch)
            {
                x = ix;
                y = iy;
                width = iw;
                height = ih;
                onMatch = imatch;
            }

            internal bool matchAndCall(Point xy)
            {
                if (xy.X >= x && xy.X < x + width && xy.Y >= y && xy.Y < y + height)
                    return onMatch(xy);
                return false;
            }

            internal bool matchAndCall(Point xy, Point xy2)
            {
                if (xy.X >= x && xy.X < x + width && xy.Y >= y && xy.Y < y + height)
                    return onMatch(xy2);
                return false;
            }
        }

        protected class DashRectCallers
        {
            List<DashRect> callers = new List<DashRect>();

            internal void add(DashRect rect)
            {
                callers.Add(rect);
            }

            internal void del(DashRect rect)
            {
                callers.Remove(rect);
            }

            internal bool matchAndCall(Point xy)
            {
                foreach (DashRect rect in callers)
                    if (rect.matchAndCall(xy))
                        return true;
                return false;
            }

            internal bool matchAndCall(Point xy, Point xy2)
            {
                foreach (DashRect rect in callers)
                    if (rect.matchAndCall(xy, xy2))
                        return true;
                return false;
            }
        }
        protected DashRectCallers mouseUpCallers = new DashRectCallers();
        protected DashRectCallers mouseDownCallers = new DashRectCallers();
        protected DashRectCallers mouseMoveCallers = new DashRectCallers();
        protected DashRectCallers mouseDoubleClickCallers = new DashRectCallers();
        #endregion

        protected const int delta_border_x = 23;
        protected int dwidth;
        protected int dheight;
        protected int rwidth; //real window width
        protected DashBoardContainer.Justify justif = 
                DashBoardContainer.Justify.Left | DashBoardContainer.Justify.Top;
        protected ImageDot bgImg;
        protected object bgTex;
        protected ImageDot bgImgWrapped;
        protected object bgTexWrapped;
        protected Color textcolor;
        protected DashMode dmode = DashMode.Normal;

        public DashBoardBase()
        {
            textcolor = Color.FromArgb(198, 253, 189);
        }

        #region IDashBoard Members

        public DashBoardContainer.Justify justify
        {
            get { return justif; }
        }

        public int width
        {
            get { return dwidth; }
            set { dwidth = value; rwidth = dwidth - delta_border_x; }
        }

        public int height
        {
            get { return dmode == DashMode.Normal ? dheight : dmode== DashMode.Wrapped ? bgImgWrapped.delta_y : 0; }
        }

        public virtual void draw(int ix, int iy)
        {
            if (dmode == DashMode.Wrapped)
            {
                GML.device.color(Color.White);
                GML.device.texDrawBegin();
                GML.device.texFilter(bgTexWrapped, TexFilter.Pixel);
                GML.device.texDraw(bgTexWrapped, ix, iy, 0, bgImgWrapped.img.Width, bgImgWrapped.img.Height);
                GML.device.texDrawEnd();
            }
                
        }

        public DashMode mode
        {
            get
            {
                return dmode;
            }
            set
            {
                dmode = value;
            }
        }

        public virtual bool mouseUp(Point xy)
        {
            return mouseUpCallers.matchAndCall(xy);
        }

        public virtual bool mouseDown(Point xy)
        {
            mouseDownCallers.matchAndCall(xy);
            return true;
        }

        public virtual bool mouseClick(Point xy)
        {
            return true;
        }

        public virtual bool mouseDoubleClick(Point xy)
        {
            mouseDoubleClickCallers.matchAndCall(xy);
            return true;
        }

        public virtual bool mouseMove(Point xy)
        {
            mouseMoveCallers.matchAndCall(xy);
            return true;
        }

        protected virtual bool clickWrapped(Point xy)
        {
            dmode = dmode == DashMode.Normal ? DashMode.Wrapped : DashMode.Normal;
            return true;
        }

        protected virtual void addCallers()
        {
            mouseUpCallers.add(new DashRect(width - 24, 0, 24, 20, new DashRect.OnMatchDelegate(clickWrapped)));
        }

        #endregion
    }
}
