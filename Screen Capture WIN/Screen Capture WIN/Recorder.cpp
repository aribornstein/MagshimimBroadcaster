#include "stdafx.h"
#include "Recorder.h"


using namespace cv;
using namespace std;


Recorder::Recorder(std::string name, int fps)
{
	_filename = name;
	_framerate = fps;
}


Mat Recorder::hwnd2mat(HWND hwnd) {

	HDC hwindowDC, hwindowCompatibleDC;

	int height, width, srcheight, srcwidth;
	HBITMAP hbwindow;
	Mat src;
	BITMAPINFOHEADER  bi;

	hwindowDC = GetDC(hwnd);
	hwindowCompatibleDC = CreateCompatibleDC(hwindowDC);
	SetStretchBltMode(hwindowCompatibleDC, COLORONCOLOR);

	RECT windowsize;    // get the height and width of the screen
	GetClientRect(hwnd, &windowsize);

	srcheight = windowsize.bottom;
	srcwidth = windowsize.right;
	height = windowsize.bottom;  //change this to whatever size you want to resize to
	width = windowsize.right;

	src.create(height, width, CV_8UC4);

	// create a bitmap
	hbwindow = CreateCompatibleBitmap(hwindowDC, width, height);
	bi.biSize = sizeof(BITMAPINFOHEADER);    //http://msdn.microsoft.com/en-us/library/windows/window/dd183402%28v=vs.85%29.aspx
	bi.biWidth = width;
	bi.biHeight = -height;  //this is the line that makes it draw upside down or not
	bi.biPlanes = 1;
	bi.biBitCount = 32;
	bi.biCompression = BI_RGB;
	bi.biSizeImage = 0;
	bi.biXPelsPerMeter = 0;
	bi.biYPelsPerMeter = 0;
	bi.biClrUsed = 0;
	bi.biClrImportant = 0;

	// use the previously created device context with the bitmap
	SelectObject(hwindowCompatibleDC, hbwindow);
	// copy from the window device context to the bitmap device context
	StretchBlt(hwindowCompatibleDC, 0, 0, width, height, hwindowDC, 0, 0, srcwidth, srcheight, SRCCOPY); //change SRCCOPY to NOTSRCCOPY for wacky colors !
	GetDIBits(hwindowCompatibleDC, hbwindow, 0, height, src.data, (BITMAPINFO *)&bi, DIB_RGB_COLORS);  //copy from hwindowCompatibleDC to hbwindow

																									   // avoid memory leak
	DeleteObject(hbwindow); DeleteDC(hwindowCompatibleDC); ReleaseDC(hwnd, hwindowDC);

	return src;
}

void Recorder::display()
{
	VideoCapture cap(_filename); // open the default camera
						 //Video Capture cap(path_to_video); // open the video file
	if (!cap.isOpened())  // check if we succeeded
		return;
	CvCapture *capture = cvCreateFileCapture(_filename.c_str());
	IplImage* frame;
	namedWindow("Video", 1);
	
	for (;;)
	{
		frame = cvQueryFrame(capture);
		if (!frame) { break; }
		cvShowImage("Video", frame);
		//Mat frame;
		//cap >> frame; // get a new frame from camera        
		//printf("%d\n", frame.size());
		//imshow("Video", frame);
		if (waitKey(1000.0/_framerate * 2.5) >= 0) break;
	}
	cvDestroyWindow("Video");
}


void Recorder::start()
{
	HWND desktop = GetDesktopWindow();
	Mat im = this->hwnd2mat(desktop);
	VideoWriter writer;
	int codec = CV_FOURCC('M', 'J', 'P', 'G');
	writer.open(_filename, codec, _framerate, im.size());
	while(true)
	{
		
		HWND desktop = GetDesktopWindow();
		im = this->hwnd2mat(desktop);
		imshow("frame", im);
		writer.write(im);
		char key = waitKey(1);
		if (key == 27)
		{
			break;
		}
	}
	writer.release();
}

Recorder::~Recorder()
{
}
