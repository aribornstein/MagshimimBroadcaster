#include "stdafx.h"
#include "Recorder.h"


using namespace cv;
using namespace std;


/*
Recorder constructor.
Parameters: the name of the file and the fps and the time to record.
Return Value: None.
*/
Recorder::Recorder(std::string name, int fps, time_t timePerVid)
{
	_filename = name;
	_framerate = fps;
	_durationOfVid = timePerVid;
	HWND desktop = GetDesktopWindow();
	_size = Mat(this->hwnd2mat(desktop)).size();
	_codec = CV_FOURCC('M', 'S', 'V', 'C');
	//_codec = CV_FOURCC(-1, -1, -1, -1);
	DecideFramerate();
}

/*
Turns a window to an openCV image (matrix).
Parameters: The window.
Return Value: The image.
*/
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


/*
Display the video.
Parameters: None.
Return Value: None.
*/
void Recorder::display()
{
	VideoCapture cap(_filename); // open the default camera
						 //Video Capture cap(path_to_video); // open the video file
	if (!cap.isOpened())  // check if we succeeded
		return;
	CvCapture *capture = cvCreateFileCapture(_filename.c_str());
	IplImage* frame;
	namedWindow("Video", 1);
	
	for (;;)// liad & elad note : while (true) { } optimitation is same
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

void Recorder::DecideFramerate()
{
	ifstream f(_confName);
	if (!f)
	{
		int counter = 0;
		VideoWriter writer;
		writer.open("record1.avi", _codec, _framerate, _size);
		Mat m;
		time_t timer = time(NULL), timeOfCapture = time(NULL);
		while (timeOfCapture - timer < 3)
		{
			HWND desktop = GetDesktopWindow();
			m = this->hwnd2mat(desktop);
			writer.write(m);
			counter++;
			time(&timeOfCapture);
		}
		writer.release();
		_framerate = counter / 3;
		system("del record1.avi");
		ofstream file(_confName);
		file << "filename: " + _filename << endl << "framerate: " +  to_string(_framerate);
		file.close();
	}
	else
	{
		ifstream readFile(_confName);
		string val, s = " ";
		int counter = 0;
		while (!readFile.eof())
		{
			getline(readFile, val);
			val = val.substr(val.find(s));
			val.erase(remove(val.begin(), val.end(), ' '), val.end());
			if (counter == 0)
			{
				_filename = val;
			}
			else if (counter == 1)
			{
				_framerate = stoi(val);
			}
			counter++;
		}
		readFile.close();
	}
	f.close();
}

/*
Records the screen for a finite amount of time.
Parameters: None.
Return Value: None.
*/
void Recorder::start()
{
	VideoWriter writer;
	writer.open(_filename, _codec, _framerate, _size);
	Mat m;
	time_t timer = time(NULL), timeOfCapture = time(NULL);
	while (timeOfCapture - timer < _durationOfVid)
	{
		HWND desktop = GetDesktopWindow();
		m = this->hwnd2mat(desktop);
		//imshow("frame", m);
		writer.write(m);
		time(&timeOfCapture);
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
