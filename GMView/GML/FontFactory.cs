using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Class that manages our fonts
    /// </summary>
    public class FontFactory: IDisposable
    {
        public enum FontAlias { Big22B, Mid14B, Small8R, 
                    Sans18R, Sans12R, Sans8R, Sans10B, Big48I, Big20I, Big24R };

        Dictionary<Font, IGLFont> fonts = new Dictionary<Font, IGLFont>();
        Dictionary<FontAlias, Font> gdiFonts = new Dictionary<FontAlias, Font>();
        private static FontFactory instance = new FontFactory();
        Bitmap bTmp;
        Graphics gfx;

        FontFactory()
        {
            bTmp = new Bitmap(2, 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(bTmp);
            gdiFonts.Add(FontAlias.Big24R, initGDIMesuredFont("Arial", 20, FontStyle.Italic | FontStyle.Bold));
            gdiFonts.Add(FontAlias.Big22B, initGDIMesuredFont("Arial", 20, FontStyle.Bold));
            gdiFonts.Add(FontAlias.Big20I, initGDIMesuredFont("Arial", 21, FontStyle.Italic));
            gdiFonts.Add(FontAlias.Mid14B, initGDIMesuredFont("Arial", 12, FontStyle.Bold));
            gdiFonts.Add(FontAlias.Small8R, initGDIMesuredFont("Arial", 8, FontStyle.Regular));
            gdiFonts.Add(FontAlias.Sans18R, initGDIMesuredFont(FontFamily.GenericSansSerif.Name, 18, FontStyle.Regular));
            gdiFonts.Add(FontAlias.Sans12R, initGDIMesuredFont(FontFamily.GenericSansSerif.Name, 10, FontStyle.Regular));
            gdiFonts.Add(FontAlias.Sans8R, initGDIMesuredFont(FontFamily.GenericSansSerif.Name, 8, FontStyle.Regular));
            gdiFonts.Add(FontAlias.Sans10B, initGDIMesuredFont(FontFamily.GenericSansSerif.Name, 10, FontStyle.Bold));
            gdiFonts.Add(FontAlias.Big48I, initGDIMesuredFont("Arial", 48, FontStyle.Italic));
        }

        protected Font initGDIMesuredFont(string name, float size, FontStyle style)
        {
            Font fnt;
            float newsize = size;
            size += 4;
            while (true)
            {
                fnt = initGDIFont(name, newsize, style);
                gfx.PageUnit = GraphicsUnit.Pixel;
                SizeF sz = gfx.MeasureString("WygY", fnt, 16384, System.Drawing.StringFormat.GenericTypographic);
                if (Math.Floor(sz.Height) > size && newsize > 5.0f)
                {
                    fnt.Dispose();
                    fnt = null;
                    newsize -= 1.0f;
                }
                else
                    break;
            }
            return fnt;
        }

        protected Font initGDIFont(string name, float size, FontStyle style)
        {
            Font gdifnt=null;
            try
            {
                gdifnt = new Font(name, size, style);
            }
            catch
            {
                try
                {
                    gdifnt = new Font(FontFamily.GenericSansSerif, size, style);
                }
                catch
                {
                    gdifnt = new Font(FontFamily.GenericSansSerif, size);
                }
            }
            return gdifnt;
        }

        public void initGLData()
        {
            foreach (KeyValuePair<FontAlias, Font> pair in gdiFonts)
                getGLFont(pair.Key);
        }

        public IGLFont getGLFont(FontAlias fal)
        {
            Font gdifnt = gdiFonts[fal];
            IGLFont glfnt;
            if (fonts.TryGetValue(gdifnt, out glfnt))
            {
                return glfnt;
            }
            glfnt = GML.device.fontCreate(gdifnt);
            fonts.Add(gdifnt, glfnt);
            return glfnt;            
        }

        public IGLFont getGLFont(Font fnt)
        {
            IGLFont glfnt;
            if (fonts.TryGetValue(fnt, out glfnt))
            {
                fnt.Dispose();
                return glfnt;
            }
            glfnt = GML.device.fontCreate(fnt);
            fonts.Add(fnt, glfnt);
            return glfnt;
        }

        public Font getGDIFont(FontAlias fal)
        {
            return gdiFonts[fal];
        }

        public static FontFactory singleton
        {
            get { return instance; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (KeyValuePair<Font, IGLFont> pair in fonts)
                pair.Value.Dispose();
        }

        #endregion
    }
}
