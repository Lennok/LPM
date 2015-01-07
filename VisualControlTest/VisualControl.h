// MathFuncsAssembly.h
#ifndef _VISUAL_CONTROL_H_
#define _VISUAL_CONTROL_H_

#include <algorithm>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
//#include <opencv2/legacy/legacy.hpp>
//
#include <Windows.h>


#include "shape.h"
using namespace cv;

namespace VisualControlLib
{

	public class VisualControl
	{
	public:
		VisualControl(int CameraIndex);
		~VisualControl();

		float PlatformAngle() const { return mPlaformAngle; };
		float PlatformAltitude() const { return mPlatformAltitude; };
		float PlatformOffsetX() const { return mPlatformOffsetX; };
		float PlatformOffsetY() const { return mPlatformOffsetY; };
		float PlatformRelativeAltitude() const { return mPlatformRelativeAltitude; };
		bool DataValid() const { return mDataValid; };
		void setCalculationParams(double param1, double param2, double param3);
		void setVideoLogging(bool log, String logDirectory);

		int getMainLoopRunning();
		int setShowGrayImage(bool show);
		int setShowCannyImage(bool show);
		int setShowResultImage(bool show);
		int setShowSettings(bool show);
		int setShowImage(bool show);
		int setShowFigureCharasteristics(bool show);


		//main loop
		void doDetection();


	private:
		//members
		float mPlaformAngle;
		float mPlatformAltitude;
		float mPlatformOffsetX;
		float mPlatformOffsetY;
		float mPlatformRelativeAltitude;
		int mainLoopRunning;
		bool mDataValid;
		int mCameraIndex;
		std::string mImageFile;
		bool ShowGrayImage;
		bool ShowCannyImage;
		bool ShowResultImage;
		bool showSettings;
		bool showSingleImage;
		bool showFigureCharacteristics;
		bool showAllFigures;

		//calculation parameter
		double param1;
		double param2;
		double param3;
		bool mlogVideo;
		VideoWriter* outputVideo;
		String logDirectory;

		//defines
		string VisualControl::CANNY_WINDOW;
		string VisualControl::RESULT_WINDOW;
		string VisualControl::GRAY_WINDOW;
		string VisualControl::SETTINGS_WINDOW;
		string VisualControl::CIRCLE_CHARACTERISTIC_WINDOW;
		string VisualControl::SQUARE_CHARACTERISTIC_WINDOW;
		string VisualControl::HEXAGON_CHARACTERISTIC_WINDOW;
		string VisualControl::TRIANGLE_CHARACTERISTIC_WINDOW;


		CvCapture* capture;

		/// captures frames
		//Mat frame, gray_src, src, drawing;
		Mat  gray_src, src, drawing, working_frame, original_single_frame;

		/// Array with figures
		vector<Shape> shapes;
		vector<Shape> triangles;
		vector<Shape> squares;
		vector<Shape> hexagons;
		vector<Shape> circles;

		/// recognized figures
		Shape platformShape;
		Shape centerShape;
		Shape triangleShape;
		Shape squareShape;
		Shape hexagonShape;
		Shape circleShape;

		/// preprocessing options
		int thresh;
		int color_coeff;
		int min_eucl_dist;
		int center_detect_threshold;
		double center_detect_radius;
		int raduis_coef;
		int class_threshold;


		/// Variables to save files
		int counter;
		char filename[512];


		//Detect altitude
		double frameHeight;
		double frameWidth;


		//methods
		void Intensity(Mat inputArray, double* energy, double* entropy, double* contrast);
		void preprocessImage(Mat &frame);
		void processContours(Mat &frame);
		int processEdgeShapes();

		void processShapes();
		void drawShapes();

		void pushShape_1(vector<Shape> &items);
		void pushShape_2(vector<Shape> &items, Shape &center);

		///Platform detection methods
		bool simpleDetection();
		bool centerFirstDetection();


		float calculateAltitude();
		void calculateOffset();
		double calculateAngle(Shape &item);
		void calculatePlatformAngle();


	};
}
#endif
