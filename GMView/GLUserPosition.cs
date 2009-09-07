using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class GLUserPosition: UserPosition
    {
        object texture;

        public GLUserPosition(MapObject imapo, ImageDot imd)
            : base(imapo, imd)
        {
            TextureFactory.singleton.onInited += initGLData;
        }

        public GLUserPosition(MapObject imapo)
            : base(imapo)
        {
            TextureFactory.singleton.onInited += initGLData;            
        }


        public override void initGLData()
        {
            texture = TextureFactory.singleton.getTex(img);
        }

        public override void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;

            Point xy;
            mapo.getVisibleXYByLonLat(lon, lat, out xy);
            if (xy.X < 0 || xy.Y < 0)
                return;

            GML.device.pushMatrix();
            GML.device.translate(xy.X - centerx, centery - xy.Y, 0);
            GML.device.rotateZ(-Program.opt.angle);
            GML.device.texDrawBegin();
            GML.device.texFilter(texture, TexFilter.Pixel);
            GML.device.texDraw(texture, -img.delta_x, img.delta_y, -(drawLevel+2), img.img.Width, img.img.Height);
            GML.device.texDrawEnd();
            GML.device.popMatrix();
        }
    }
}
