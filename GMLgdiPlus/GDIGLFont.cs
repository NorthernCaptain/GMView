using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class GDIGLFont: IGLFont
    {
        Font fnt;
        public GDIGLFont(Font gdiFont)
        {
            fnt = gdiFont;
        }

        #region IGLFont Members

        public void draw(string str, int ix, int iy, int iz)
        {
            GDIUControl gdiu = (GML.device as GDIUControl);
            Graphics gr = gdiu.currentGraphics;
            iy++;
            gr.DrawString(str, fnt, gdiu.fontBrush, ix, -iy);
        }

        public void drawscene(string str, int ix, int iy, int iz)
        {
            GDIUControl gdiu = (GML.device as GDIUControl);
            Graphics gr = gdiu.currentGraphics;
            System.Drawing.Point pt = new System.Drawing.Point(ix, iy);
            /*
            pt = GML.device.translateSceneToAbs(pt);
            int hh = pt.Y - iy;
            pt.Y = hh - iy;
             */
            pt.Y++;
            GML.device.pushMatrix();
            GML.device.zeroPosition();
            do
            {
                double angle = GML.device.angle;
                if (angle <= 0.01 && angle >= -0.01)
                    break;
                double xold = (double)pt.X;
                double yold = (double)pt.Y;

                pt.X = (int)(xold * GML.cosa - yold * GML.sina);
                pt.Y = (int)(yold * GML.cosa + xold * GML.sina);

            } while (false);

            gr.DrawString(str, fnt, gdiu.fontBrush, pt.X, -pt.Y);
            GML.device.popMatrix();
        }

        public void drawright(string str, int ix, int iy, int iz)
        {
            GDIUControl gdiu = (GML.device as GDIUControl);
            Graphics gr = gdiu.currentGraphics;
            SizeF sz = gr.MeasureString(str, fnt);
            ix -= (int)sz.Width;
            iy++;
            gr.DrawString(str, fnt, gdiu.fontBrush, ix, -iy);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
