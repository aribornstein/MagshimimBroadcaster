#pragma once
#include <string>
#include <opencv2\opencv.hpp>
#include <time.h>
#include <fstream>
#include <algorithm>

class Recorder
{
public:
	Recorder(std::string name, int fps=25, time_t timePerVid=3);
	void start();
	void display();

	~Recorder();

private:
	cv::Mat hwnd2mat(HWND hwnd);
	void Recorder::DecideFramerate();
	float _framerate;
	time_t _durationOfVid;
	std::string _filename;
	cv::Size _size;
	int _codec;
	std::string _confName = "config.txt";
};

