using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace XnGFL
{
    using ImgList = List<XnGFL.Image>;

    /// <summary>
    /// Task for loading all images from the given directory
    /// </summary>
    class ExifImageLoader: ncUtils.IRunnable
    {
        internal delegate void onPartialCompletionDelegate(ExifImageLoader task, ImgList whatIsDone);
        internal onPartialCompletionDelegate onPartialCompletion = null;

        private string dir;
        private ImgList imageList = new ImgList();
        private int imgWidth;
        private int imgHeight;
        private Control resultControl;

        private const int partialCount = 5;

        /// <summary>
        /// Contructor for the loading task
        /// </summary>
        /// <param name="ifromdir"></param>
        /// <param name="iwidth"></param>
        /// <param name="iheight"></param>
        internal ExifImageLoader(Control resultC, string ifromdir, int iwidth, int iheight)
        {
            resultControl = resultC;
            dir = ifromdir;
            imgWidth = iwidth;
            imgHeight = iheight;
        }

        /// <summary>
        /// Result of the task processing - a list of loaded images.
        /// </summary>
        internal ImgList resultList
        {
            get { return imageList; }
        }

        #region IRunnable Members

        public void run()
        {
            string[] files = Directory.GetFiles(dir, "*.*");
            int count = 0;
            foreach (string fname in files)
            {
                XnGFL.Image img = loadImage(fname);
                if (img == null)
                    continue;
                imageList.Add(img);
                count++;

                //Do partial notification if we need it
                if (count >= partialCount && onPartialCompletion != null && resultControl != null)
                {
                    resultControl.Invoke(onPartialCompletion, new Object[] { this, imageList });
                    count = 0;
                    imageList = new ImgList();
                }
            }
        }
        
        #endregion

        /// <summary>
        /// Loads one image into memory
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        private XnGFL.Image loadImage(string fname)
        {
            XnGFL.Image img = new Image(fname);
            img.thumbnail = true;
            if (img.Load(imgWidth, imgHeight) != Common.GFL_ERROR.NOERROR)
                return null;
            img.ConvertToImage();
            img.loadExif();
            string sname = Path.GetFileName(fname);
            img.Text = sname;
            return img;
        }
    }
}
