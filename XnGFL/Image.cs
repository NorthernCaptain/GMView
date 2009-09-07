using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace XnGFL
{
    public class Image: ListViewItem, IDisposable
    {
        /// <summary>
        /// Original filename
        /// </summary>
        string fname;
        /// <summary>
        /// Parameters for loading image thumbnail using GFL
        /// </summary>
        ImageWrap.LOAD_PARAMS param;
        ImageWrap.BITMAP ibitmap;
        bool is_thumbnail = false;

        /// <summary>
        /// Bitmap image for displaying
        /// </summary>
        public Bitmap image;

        /// <summary>
        /// EXIF information retrieved from image using Exiv2
        /// </summary>
        public Exiv2Net.Image exiv;

        public Image(string image_file_name)
        {
            fname = image_file_name;
        }

        public Image()
        {            
        }

        public string filename
        {
            get { return fname; }
            set { fname = value; }
        }

        /// <summary>
        /// Gets or sets thumbnail mode. Use before loading image
        /// </summary>
        public bool thumbnail
        {
            get { return is_thumbnail; }
            set { is_thumbnail = value; }
        }

        /// <summary>
        /// Loads image into memory using default params
        /// </summary>
        /// <returns></returns>
        public Common.GFL_ERROR Load()
        {
            return Load(0, 0);
        }

        /// <summary>
        /// Loads image or it thumbnail into memory. For thumbnail uses given size
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public Common.GFL_ERROR Load(int w, int h)
        {
            IntPtr fname = Marshal.StringToHGlobalAnsi(this.fname);
            try
            {
                Common.GFL_ERROR errcode;
                if (is_thumbnail)
                {
                    ImageWrap.GetDefaultThumbnailParams(ref param);
                    param.Flags |= (uint)ImageWrap.LOAD_FLAGS.EMBEDDED_THUMBNAIL;
                    param.Flags |= (uint)ImageWrap.LOAD_FLAGS.ORIENTED_THUMBNAIL;
                    param.Flags |= (uint)ImageWrap.LOAD_FLAGS.METADATA;
                    errcode = ImageWrap.LoadThumbnail(fname, w, h, out ibitmap, ref param, IntPtr.Zero);
                }
                else
                {
                    ImageWrap.GetDefaultLoadParams(ref param);
                    param.Flags |= (uint)ImageWrap.LOAD_FLAGS.METADATA;
                    errcode = ImageWrap.LoadBitmap(fname, out ibitmap, ref param, IntPtr.Zero);
                    ///TODO: resize image code if we have w!=h!=0
                }
                return errcode;
            }
            finally
            {
                Marshal.FreeHGlobal(fname);
            }
        }

        /// <summary>
        /// Converts bitmap data to a System Bitmap object (image)
        /// </summary>
        /// <returns></returns>
        public Common.GFL_ERROR ConvertToImage()
        {
            IntPtr handle;
            Common.GFL_ERROR errcode = ImageWrap.ConvertBitmapIntoDDB(ibitmap, out handle);
            if (errcode != Common.GFL_ERROR.NOERROR)
                return errcode;

            image = Bitmap.FromHbitmap(handle);
            return errcode;
        }

        /// <summary>
        /// Alternative variant to Load() call, loads image directly into DDB and then to System Bitmap object.
        /// Loads full image only. Could not load thumbnails.
        /// </summary>
        /// <param name="image_file_name"></param>
        /// <returns></returns>
        public Common.GFL_ERROR LoadImage(string image_file_name)
        {
            IntPtr fname = Marshal.StringToHGlobalAnsi(image_file_name);
            IntPtr handle;
            try
            {
                ImageWrap.GetDefaultLoadParams(ref param);
                param.ColorModel = ImageWrap.BITMAP_TYPE.RGB | ImageWrap.BITMAP_TYPE.BITS24;
                Common.GFL_ERROR errcode = ImageWrap.LoadIntoDDBHandle(fname, out handle, ref param, IntPtr.Zero);
                if (errcode != Common.GFL_ERROR.NOERROR)
                    return errcode;

                image = Bitmap.FromHbitmap(handle);
                return errcode;
            }
            finally
            {
                Marshal.FreeHGlobal(fname);
            }

        }

        #region IDisposable Members

        /// <summary>
        /// Dispose all internal objects
        /// </summary>
        public void Dispose()
        {
            if (ctx != null)
            {
                MetaDataWrap.xnFreeMetaContext(ctx);
                ctx = null;
            }

            if (ibitmap != null)
            {
                ImageWrap.FreeBitmap(ibitmap);
                ibitmap = null;
            }

            if (image != null)
            {
                image.Dispose();
                image = null;
            }

        }

        #endregion

        #region GFL Exif routines. Very small info
        /// <summary>
        /// Return true if image has Exif tagged information
        /// </summary>
        public bool hasExif
        {
            get { return MetaDataWrap.xnHasEXIF(ctx) != 0; }
        }

        private MetaDataWrap.XnMetaContext ctx;

        public List<MetaDataWrap.GFL_EXIF_ENTRYEX> getInternalExif()
        {
            ctx = MetaDataWrap.xnCreateMetaContext(ibitmap);
            if (MetaDataWrap.xnHasEXIF(ctx) != 0)
            {
                MetaDataWrap.GFL_EXIF_ENTRYEX item = MetaDataWrap.xnStartEXIF(ctx);

                List<MetaDataWrap.GFL_EXIF_ENTRYEX> lst = new List<MetaDataWrap.GFL_EXIF_ENTRYEX>();

                while (item != null)
                {
                    lst.Add(item);
                    item = MetaDataWrap.xnNextEXIF(ctx);
                }

                return lst;
            }
            return null;
        }


        public List<MetaDataWrap.GFL_EXIF_ENTRY> getInternalExif1()
        {
            ctx = MetaDataWrap.xnCreateMetaContext(ibitmap);
            if (MetaDataWrap.xnHasEXIF(ctx) != 0)
            {
                MetaDataWrap.GFL_EXIF_ENTRY item = MetaDataWrap.xnStartEXIF1(ctx);

                List<MetaDataWrap.GFL_EXIF_ENTRY> lst = new List<MetaDataWrap.GFL_EXIF_ENTRY>();

                while (item != null)
                {
                    lst.Add(item);
                    item = MetaDataWrap.xnNextEXIF1(ctx);
                }

                return lst;
            }
            return null;
        }

        #endregion

        public EXIFData exif;

        /// <summary>
        /// Reads EXIF tags from file and decodes part of them.
        /// </summary>
        public void loadExif()
        {
            ctx = MetaDataWrap.xnCreateMetaContext(ibitmap);
            exif = new EXIFData(ctx);
        }

        /// <summary>
        /// Does image have EXIF information?
        /// </summary>
        public bool hasExiv
        {
            get { return exif != null ? exif.hasEXIF : false; }
        }
    }
}
