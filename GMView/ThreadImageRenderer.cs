using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace GMView
{
    public abstract class ThreadImageRenderer
    {
        protected Queue<Bitmap> availableImg = new Queue<Bitmap>();
        protected Bitmap curImg = null;
        protected Object curTex = null;
        protected RenderInfoBase curNfo;
        protected Control resultControl;
        protected Bitmap origImg;

        public event MethodInvoker onVisualChanged;

        protected Thread visualizer = new Thread(new ParameterizedThreadStart(processVisualStub));

        public class RenderInfoBase
        {
            internal int vis_nx;
            internal int vis_ny;
            internal int size_nw;
            internal int size_nh;
            internal Bitmap img;

            internal RenderInfoBase()
            {
            }

            internal RenderInfoBase(int ix, int iy, int iw, int ih)
            {
                vis_nx = ix;
                vis_ny = iy;
                size_nw = iw;
                size_nh = ih;
                img = null;
            }

        }

        internal ncUtils.SyncQueue<RenderInfoBase> visTasks = new ncUtils.SyncQueue<RenderInfoBase>();

        protected Bitmap getAvailableImg()
        {
            lock (availableImg)
            {
                if (availableImg.Count > 0)
                    return availableImg.Dequeue();
            }
            
            Bitmap img = new Bitmap(origImg.Width, origImg.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            return img;
        }

        internal void switchImage(RenderInfoBase nfo)
        {
            if (curImg != null)
            {
                lock (availableImg)
                {
                    availableImg.Enqueue(curImg);
                }
                if (curTex != null)
                    GML.device.texDispose(curTex);
            }
            curImg = nfo.img;
            curNfo = nfo;
            curTex = GML.device.texFromBitmap(ref curImg);
            if (onVisualChanged != null)
                onVisualChanged();
        }

        public Control control
        {
            get { return resultControl; }
            set { resultControl = value; }
        }

        static void processVisualStub(Object o)
        {
            ThreadImageRenderer mgr = (ThreadImageRenderer)o;
            mgr.processVisualThread();
        }

        protected delegate void onSwitchImgDelegate(RenderInfoBase newImg);
        void processVisualThread()
        {
            RenderInfoBase nfo;
            while (true)
            {
                nfo = visTasks.DequeueAllGetLast();
                nfo = drawImage(nfo);
                if (resultControl != null && nfo != null)
                {
                    try
                    {
                        resultControl.Invoke(new onSwitchImgDelegate(switchImage), new Object[] { nfo });
                    }
                    catch { };
                }
            }
        }

        protected abstract RenderInfoBase drawImage(RenderInfoBase info);
    }
}
