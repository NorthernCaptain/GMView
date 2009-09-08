using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XnGFL
{
    public class MetaDataWrap
    {
        public enum EXIF_FLAG
        {
            IFD_0 = 0x0001,
            EXIF_IFD = 0x0002,
            INTEROPERABILITY_IFD = 0x0004,
            IFD_THUMBNAIL = 0x0008,
            GPS_IFD = 0x0010,
            MAKERNOTE_IFD = 0x0020
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GFL_EXIF_ENTRY
        {
		    public EXIF_FLAG Flag; /* EXIF_...IFD */
		    public UInt32 Tag;
            [MarshalAs(UnmanagedType.LPStr)]
		    public string Name;
            [MarshalAs(UnmanagedType.LPStr)]
		    public string Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GFL_EXIF_DATA
        {
            public UInt32 NumberOfItems;
            public IntPtr ItemsList;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GFL_EXIF_ENTRYEX
        {
            public UInt16 Tag;
            public UInt16 Format;
            public Int32 Ifd;
            public Int32 NumberOfComponents;
            public UInt32 Value;
            public Int32 DataLength;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Data;
            public IntPtr Next;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class GFL_EXIF_DATAEX
        {
            public IntPtr Root;
            public Int32 UseMsbf;
        }

        /// <summary>
        /// The gflBitmapHasEXIF function is used to know if the picture has EXIF metadata.
        /// </summary>
        /// <param name="ibitmap"></param>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflBitmapHasEXIF")]
        public static extern int BitmapHasEXIF(ImageWrap.BITMAP ibitmap);

        /// <summary>
        /// The gflBitmapGetEXIF function returns EXIF metadata in a readable form.
        /// </summary>
        /// <param name="ibitmap"></param>
        /// <param name="flags"></param>
        /// <returns>GFL_EXIF_DATA</returns>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflBitmapGetEXIF")]
        public static extern GFL_EXIF_DATA BitmapGetEXIF(ImageWrap.BITMAP ibitmap, UInt32 flags);

        /// <summary>
        /// The gflBitmapGetEXIF2 function returns EXIF metadata in a readable form.
        /// </summary>
        /// <param name="ibitmap"></param>
        /// <returns>GFL_EXIF_DATAEX</returns>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflBitmapGetEXIF2")]
        public static extern GFL_EXIF_DATAEX BitmapGetEXIF2(ImageWrap.BITMAP ibitmap);

        [StructLayout(LayoutKind.Sequential)]
        public class XnMetaContext
        {
            public IntPtr bitmap;
            public IntPtr exifdata;
            public IntPtr curItem;
            public Int32 numItems;
        }

        public static readonly object ctxLock = new object();

        /// <summary>
        /// Creates meta context - wrapper for retrieving EXIF metadata from GLF bitmap
        /// </summary>
        /// <param name="ibitmap"></param>
        /// <returns></returns>
        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnCreateMetaContext")]
        public static extern XnMetaContext xnCreateMetaContext(ImageWrap.BITMAP ibitmap);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnHasEXIF")]
        public static extern int xnHasEXIF(XnMetaContext ctx);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnStartEXIF")]
        public static extern GFL_EXIF_ENTRYEX xnStartEXIF(XnMetaContext ctx);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnNextEXIF")]
        public static extern GFL_EXIF_ENTRYEX xnNextEXIF(XnMetaContext ctx);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnStartEXIF1")]
        public static extern GFL_EXIF_ENTRY xnStartEXIF1(XnMetaContext ctx);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnNextEXIF1")]
        public static extern GFL_EXIF_ENTRY xnNextEXIF1(XnMetaContext ctx);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnFreeMetaContext")]
        public static extern void xnFreeMetaContext(XnMetaContext ctx);

        [DllImport(Common.XNWRAP_DLL, EntryPoint = "xnDisposeAvailable")]
        public static extern void xnDisposeAvailable();


    }
}
