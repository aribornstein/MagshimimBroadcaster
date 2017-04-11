#include "stdafx.h"
#include "Encoder.h"

#pragma warning(disable:4996)

using namespace System;

Encoder::Encoder(int size_x, int size_y) : width(size_x), height(size_y), i(0)
{
	avcodec_register_all();
	c = NULL;
	/* find the video encoder */
	codec = avcodec_find_encoder(AV_CODEC_ID_H264);
	if (!codec) {
		fprintf(stderr, "Codec not found\n");
		exit(1);
	}


	c = avcodec_alloc_context3(codec);
	if (!c) {
		fprintf(stderr, "Could not allocate video codec context\n");
		exit(1);
	}

	/* put sample parameters */
	c->bit_rate = 1200000;
	c->width = size_x; // resolution must be a multiple of two (1280x720),(1900x1080),(720x480)
	c->height = size_y;
	c->time_base.num = 1;                                   // framerate numerator
	c->time_base.den = 20;                                  // framerate denominator
	c->gop_size = 10;                                       // emit one intra frame every ten frames
	c->max_b_frames = 1;                                    // maximum number of b-frames between non b-frames
	c->keyint_min = 1;                                      // minimum GOP size
	c->i_quant_factor = (float)0.71;                        // qscale factor between P and I frames
	c->b_frame_strategy = 20;                               ///// find out exactly what this does
	c->qcompress = (float)0.6;                              ///// find out exactly what this does
	c->qmin = 20;                                           // minimum quantizer
	c->qmax = 51;                                           // maximum quantizer
	c->max_qdiff = 4;                                       // maximum quantizer difference between frames
	c->refs = 4;                                            // number of reference frames
	c->trellis = 1;                                         // trellis RD Quantization
	c->pix_fmt = AV_PIX_FMT_YUV420P;                           // universal pixel format for video encoding
	c->codec_id = AV_CODEC_ID_H264;
	c->codec_type = AVMEDIA_TYPE_VIDEO;

	av_opt_set(c->priv_data, "preset", "slow", 0);

	/* open it */
	if (avcodec_open2(c, codec, NULL) < 0) {
		fprintf(stderr, "Could not open codec\n");
		exit(1);
	}

	frame = av_frame_alloc();

	if (!frame) {
		fprintf(stderr, "Could not allocate video frame\n");
		exit(1);
	}
	frame->format = c->pix_fmt;
	frame->width = c->width;
	frame->height = c->height;

	/* the image can be allocated by any means and av_image_alloc() is
	* just the most convenient way if av_malloc() is to be used */
	int ret = av_image_alloc(frame->data, frame->linesize, c->width, c->height,
		c->pix_fmt, 32);
	if (ret < 0) {
		fprintf(stderr, "Could not allocate raw picture buffer\n");
		exit(1);
	}

	pPixels = new RGBQUAD[width*height];


}


Encoder::~Encoder()
{
	avcodec_free_context(&c);
	av_freep(&frame->data[0]);
	av_frame_free(&frame);
	delete[] pPixels;
} 


char * Encoder::encode(char * im, int *size)
{
	int ret, got_output;

	av_init_packet(&pkt);
	pkt.data = NULL;    // packet data will be allocated by the encoder
	pkt.size = 0;

	fflush(stdout);
	RGBQUAD *pPixels = getBitmap(width, height);


	int nbytes = avpicture_get_size(AV_PIX_FMT_YUV420P, c->width, c->height);                                      // allocating outbuffer
	uint8_t* outbuffer = (uint8_t*)av_malloc(nbytes*sizeof(uint8_t));
	AVFrame* inpic = av_frame_alloc();													              // mandatory frame allocation

	frame->pts = i++;                                                                                              // setting frame pts
	avpicture_fill((AVPicture*)inpic, (uint8_t*)pPixels, AV_PIX_FMT_RGB32, c->width, c->height);                   // fill image with input screenshot
	avpicture_fill((AVPicture*)frame, outbuffer, AV_PIX_FMT_YUV420P, c->width, c->height);                         // clear output picture for buffer copy
	av_image_alloc(frame->data, frame->linesize, c->width, c->height, c->pix_fmt, 1);
	inpic->data[0] += inpic->linesize[0] * (height - 1);                                                      // flipping frame
	inpic->linesize[0] = -inpic->linesize[0];                                                                   // flipping frame

	struct SwsContext* fooContext = sws_getContext(width, height, AV_PIX_FMT_RGB32, c->width, c->height, AV_PIX_FMT_YUV420P, SWS_FAST_BILINEAR, NULL, NULL, NULL);
	sws_scale(fooContext, inpic->data, inpic->linesize, 0, c->height, frame->data, frame->linesize);          // converting frame size and format

	av_freep(&inpic->data[0]);
	av_frame_free(&inpic);
	
	ret = avcodec_encode_video2(c, &pkt, frame, &got_output);

	if (ret < 0) {
		fprintf(stderr, "Error encoding frame\n");
		exit(1);
	}
	char *data = nullptr;
	if (got_output)
	{
		data =  (char *)pkt.data;
		*size = pkt.size;
	}
	av_freep(&frame->data[0]);
	av_packet_unref(&pkt);
	return data;
}

RGBQUAD *Encoder::getBitmap(int screenWidth, int screenHeight)
{
	HWND hDesktopWnd = GetDesktopWindow();
	HDC hDesktopDC = GetDC(hDesktopWnd);
	HDC hCaptureDC = CreateCompatibleDC(hDesktopDC);
	HBITMAP hBmp = CreateCompatibleBitmap(GetDC(0), screenWidth, screenHeight);
	SelectObject(hCaptureDC, hBmp);
	BitBlt(hCaptureDC, 0, 0, screenWidth, screenHeight, hDesktopDC, 0, 0, SRCCOPY | CAPTUREBLT);
	BITMAPINFO bmi = { 0 };
	bmi.bmiHeader.biSize = sizeof(bmi.bmiHeader);
	bmi.bmiHeader.biWidth = screenWidth;
	bmi.bmiHeader.biHeight = screenHeight;
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 32;
	bmi.bmiHeader.biCompression = BI_RGB;
	GetDIBits(hCaptureDC, hBmp, 0, screenHeight, pPixels, &bmi, DIB_RGB_COLORS);

	ReleaseDC(hDesktopWnd, hDesktopDC);
	DeleteDC(hCaptureDC);
	DeleteObject(hBmp);

	return pPixels;
}