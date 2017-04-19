#include "stdafx.h"
#include "Decoder.h"

#include <iterator>

using namespace System;

Decoder::Decoder(int width, int height) : cursor(0)
{
	avcodec_register_all();
	c = NULL;

	avpkt = av_packet_alloc();

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
		c->flags |= CODEC_FLAG_TRUNCATED; /* we do not send complete frames 

	/* open it */
	if (avcodec_open2(c, codec, NULL) < 0) {
		fprintf(stderr, "Could not open codec\n");
		exit(1);
	}

	c->width = width;
	c->height = height;


	frame = av_frame_alloc();
	if (!frame)
	{
		fprintf(stderr, "Could not allocate video frame\n");
		exit(1);
	}

	frame->format = AV_PIX_FMT_YUV420P;
	frame->width = c->width;
	frame->height = c->height;

	parser = av_parser_init(AV_CODEC_ID_H264);
	if (!parser)
	{
		Console::WriteLine("Error: cannot create H264 parser.");
		exit(1);
	}

	RGBFrame = av_frame_alloc();
	if (!RGBFrame)
	{
		fprintf(stderr, "Could not allocate video frame\n");
		exit(1);
	}

	RGBFrame->format = AV_PIX_FMT_RGB24;
	RGBFrame->width = c->width;
	RGBFrame->height = c->height;

	int ret = av_image_alloc(RGBFrame->data, RGBFrame->linesize, c->width, c->height,
		AV_PIX_FMT_RGB24, 24); //I was an idiot and allocated a YUV image by mistake and it took me an hour to find.
	if (ret < 0) {
		fprintf(stderr, "Could not allocate raw picture buffer\n");
		exit(1);
	}


	fooContext = sws_getContext(width, height, AV_PIX_FMT_YUV420P,
		width, height, AV_PIX_FMT_RGB24, NULL, NULL, NULL, NULL);

	
}

Decoder::~Decoder()
{
	avcodec_free_context(&c);
	av_frame_free(&frame);
	av_freep(&RGBFrame->data[0]);
	av_frame_free(&RGBFrame);
	av_free_packet(avpkt);
}

char * Decoder::ReadFrame(char * pkt, int size, int *imSize)
{
	
	std::vector<uint8_t> packetData;
	for (int i = 0; i < size; i++) //initialize packetData.
		packetData.push_back(pkt[i]);


	//append the packet's data to out data.
	fullData.insert(fullData.end(), packetData.begin(), packetData.end());



	bool needs_more = false;

	while (!update(needs_more)) {
		if (needs_more) {
			if (readBuffer() == 0)
			{
				return nullptr;
			}
		}
	}

	sws_scale(fooContext, frame->data, frame->linesize, 0, c->height, RGBFrame->data, RGBFrame->linesize);          // converting frame size and format
	*imSize = RGBFrame->height*RGBFrame->width * 3;
	

	char *im = new char[*imSize];
	int ret = av_image_copy_to_buffer((uint8_t *)im, *imSize, RGBFrame->data, RGBFrame->linesize, AV_PIX_FMT_RGB24, RGBFrame->width, RGBFrame->height, 24);
	
	if (ret > 0)
		return im;
	return nullptr;
	
}


bool Decoder::update(bool& needsMoreBytes)
{
	needsMoreBytes = false;

	if (buffer.size() == 0) {
		needsMoreBytes = true;
		return false;
	}


	uint8_t* data = NULL;
	int size = 0;
	
	int len = av_parser_parse2(parser, c, &data, &size,
		&buffer[0], buffer.size(), 0, 0, AV_NOPTS_VALUE);

	if (size == 0 && len >= 0) {
		needsMoreBytes = true;
		return false;
	}

	if (len) {
		if (decodeFrame(&data[0], size))
		{
			buffer.erase(buffer.begin(), buffer.begin() + len);
			return true;
		}
		buffer.erase(buffer.begin(), buffer.begin() + len);
		return false;
	}

	return false;

	

}

bool Decoder::decodeFrame(uint8_t * data, int size)
{
	AVPacket pkt;
	int got_picture = 0;
	int len = 0;

	av_init_packet(&pkt);

	pkt.data = data;
	pkt.size = size;

	len = avcodec_decode_video2(c, frame, &got_picture, &pkt);
	if (len < 0) {
		printf("Error while decoding a frame.\n");
		return false;
	}

	if (got_picture == 0) {
		return false;
	}

	return true;
}

int Decoder::readBuffer()
{
	int bytesToRead = fullData.size() - cursor + 1 > H264_INBUF_SIZE ? H264_INBUF_SIZE : (fullData.size() - cursor);
	std::copy(fullData.begin() + cursor, fullData.begin() + cursor + bytesToRead, std::begin(inbuf));
	cursor += bytesToRead;
	std::copy(inbuf, inbuf + bytesToRead, std::back_inserter(buffer));
	return bytesToRead;
}
