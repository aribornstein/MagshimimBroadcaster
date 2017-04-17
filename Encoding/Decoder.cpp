#include "stdafx.h"
#include "Decoder.h"


using namespace System;

Decoder::Decoder()
{
	avcodec_register_all();
	c = NULL;

	av_init_packet(&avpkt);

	/* find the video decoder*/
	codec = avcodec_find_decoder(AV_CODEC_ID_H264);
	if (!codec) {
		fprintf(stderr, "Codec not found\n");
		exit(1);
	}

	c = avcodec_alloc_context3(codec);
	if (!c) {
		fprintf(stderr, "Could not allocate video codec context\n");
		exit(1);
	}

	if (codec->capabilities&CODEC_CAP_TRUNCATED)
		c->flags |= CODEC_FLAG_TRUNCATED; /* we do not send complete frames */

	/* open it */
	if (avcodec_open2(c, codec, NULL) < 0) {
		fprintf(stderr, "Could not open codec\n");
		exit(1);
	}

	frame = av_frame_alloc();
	if (!frame)
	{
		fprintf(stderr, "Could not allocate video frame\n");
		exit(1);
	}

	RGBFrame = av_frame_alloc();
	if (!RGBFrame)
	{
		fprintf(stderr, "Could not allocate video frame\n");
		exit(1);
	}

	
}

Decoder::~Decoder()
{
	avcodec_free_context(&c);
	av_freep(&frame->data[0]);
	av_frame_free(&frame);
	av_freep(&RGBFrame->data[0]);
	av_frame_free(&RGBFrame);
}

char * Decoder::decode(char * pkt, int size)
{
	data += pkt;
	avpkt.size = data.length();
	avpkt.data = (uint8_t *)data.c_str();
	
	int *got_frame = 0;

	int len = avcodec_decode_video2(c, frame, got_frame, &avpkt);
	if (len < 0)
	{
		Console::WriteLine("Error while decoding frame,");
		return nullptr;
	}
	if (got_frame)
	{
		struct SwsContext* fooContext = sws_getContext(frame->width, frame->height, AV_PIX_FMT_YUV420P, 
			                                           frame->width, frame->height, AV_PIX_FMT_RGB24, NULL, NULL, NULL, NULL);
		sws_scale(fooContext, frame->data, frame->linesize, 0, c->height, frame->data, frame->linesize);          // converting frame size and format

	}

}
