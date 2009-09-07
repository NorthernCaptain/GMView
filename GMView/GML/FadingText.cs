using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    public class FadingText: IFrameUpdate, ISprite
    {
        private string text;
        private float r,g,b,alpha=1.5f;
        private IGLFont font;
        private ulong ticks;
        private int x, y;

        public event EventHandler onDone;

        public FadingText(int ix, int iy, string txt, Color col, IGLFont ifnt)
        {
            text = txt;
            r = (float)col.R / 255.0f;
            g = (float)col.G / 255.0f;
            b = (float)col.B / 255.0f;
            font = ifnt;
            x = ix; y = iy;
            FrameTimer.singleton.add(this);
        }
        
        #region IFrameUpdate Members

        public bool updateFrame(ulong ticks)
        {
            alpha -= 0.03f*(float)ticks/15.0f;
            if (alpha < 0.0f)
            {
                FrameTimer.singleton.del(this);
            }
            this.ticks = ticks;
            return true;
        }

        public bool registered(ulong start_ticks)
        {
            ticks = start_ticks;
            this.show();
            return true;
        }

        public bool unregistered(ulong end_ticks)
        {
            if (onDone != null)
                onDone(this, EventArgs.Empty);
            return true;
        }

        #endregion

        #region ISprite Members

        public void draw(Graphics gr)
        {
            
        }

        public void draw(Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void glDraw(int centerx, int centery)
        {
            GML.device.pushMatrix();
            GML.device.identity();
            GML.device.color4(r, g, b, alpha > 1.0f ? 1.0f : alpha);
            font.draw(text, x - centerx, centery - y, dLevel + 2);
            GML.device.color4(1.0f, 1.0f, 1.0f, 1.0f);
            GML.device.popMatrix();
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
        }

        public void hide()
        {
        }

        #endregion
    }
}
