#pragma once



extern "C"
{
	#include "libavutil/opt.h"
	#include "libavcodec/avcodec.h"
	#include "libavutil/channel_layout.h"
	#include "libavutil/common.h"
	#include "libavutil/imgutils.h"
	#include "libavutil/mathematics.h"
	#include "libavutil/samplefmt.h"
	#include "libswScale/swscale.h"
}

#pragma comment(lib, "avcodec.lib")
#pragma comment(lib, "avutil.lib")
#pragma comment(lib, "swScale.lib")


#define INBUF_SIZE 4096
#define AUDIO_INBUF_SIZE 20480
#define AUDIO_REFILL_THRESH 4096


#include <Windows.h>
#include <string>

#pragma comment(lib,"user32.lib") 
#pragma comment(lib,"Gdi32.lib") 


class Encoder
{
public:
	Encoder(int size_x, int size_y);
	~Encoder();
	char * encode(char *im, int *size);
	RGBQUAD *getBitmap(int *size);


private:
	AVCodec *codec;
	AVCodecContext *c;
	AVFrame *frame;
	AVFrame* inpic;
	AVPacket *pkt;
	int width;
	int height;
	int i;
	RGBQUAD *pPixels;
	RGBQUAD *bitmapArray;
	uint8_t* outbuffer;
	struct SwsContext* fooContext;

};

