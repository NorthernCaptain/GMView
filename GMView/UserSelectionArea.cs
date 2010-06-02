using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class UserSelectionArea: ISprite
    {
        protected Point start_p;
        protected Point end_p;
        protected Pen   drawPen;
        protected bool shown = false;
        protected MapObject mapo;
        protected double lon1, lat1;
        protected double lon2, lat2;

        public delegate void OnAreaSelectionDelegate(double lon1, double lat1, double lon2, double lat2);
        public event OnAreaSelectionDelegate onAreaSelection;

        public UserSelectionArea(MapObject imapo)
        {
            mapo = imapo;
            drawPen = new Pen(Color.DarkRed, 2);
            drawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }

        public void setColor(Color newCol)
        {
            drawPen = new Pen(newCol, 2);
            drawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }

        public void setStartXY(Point istart_p)
        {
            start_p = istart_p;
            end_p = istart_p;
        }

        public void setDeltaXY(Point idelta_p)
        {
            end_p.X += idelta_p.X;
            end_p.Y += idelta_p.Y;
            GML.repaint();
        }

        public void setEndXY(Point iend_p)
        {
            Point p1=new Point(), p2= new Point();

            if (end_p.X < start_p.X)
            {
                p1.X = end_p.X;
                p2.X = start_p.X;
            }
            else
            {
                p1.X = start_p.X;
                p2.X = end_p.X;
            }

            if (end_p.Y < start_p.Y)
            {
                p1.Y = end_p.Y;
                p2.Y = start_p.Y;
            }
            else
            {
                p1.Y = start_p.Y;
                p2.Y = end_p.Y;
            }

            mapo.getLonLatByVisibleXY(p1, out lon1, out lat1);
            mapo.getLonLatByVisibleXY(p2, out lon2, out lat2);
            if (onAreaSelection != null)
                onAreaSelection(lon1, lat1, lon2, lat2);
        }

        protected Rectangle getProperRect()
        {
            Rectangle selection=new Rectangle();
            selection.X = end_p.X < start_p.X ? end_p.X : start_p.X;
            selection.Y = end_p.Y < start_p.Y ? end_p.Y : start_p.Y;
            selection.Width = System.Math.Abs(start_p.X - end_p.X);
            selection.Height = System.Math.Abs(start_p.Y - end_p.Y);
            return selection;
        }

        public void reset()
        {
            start_p.X = start_p.Y = 0;
            end_p = start_p;
            lon1 = lat1 = lon2 = lat2 = 0.0;
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            if (!shown)
                return;

            int width = end_p.X - start_p.X;
            int height = end_p.Y - start_p.Y;
            int x = start_p.X, y = start_p.Y;

            if (width == 0 || height == 0)
                return;

            if (width < 0)
            {
                width = -width;
                x = end_p.X;
            }

            if (height < 0)
            {
                height = -height;
                y = end_p.Y;
            }

            gr.DrawRectangle(drawPen, x, y, width, height); 
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public int dLevel
        {
            get
            {
                return 2;
            }
            set
            {
                //nothing to do
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

        public virtual void glDraw(int centerx, int centery)
        {
        }

        #endregion
    }
}
