using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XnGFL
{
    public class ImageWrap
    {
        public enum ORIGIN: ushort
        {
            Top_Left = 0,
            Right = 1,
            Bottom = 0x10
        }

        public enum BITMAP_TYPE: ushort
        {
            BINARY  = 0x0001,
            GREY  =   0x0002,
            COLORS =  0x0004,
            RGB  =    0x0010,
            RGBA =    0x0020,
            BGR =     0x0040,
            ABGR =    0x0080,
            BGRA =    0x0100,
            ARGB =    0x0200,
            CMYK =    0x0400,
            BITS24 =  0x1000, /* Only for gflBitmapTypeIsSupportedByIndex or gflBitmapTypeIsSupportedByName */
            BITS32 =  0x2000,
            BITS48 =  0x4000,
            BITS64 =  0x8000
        }

        public enum CORDER: ushort
        {
            INTERLEAVED = 0,
            SEQUENTIAL = 1,
            SEPARATE =2
        }

        public enum CTYPE: ushort
        {
            GREYSCALE =   0,
            RGB =         1,
            BGR =         2,
            RGBA =        3,
            ABGR =        4,
            CMY =         5,
            CMYK =        6
        }

        public enum LOAD_FLAGS : uint
        {
            SKIP_ALPHA	=			  0x00000001, /* Alpha not loaded (32bits only)                     */
            IGNORE_READ_ERROR =	      0x00000002,
            BY_EXTENSION_ONLY =       0x00000004, /* Use only extension to recognize format. Faster     */
            READ_ALL_COMMENT =        0x00000008, /* Read Comment in GFL_FILE_DESCRIPTION               */
            FORCE_COLOR_MODEL =       0x00000010, /* Force to load picture in the ColorModel            */
            PREVIEW_NO_CANVAS_RESIZE =0x00000020, /* With gflLoadPreview, width & height are the maximum box */
            BINARY_AS_GREY =          0x00000040, /* Load Black&White file in greyscale                 */
            ORIGINAL_COLORMODEL =     0x00000080, /* If the colormodel is CMYK, keep it                 */
            ONLY_FIRST_FRAME =        0x00000100, /* No search to check if file is multi-frame          */
            ORIGINAL_DEPTH =          0x00000200, /* In the case of 10/16 bits per component            */
            METADATA =                0x00000400, /* Read all metadata                                  */
            COMMENT =                 0x00000800, /* Read comment                                       */
            HIGH_QUALITY_THUMBNAIL =  0x00001000, /* gflLoadThumbnail                                   */
            EMBEDDED_THUMBNAIL =      0x00002000, /* gflLoadThumbnail                                   */
            ORIENTED_THUMBNAIL =      0x00004000, /* gflLoadThumbnail                                   */
            ORIGINAL_EMBEDDED_THUMBNAIL=0x00008000, /* gflLoadThumbnail                                   */
            ORIENTED =                0x00008000 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LOAD_PARAMS
        {
            public UInt32 Flags;
            public Int32 FormatIndex;
            public Int32 ImageWanted;
            public ORIGIN Origin;
            public BITMAP_TYPE ColorModel;
            public UInt32 LinePadding;
            public Byte DefaultAlpha;
            public Byte Reserved1;
            public UInt16 Reserved2;
            public Int32 Width;
            public Int32 Height;
            public UInt32 Offset;
            public CORDER ChannelOrder;
            public CTYPE ChannelType;
            public UInt16 PcdBase;
            public UInt16 EpsDpi;
            public Int32 EpsWidth;
            public Int32 EpsHeight;

		    public UInt16 LutType; /* GFL_LUT_TO8BITS, GFL_LUT_TO10BITS, GFL_LUT_TO12BITS, GFL_LUT_TO16BITS */
		    public UInt16 Reserved3; 
		    public IntPtr LutData; /* RRRR.../GGGG..../BBBB.....*/
            [MarshalAs(UnmanagedType.LPStr)]
		    public string LutFilename; 
    
		/* 
		 * Camera RAW only
		 */
		    public Byte CameraRawUseAutomaticBalance; 
		    public Byte CameraRawUseCameraBalance; 
		    public Byte CameraRawHighlight; 
		    public Byte Reserved4; 
		    public float CameraRawGamma; 
		    public float CameraRawBrightness; 
		    public float CameraRawRedScaling; 
		    public float CameraRawBlueScaling;

            IntPtr Read;
            IntPtr Tell;
            IntPtr Seek;
            IntPtr AllocateBitmap;
            IntPtr AllocateParams;
            IntPtr Progress;
            IntPtr ProgressParams;
            IntPtr WantCancel;
            IntPtr WantCancelParams;
            IntPtr SetLine;
            IntPtr SetLineParams;

    		public IntPtr UserParams; 
        }

        [StructLayout(LayoutKind.Sequential)]
        public class BITMAP
        {
            public BITMAP_TYPE Type;
            public ORIGIN Origin;
            public Int32 Width;
            public Int32 Height;
            public UInt32 BytesPerLine;
            public Int16 LinePadding;
            public UInt16 BitsPerComponent;  /* 1, 8, 10, 12, 16 */
            public UInt16 ComponentsPerPixel;/* 1, 3, 4  */
            public UInt16 BytesPerPixel;     /* Only valid for 8 or more bits */
            public UInt16 Xdpi;
            public UInt16 Ydpi;
            public Int16 TransparentIndex;  /* -1 if not used */
            public Int16 Reserved;
            public Int32 ColorUsed;
            public IntPtr ColorMap;
            public IntPtr Data;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Comment;
            public IntPtr MetaData;

            public Int32 XOffset;
            public Int32 YOffset;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Name;
        }

        /// <summary>
        /// The gflGetDefaultLoadParams function sets the GFL_LOAD_PARAMS structure with default values. 
        /// To use before call of gflLoadBitmap. 
        /// </summary>
        /// <param name="param"></param>
        [DllImport(Common.GFL_DLL, EntryPoint="gflGetDefaultLoadParams")]
        public static extern void GetDefaultLoadParams(ref LOAD_PARAMS param);

        /// <summary>
        /// The gflGetDefaultThumbnailParams function sets the GFL_LOAD_PARAMS structure with default values. 
        /// To use before call of gflLoadThumbnail*.
        /// </summary>
        /// <param name="param"></param>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflGetDefaultThumbnailParams")]
        public static extern void GetDefaultThumbnailParams(ref LOAD_PARAMS param);

        /// <summary>
        /// The gflLoadBitmap function load a picture file into memory
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="bitmap"></param>
        /// <param name="param"></param>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflLoadBitmap")]
        public static extern Common.GFL_ERROR LoadBitmap(IntPtr filename, out BITMAP bitmap,
                ref LOAD_PARAMS param, IntPtr FileInfo);

        /// <summary>
        /// The gflLoadThumbnail function load a picture file as a thumbnail into memory
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bitmap"></param>
        /// <param name="param"></param>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflLoadThumbnail")]
        public static extern Common.GFL_ERROR LoadThumbnail(IntPtr filename, Int32 width, Int32 height, 
                out BITMAP bitmap,
                ref LOAD_PARAMS param, IntPtr FileInfo);

        /// <summary>
        /// The gflConvertBitmapIntoDIB function converts a GFL_BITMAP in a Windows Device Independant Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(Common.GFLE_DLL, EntryPoint = "gflConvertBitmapIntoDIB")]
        public static extern Common.GFL_ERROR ConvertBitmapIntoDIB(IntPtr bitmap, out IntPtr handle);

        [DllImport(Common.GFLE_DLL, EntryPoint = "gflConvertBitmapIntoDDB")]
        public static extern Common.GFL_ERROR ConvertBitmapIntoDDB(BITMAP bitmap, out IntPtr hbitmap);

        /// <summary>
        /// The gflLoadBitmapIntoDIB function load a picture file into a Windows Device Independant Bitmap.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="handle"></param>
        /// <param name="param"></param>
        /// <param name="FileInfo"></param>
        /// <returns></returns>
        [DllImport(Common.GFLE_DLL, EntryPoint = "gflLoadBitmapIntoDIB")]
        public static extern Common.GFL_ERROR LoadIntoDIBHandle(IntPtr filename,
                out IntPtr handle,
                ref LOAD_PARAMS param, IntPtr FileInfo);

        [DllImport(Common.GFLE_DLL, EntryPoint = "gflLoadBitmapIntoDDB")]
        public static extern Common.GFL_ERROR LoadIntoDDBHandle(IntPtr filename,
                out IntPtr handle,
                ref LOAD_PARAMS param, IntPtr FileInfo);

        /// <summary>
        /// The gflFreeBitmap function frees a GFL_BITMAP structure, and his content. 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        [DllImport(Common.GFL_DLL, EntryPoint = "gflFreeBitmap")]
        public static extern Common.GFL_ERROR FreeBitmap(BITMAP bitmap);

    }
}
