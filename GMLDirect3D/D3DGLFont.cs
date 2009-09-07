using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace GMView
{
    public class D3DGLFont: IGLFont
    {
        Microsoft.DirectX.Direct3D.Device device;
        Microsoft.DirectX.Direct3D.Font fnt;
        Microsoft.DirectX.Direct3D.Sprite sprite;
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle();

        public D3DGLFont(System.Drawing.Font gdiFont, Device dev, Sprite spr)
        {
            fnt = new Font(dev, gdiFont);
            sprite = spr;
            device = dev;
        }

        #region IGLFont Members

        public void draw(string str, int ix, int iy, int iz)
        {
            System.Drawing.Point pt = new System.Drawing.Point(ix, iy);
            pt = GML.device.translateSceneToAbs(pt);
            int hh = pt.Y - iy;
            pt.Y = hh - iy;
            sprite.Begin(SpriteFlags.SortTexture);
            fnt.DrawText(sprite, str, pt, GML.device.curColorInt);
            sprite.End();
        }

        public void drawscene(string str, int ix, int iy, int iz)
        {
            System.Drawing.Point pt = GML.translateToScene(new System.Drawing.Point(ix, iy));
            draw(str, pt.X + GML.device.deltaCenter.X, pt.Y + GML.device.deltaCenter.Y, iz);
            return;
        }

        public void drawright(string str, int ix, int iy, int iz)
        {
            System.Drawing.Point pt = new System.Drawing.Point(ix, iy);
            pt = GML.device.translateSceneToAbs(pt);
            int hh = pt.Y - iy;
            pt.Y = hh - iy;
            rect.X = -1000;
            rect.Y = pt.Y;
            rect.Width = pt.X + 1000;
            rect.Height = 100;
            sprite.Begin(SpriteFlags.SortTexture);
            fnt.DrawText(sprite, str, rect, DrawTextFormat.NoClip | DrawTextFormat.Right, GML.device.curColorInt);
            sprite.End();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            fnt.Dispose();
        }

        #endregion



    }
}
