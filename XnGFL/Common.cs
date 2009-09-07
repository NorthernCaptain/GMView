using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XnGFL
{
    public class Common
    {
        public const string GFL_DLL = "libgfl311.dll";
        public const string GFLE_DLL = "libgfle311.dll";
        public const string XNWRAP_DLL = "XnGFLWrap.dll";

        public enum GFL_ERROR : short
        {
            NOERROR = 0,
            ERROR_FILE_OPEN = 1,
            ERROR_FILE_READ = 2,
            ERROR_FILE_CREATE = 3,
            ERROR_FILE_WRITE = 4,
            ERROR_NO_MEMORY = 5,
            ERROR_UNKNOWN_FORMAT = 6,

            ERROR_BAD_BITMAP = 7,

            ERROR_BAD_FORMAT_INDEX = 10,
            ERROR_BAD_PARAMETERS = 50,

            UNKNOWN_ERROR = 255
        }

        [DllImport(GFL_DLL, EntryPoint ="gflLibraryInit")]
        public static extern GFL_ERROR LibraryInit();

        [DllImport(GFL_DLL, EntryPoint = "gflGetErrorString")]
        public static extern IntPtr GetErrorPtr(GFL_ERROR errcode);

        public static string GetErrorString(GFL_ERROR errcode)
        {
            IntPtr ptr = GetErrorPtr(errcode);
            return Marshal.PtrToStringAnsi(ptr);
        }
    }
}
