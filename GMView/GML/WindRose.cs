using System;
using System.Collections.Generic;
using System.Text;

namespace GMView
{
    /// <summary>
    /// Class that draw wind rose in the center of the screen
    /// </summary>
    class WindRose: ISprite
    {
        private ImageDot imd;
        private object tex;

        private ImageDot reddotimd;
        private object reddotex;

        private bool shown = false;

        public WindRose()
        {
            TextureFactory.singleton.onInited += initGLData;
        }

        public void initGLData()
        {
            imd = TextureFactory.singleton.getImg(TextureFactory.TexAlias.Rose);
            tex = TextureFactory.singleton.getTex(imd);
            reddotimd = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DotRed);
            reddotex = TextureFactory.singleton.getTex(reddotimd);
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
        }

        public void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;

            GML.device.texDrawBegin();
            GML.device.texFilter(tex, TexFilter.Pixel);
            GML.device.texDraw(tex, -imd.delta_x, imd.delta_y, 0, imd.img.Width, imd.img.Height);
            GML.device.texDrawEnd();

            GML.device.texDrawBegin();
            GML.device.texFilter(reddotex, TexFilter.Pixel);
            GML.device.texDraw(reddotex, -reddotimd.delta_x, reddotimd.delta_y - 45, 0, 
                                reddotimd.img.Width, reddotimd.img.Height);
            GML.device.texDrawEnd();
        }

        public int dLevel
        {
            get
            {
                return 1;
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

        #endregion
    }
}
