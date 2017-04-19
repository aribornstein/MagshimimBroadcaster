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
#include <vector>

#pragma comment(lib,"user32.lib") 
#pragma comment(lib,"Gdi32.lib") 


#define H264_INBUF_SIZE 16384                                                           /* number of bytes we read per chunk */


class Decoder
{
public:
	Decoder(int width, int height);
	~Decoder();
	char * ReadFrame(char *pkt, int size, int *imSize);

private:
	int readBuffer();                                                                      /* read a bit more data from the buffer */
	bool update(bool &needsMoreBytes);
	bool decodeFrame(uint8_t* data, int size);

	AVCodec *codec;
	AVCodecContext *c;
	AVCodecParserContext* parser;
	AVFrame *frame;
	AVFrame* RGBFrame;
	AVPacket* avpkt;
	struct SwsContext* fooContext;
	uint8_t inbuf[H264_INBUF_SIZE + FF_INPUT_BUFFER_PADDING_SIZE];                         /* used to read chunks from the file */
	std::vector<uint8_t> buffer;
	std::vector<uint8_t> fullData;
	int cursor;

};
