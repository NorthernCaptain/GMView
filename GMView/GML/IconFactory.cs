using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace GMView
{
    /// <summary>
    /// Class provides management for our internal icons grouped in iconsets
    /// </summary>
    public class IconFactory
    {
        /// <summary>
        /// Current path to the iconset directory
        /// </summary>
        private string currentPath;

        /// <summary>
        /// Single instance of the factory
        /// </summary>
        private static volatile IconFactory instance;

        /// <summary>
        /// Cached icons, that were loaded from disk into bitmap
        /// </summary>
        private Dictionary<string, ImageDot> dotMap = new Dictionary<string, ImageDot>();

        /// <summary>
        /// return instance of the factory to use
        /// </summary>
        public static IconFactory singleton
        {
            get
            {
                if (instance == null)
                    instance = new IconFactory();
                return instance;
            }
        }


        /// <summary>
        /// Constructor, sets path to the iconset
        /// </summary>
        private IconFactory()
        {
            currentPath = Program.opt.iconSetPath;
        }

        /// <summary>
        /// Loads and return ImageDot with icon inside it as Bitmap image.
        /// Caches icons in memory, loads them only once.
        /// </summary>
        /// <param name="nfo"></param>
        /// <returns></returns>
        public ImageDot getIcon(IIconInfo nfo)
        {
            ImageDot img;
            if (dotMap.TryGetValue(nfo.iconName, out img))
                return img;

            string path = Path.Combine(currentPath, nfo.iconName);
            if (!File.Exists(path))
            {
                ImageDot dot = new ImageDot(global::GMView.Properties.Resources.unknown, 15, 34);
                dotMap.Add(nfo.iconName, dot);
                return dot;
            }

            try
            {
                Bitmap bmp = new Bitmap(path);
                ImageDot dot = new ImageDot(bmp, nfo.iconDeltaX, nfo.iconDeltaY);
                dotMap.Add(nfo.iconName, dot);
                return dot;
            }
            catch (System.Exception ex)
            {
                Program.Log("GetIcon error: " + ex.ToString());
                ImageDot dot = new ImageDot(global::GMView.Properties.Resources.unknown, 15, 34);
                dotMap.Add(nfo.iconName, dot);
                return dot;
            }
            return null;
        }
    }
}
