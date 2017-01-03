#pragma once
#include <string>
#include <opencv2\opencv.hpp>


class Recorder
{
public:
	Recorder(std::string name, int fps=25);
	void start();
	void display();

	~Recorder();

private:
	cv::Mat hwnd2mat(HWND hwnd);
	float _framerate;
	std::string _filename;
};

