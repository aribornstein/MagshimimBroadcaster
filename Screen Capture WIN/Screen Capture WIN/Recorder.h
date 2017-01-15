#pragma once
#include <string>
#include <opencv2\opencv.hpp>
#include <time.h>

class Recorder
{
public:
	Recorder(std::string name, int fps=25, time_t timePerVid=3);
	void start();
	void display();

	~Recorder();

private:
	cv::Mat hwnd2mat(HWND hwnd);
	float _framerate;
	time_t _durationOfVid;
	std::string _filename;
};

