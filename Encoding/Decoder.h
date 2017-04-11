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


class Decoder
{
public:
	Decoder();
	~Decoder();
	char * decode(char *pkt, int size);


private:
	AVCodec *codec;
	AVCodecContext *c;
	AVFrame *frame;
	AVFrame* RGBFrame;
	AVPacket avpkt;

	int i;
	std::string data;

};
