using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XnGFL
{
    /// <summary>
    /// Class loads and displays large preview of the image file
    /// </summary>
    public partial class ImagePreview : Form
    {
        Image img;

        public ImagePreview()
        {
            InitializeComponent();
        }

        public void showImage(string fname)
        {
            Size sz = this.Size;
            img = new Image(fname);
            //if(!fname.EndsWith("jpg", true, System.Globalization.CultureInfo.CurrentCulture))
                img.thumbnail = true;
            if (img.Load(sz.Width, sz.Height) != Common.GFL_ERROR.NOERROR)
            {
                img = null;
                return;
            }
            img.ConvertToImage();

            this.Visible = true;
        }

        public void hideImage()
        {
            this.Visible = false;
            if(img != null)
                img.Dispose();
            img = null;
        }

        private void ImagePreview_Paint(object sender, PaintEventArgs e)
        {
            if(img != null)
            {
                e.Graphics.DrawImage(img.image, 0, 0, img.image.Width, img.image.Height);
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, img.image.Width -1, img.image.Height -1);
            }
        }
    }
}
