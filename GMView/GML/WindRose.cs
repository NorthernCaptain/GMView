using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

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

        private ImageDot greendotimd;
        private object greendotex;

        private bool shown = false;

        private GPS.FollowUpConnector followCon = null;

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
            greendotimd = TextureFactory.singleton.getImg(TextureFactory.TexAlias.DotGreen);
            greendotex = TextureFactory.singleton.getTex(greendotimd);
        }

        /// <summary>
        /// Setter for follow up connector class. We draw it's direction dots here
        /// </summary>
        public GPS.FollowUpConnector follower
        {
            set
            {
            	followCon = value;
            }
            get
            {
                return followCon;
            }
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

            if (followCon != null && followCon.isShown)
            {
                double fang = followCon.finishAng;
                GML.device.pushMatrix();
                GML.device.color(Color.White);
                GML.device.zeroPosition();
                GML.device.rotateZ(fang);
                GML.device.texDrawBegin();
                GML.device.texFilter(reddotex, TexFilter.Smooth);
                GML.device.texDraw(reddotex, -reddotimd.delta_x, reddotimd.delta_y + 95, 0,
                                    reddotimd.img.Width, reddotimd.img.Height);
                GML.device.rotateZ(-fang + followCon.currentAng);
                GML.device.texFilter(greendotex, TexFilter.Smooth);
                GML.device.texDraw(greendotex, -greendotimd.delta_x, greendotimd.delta_y + 95, 0,
                                    greendotimd.img.Width, greendotimd.img.Height);
                GML.device.texDrawEnd();
                GML.device.popMatrix();
            }
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
