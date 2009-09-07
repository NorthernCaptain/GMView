using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace GMView
{
    /// <summary>
    /// Class represents Texture TrueType fonts drawing in OpenGL
    /// </summary>
    public class GLTexTTFont: IGLFont
    {
        protected Font fnt;
        protected Graphics gfx;
        protected Bitmap bmp; //we need this for drawing glyphs

        public class Glyph: IDisposable
        {
            object texture;
            int tex_width;
            int tex_height;
            internal float width;
            float height;
            char character;

            public Glyph(char c, Font fnt, Graphics gfx, Bitmap img)
            {
                SizeF  sz;
                string char_str = c.ToString();

                character = c;
                tex_width = img.Width;
                tex_height = img.Height;

                lock (GML.lock2D)
                {
                    gfx.Clear(Color.Transparent);
                    gfx.DrawString(char_str, fnt, Brushes.White, 0.0f, 0.0f);
                    sz = gfx.MeasureString(char_str, fnt, 16384, System.Drawing.StringFormat.GenericTypographic);

                    height = sz.Height;
                    width = sz.Width;
                    if (width < 0.01)
                        width = fnt.SizeInPoints * 0.5f;
                    gfx.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
                }
                texture = GML.device.texFromBitmapNoCheck(img); //GL_LINEAR
            }


            public virtual void draw(ref int fromx, int fromy, int fromz)
            {
                GML.device.texFilter(texture, TexFilter.Linear);
                GML.device.texDraw(texture, fromx, fromy, fromz, tex_width, tex_height);
                fromx += (int)(width + 0.5f);
            }

            #region IDisposable Members

            public void Dispose()
            {
                GML.device.texDispose(texture);
            }

            #endregion
        }

        protected Dictionary<char, Glyph> loaded_glyphs = new Dictionary<char, Glyph>();

        public GLTexTTFont(Font ifnt) : this(ifnt, "0123456789+-., /")
        {
        }

        public GLTexTTFont(Font ifnt, string preload)
        {
            fnt = ifnt;
            if (fnt == null)
                throw new ArgumentNullException("TexTTFont", "Font parameter to contructor cannot be null.");

            int texheight = GML.device.getTextureAlignedLen(fnt.Height);
            bmp = new Bitmap(texheight, texheight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(bmp);
            if (fnt.Size <= 18.0f)
            {
                gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                gfx.TextContrast = 1;
            }
            else
            {
                gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
                gfx.TextContrast = 0;
            }

            if (preload != null && preload.Length > 0)
                prepareGlyphs(preload);
        }

        public virtual void prepareGlyphs(string str)
        {
            if (str.Length == 0)
                return;
            foreach (char c in str)
            {
                if (loaded_glyphs.ContainsKey(c))
                    continue;
                Glyph glyph = new Glyph(c, fnt, gfx, bmp);
                loaded_glyphs.Add(c, glyph);
            }
        }

        public int textWidth(string str)
        {
            int len = 0;
            if (str == null || str.Length == 0)
                return 0;
            foreach (char c in str)
            {
                Glyph glyph;
                if (!loaded_glyphs.TryGetValue(c, out glyph))
                {
                    glyph = new Glyph(c, fnt, gfx, bmp);
                    loaded_glyphs.Add(c, glyph);
                }
                len += (int)(glyph.width + 0.5f);
            }
            return (int)len;
        }

        #region IGLFont Members

        public virtual void draw(string str, int ix, int iy, int iz)
        {
            if (str == null)
                return;
            Glyph glyph;
            ix -= 2;
            iy++;
            foreach (char c in str)
            {
                if (!loaded_glyphs.TryGetValue(c, out glyph))
                {
                    glyph = new Glyph(c, fnt, gfx, bmp);
                    loaded_glyphs.Add(c, glyph);
                }
                glyph.draw(ref ix, iy, iz);
            }
        }

        public void drawscene(string str, int ix, int iy, int iz)
        {
            if (str == null)
                return;
            Glyph glyph;
            System.Drawing.Point pt = new System.Drawing.Point(ix, iy);
            /*
            pt = GML.device.translateSceneToAbs(pt);
            int hh = pt.Y - iy;
            pt.Y = hh - iy;
             */
            GML.device.pushMatrix();
            GML.device.zeroPosition();
            do
            {
                double angle = GML.device.angle;
                if (angle <= 0.01 && angle >= -0.01)
                    break;
                double xold = (double)pt.X;
                double yold = (double)pt.Y;

                pt.X = (int)(xold * GML.cosa + yold * GML.sina);
                pt.Y = (int)(yold * GML.cosa - xold * GML.sina);

            } while (false);


            ix = pt.X - 2;
            pt.Y++;
            foreach (char c in str)
            {
                if (!loaded_glyphs.TryGetValue(c, out glyph))
                {
                    glyph = new Glyph(c, fnt, gfx, bmp);
                    loaded_glyphs.Add(c, glyph);
                }
                glyph.draw(ref ix, pt.Y, iz);
            }
        }


        public void drawright(string str, int ix, int iy, int iz)
        {
            Glyph glyph;
            if (str == null)
                return;

            ix -= textWidth(str)+2;
            iy++;
            foreach (char c in str)
            {
                if (!loaded_glyphs.TryGetValue(c, out glyph))
                {
                    glyph = new Glyph(c, fnt, gfx, bmp);
                    loaded_glyphs.Add(c, glyph);
                }
                glyph.draw(ref ix, iy, iz);
            }
        }

        public void Dispose()
        {
            foreach (KeyValuePair<char, Glyph> pair in loaded_glyphs)
                pair.Value.Dispose();
        }

        #endregion
    }
}
