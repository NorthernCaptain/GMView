using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class ImageDot
    {
        public int delta_x;
        public int delta_y;
        public int real_len;
        public int real_hei;

        public Bitmap img;

        public ImageDot(Bitmap i, int ix, int iy)
        {
            img = i;
            delta_x = ix;
            delta_y = iy;
            real_len = img.Width;
            real_hei = img.Height;
        }

        public void draw(Graphics gr, Point xy)
        {
            gr.DrawImageUnscaled(img,xy.X - delta_x, xy.Y - delta_y);
        }
    }
}
