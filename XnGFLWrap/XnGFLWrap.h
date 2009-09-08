// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the XNGFLWRAP_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// XNGFLWRAP_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef XNGFLWRAP_EXPORTS
#define XNGFLWRAP_API __declspec(dllexport)
#else
#define XNGFLWRAP_API __declspec(dllimport)
#endif


#include "libgfl.h"

// This class is exported from the XnGFLWrap.dll
class XNGFLWRAP_API CXnGFLWrap {
public:
	CXnGFLWrap(void);
	// TODO: add your methods here.
};

extern XNGFLWRAP_API int nXnGFLWrap;

XNGFLWRAP_API int fnXnGFLWrap(void);

extern "C"
{

struct XNGFL_MetaContext
{
	GFL_BITMAP* bitmap;
	GFL_EXIF_DATA* exiftextdata;
	int            curTextItem;

	XNGFL_MetaContext(GFL_BITMAP *bmp)
	{
		bitmap = bmp;
		curTextItem = 0;
	}

	//Initializes context env
	void initCtx();
	void initCtx(GFL_BITMAP *bmp);

	//Do we have exif info or no?
	int hasEXIF() { return exiftextdata != 0; }

	void freeAll();
	~XNGFL_MetaContext();
};

struct XNGFL_EXIF_ITEM
{
	int   IFD;
	int   Tag;
	int   Format;
	UINT32   Value;
};

XNGFLWRAP_API XNGFL_MetaContext*  xnCreateMetaContext(GFL_BITMAP* bitmap);
XNGFLWRAP_API int xnHasEXIF(XNGFL_MetaContext* ctx);
XNGFLWRAP_API GFL_EXIF_ENTRYEX* xnNextEXIF(XNGFL_MetaContext* ctx);
XNGFLWRAP_API GFL_EXIF_ENTRYEX* xnStartEXIF(XNGFL_MetaContext* ctx);
XNGFLWRAP_API GFL_EXIF_ENTRY* xnNextEXIF1(XNGFL_MetaContext* ctx);
XNGFLWRAP_API GFL_EXIF_ENTRY* xnStartEXIF1(XNGFL_MetaContext* ctx);
XNGFLWRAP_API void xnFreeMetaContext(XNGFL_MetaContext* ctx);
XNGFLWRAP_API void xnDisposeAvailable();
}


