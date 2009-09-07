using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    class GLUserSelectionArea: UserSelectionArea
    {
        public GLUserSelectionArea(MapObject mapo)
            : base(mapo)
        {

        }

        public override void glDraw(int centerx, int centery)
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

            x -= centerx; y = centery - y;
            GML.device.texDrawBegin();

            GML.device.lineWidth(2);
            GML.device.lineStipple((short)0x3333);
            GML.device.quadDraw(x, y, width, height, dLevel + 2, Color.FromArgb(55, drawPen.Color));
            GML.device.rectDraw(x, y, width, height, dLevel+2, Color.FromArgb(155, drawPen.Color));
            GML.device.texDrawEnd();
        }
    }
}
