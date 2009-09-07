using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


namespace GMView
{
    class GLImgTile: ImgTile
    {
        protected object texture;

        public GLImgTile(int ix_idx, int iy_idx, int izoom_lvl, MapTileType itype)
            : base(ix_idx, iy_idx, izoom_lvl, itype)
        { }

        public override void bitmap_loaded()
        {
//            img.RotateFlip(RotateFlipType.RotateNoneFlipY);
            texture = GML.device.texFromBitmapNoCheck(img);
        }

        public override void dispose()
        {
            base.dispose();
            GML.device.texDispose(texture);
        }

        public override void draw(Graphics gr, int ix, int iy)
        {
            if (gr != null)
            {
                base.draw(gr, ix, iy);
                return;
            }

            if (texture == null)
            {
                Program.Err("GLTile: Attempt to draw empty texture for square: " + x_idx + ", " + y_idx + "/" + zoom_lvl);
                throw new ArgumentException("GLTile: Attempt to draw empty texture");
            }

            if (Math.Abs(Program.opt.angle % 90.0) > 0.001)
            {
                GML.device.texFilter(texture, TexFilter.Smooth);
            }
            else
            {
                GML.device.texFilter(texture, TexFilter.Pixel);
            }
            GML.device.texDraw(texture, ix, iy, 0, Program.opt.image_len, Program.opt.image_hei);
        }
    }
}
