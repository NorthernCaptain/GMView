// XnGFLWrap.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "XnGFLWrap.h"


#include <list>
#include <queue>

std::queue<XNGFL_MetaContext*> freeCtxPool;

// This is an example of an exported variable
XNGFLWRAP_API int nXnGFLWrap=0;

// This is an example of an exported function.
XNGFLWRAP_API int fnXnGFLWrap(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see XnGFLWrap.h for the class definition
CXnGFLWrap::CXnGFLWrap()
{
	return;
}

void XNGFL_MetaContext::initCtx()
{
	initCtx(bitmap);
}

void XNGFL_MetaContext::initCtx(GFL_BITMAP *bmp)
{
	bitmap = bmp;

	exiftextdata = 0;

	if(!gflBitmapHasEXIF(bmp))
		return;

	exiftextdata = gflBitmapGetEXIF(bmp, GFL_EXIF_WANT_MAKERNOTES);
	curTextItem = 0;
}

void XNGFL_MetaContext::freeAll()
{
	if(exiftextdata != 0)
		gflFreeEXIF(exiftextdata);
	exiftextdata = 0;
}

XNGFL_MetaContext::~XNGFL_MetaContext()
{
	freeAll();
}

extern "C" {
XNGFL_MetaContext*  xnCreateMetaContext(GFL_BITMAP* bitmap)
{
	XNGFL_MetaContext* ctx;
	if(false && !freeCtxPool.empty())
	{
		ctx = freeCtxPool.front();
		ctx->initCtx(bitmap);
		freeCtxPool.pop();
	}
	else
	{
		ctx = new XNGFL_MetaContext(bitmap);
		ctx->initCtx();
	}
	return ctx;
}

int xnHasEXIF(XNGFL_MetaContext* ctx)
{
	if(ctx == 0)
		return 0;

	return ctx->hasEXIF();
}

GFL_EXIF_ENTRY* xnStartEXIF1(XNGFL_MetaContext* ctx)
{
	if(ctx == 0 || ctx->exiftextdata == 0 || ctx->exiftextdata->NumberOfItems == 0)
		return 0;
	ctx->curTextItem = 0;
	return &ctx->exiftextdata->ItemsList[ctx->curTextItem];
}

GFL_EXIF_ENTRY* xnNextEXIF1(XNGFL_MetaContext* ctx)
{
	ctx->curTextItem ++;
	if(ctx == 0 || ctx->exiftextdata == 0 || ctx->exiftextdata->NumberOfItems <= ctx->curTextItem)
		return 0;
	return &ctx->exiftextdata->ItemsList[ctx->curTextItem];
}


void xnFreeMetaContext(XNGFL_MetaContext* ctx)
{
	freeCtxPool.push(ctx);
}


void xnDisposeAvailable()
{
	while(!freeCtxPool.empty())
	{
		XNGFL_MetaContext* ctx = freeCtxPool.front();
		freeCtxPool.pop();
		delete ctx;
	}
}

}