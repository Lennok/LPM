#define _USE_MATH_DEFINES

#include <math.h>

#include "VisualControl.h"

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

#include "iteration_return.h"


//#include <opencv2/legacy/legacy.hpp>
using namespace cv;
using namespace std;
using namespace VisualControlLib;;



/////////////////////////////////////////////////////////////
///	public functions 
/////////////////////////////////////////////////////////////

VisualControl::VisualControl(int CameraIndex)
{
	counter = 0;
	mainLoopRunning = 0;
	ShowCannyImage = false;
	ShowGrayImage = false;
	ShowResultImage = false;
	showSettings = false;
	showSingleImage = false;
	mPlaformAngle = 0;
	mPlatformAltitude = 0;
	mPlatformOffsetX = 0;
	mPlatformOffsetY = 0;
	mPlatformRelativeAltitude = 0;
	mlogVideo = true;
	outputVideo = NULL;

	_fpLogfile = NULL;

	mCameraIndex = CameraIndex;
	
	/// set preprocessing options
	thresh = 50;
	color_coeff = 70;
	min_eucl_dist = 50;
	center_detect_threshold = 100;
	center_detect_radius = 1.3;
	raduis_coef = 375;
	class_threshold = 80;

	//set cameraspec. params
	this->param1 = 0.0064;
	this->param2 = 38.112;
	this->param3 = 5;

	//set window names
	CANNY_WINDOW = "Canny_" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	RESULT_WINDOW = "Result" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	GRAY_WINDOW = "Gray" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	SETTINGS_WINDOW = "Settings" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	cvNamedWindow("OutputHelper", CV_WINDOW_AUTOSIZE);
	capture = NULL;
	showAllFigures = false;

}

VisualControl::~VisualControl()
{
	cvReleaseCapture(&capture);
	capture = NULL;
	destroyAllWindows();
}

int VisualControl::getMainLoopRunning()
{
	return mainLoopRunning;
}

int  VisualControl::setShowGrayImage(bool show)
{
	ShowGrayImage = show;
	if (show)
	{
		cvNamedWindow(GRAY_WINDOW.c_str(), CV_WINDOW_AUTOSIZE);
	}
	else
	{
		cvDestroyWindow(GRAY_WINDOW.c_str());
	}
	return 0;
}
int  VisualControl::setShowCannyImage(bool show)
{
	ShowCannyImage = show;
	if (show)
	{
		cvNamedWindow(CANNY_WINDOW.c_str(), CV_WINDOW_AUTOSIZE);
	}
	else
	{
		cvDestroyWindow(CANNY_WINDOW.c_str());
	}	
	return 0;
}
int  VisualControl::setShowResultImage(bool show)
{
	ShowResultImage = show;
	if (show)
	{
		cvNamedWindow(RESULT_WINDOW.c_str(), CV_WINDOW_AUTOSIZE);
	}
	else
	{
		cvDestroyWindow(RESULT_WINDOW.c_str());
	}	
	return 0;
}

int  VisualControl::setShowSettings(bool show)
{
	showSettings = show;
	if (show)
	{
		cvNamedWindow(SETTINGS_WINDOW.c_str(), CV_WINDOW_NORMAL);
		createTrackbar("THR:", SETTINGS_WINDOW, &thresh, 255);
		createTrackbar("CCOEF", SETTINGS_WINDOW, &color_coeff, 255);
		createTrackbar("MED", SETTINGS_WINDOW, &min_eucl_dist, 150);
		createTrackbar("CDT", SETTINGS_WINDOW, &center_detect_threshold, 150);
		createTrackbar("RCOEF", SETTINGS_WINDOW, &raduis_coef, 1000);
		createTrackbar("CLSTHR", SETTINGS_WINDOW, &class_threshold, 150);
	}
	else
	{
		cvDestroyWindow(SETTINGS_WINDOW.c_str());
	}	
	return 0;
}

int VisualControl::setShowImage(bool show)
{
	showSingleImage = show;
	if (show)
	{
		//open file dialog and get path to image
		wchar_t file[256] = {0};
		OPENFILENAME openFileDialog;
		ZeroMemory(&openFileDialog, sizeof(openFileDialog));
		openFileDialog.lStructSize = sizeof(openFileDialog);
		openFileDialog.hwndOwner = NULL;
		openFileDialog.lpstrFile = file;
		openFileDialog.nMaxFile = sizeof(file);
		openFileDialog.lpstrFilter = L"All\0*.*\0Image\0*.jpg\0";
		openFileDialog.nFilterIndex = -1;
		openFileDialog.lpstrFileTitle = NULL;
		openFileDialog.nMaxFileTitle = 0;
		openFileDialog.lpstrInitialDir = NULL;
		openFileDialog.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

		if(GetOpenFileName(&openFileDialog))
		{
			std:wstring wideString (openFileDialog.lpstrFile);
			mImageFile = std::string(wideString.begin(), wideString.end());
			cv::Mat frame = cv::imread(mImageFile);
			cv::resize(frame,original_single_frame, cv::Size(640, 480));
			//cv::resize(frame,original_single_frame, cv::Size(1280, 720));

			frameHeight = 480;
			frameWidth = 640;
			//GoPro
			/*	frameHeight = 720;
			frameWidth = 1280;*/
		}
		else
		{
			showSingleImage = false;
		}

	}
	return 0;
}


int VisualControl::setShowAllShapes(bool show)
{
	showAllFigures = show;
	if (show)
	{
		cvNamedWindow("All Shapes", CV_WINDOW_AUTOSIZE);
	}
	else
	{
		cvDestroyWindow("All Shapes");
	}	
	return 0;
}

int VisualControl::setWriteHtmlProtocol(bool write)
{
	writeHtmlProtocol = write;
	return 0;
}
//int VisualControl::startPlatformDetection()
//{
//	if (!(mainLoopRunning==1))
//	{
//
//
//		
//		mainLoopRunning = true;
//		mainLoopThread = new thread(&VisualControl::mainLoop, this);
//	}
//	return 0;
//
//}
//
//int VisualControl::stopPlatformDetection()
//{
//	if (mainLoopRunning == 1)
//	{
//		//end main loop
//		mainLoopRunning = false;
//		mainLoopThread->join();
//		// release resources
//		cvReleaseCapture(&capture);
//		delete mainLoopThread;
//
//	}
//	return 0;
//}

/// end public functions  //////////////////////////////////////////


///////////////////////////////////////////////////////////////////
///private functions 
////////////////////////////////////////////////////////////////////


void VisualControl::preprocessImage(Mat &frame) {

	Mat hsv, yuv;
	vector<Mat> channels_hsv;
	vector<Mat> channels_yuv;
	src = frame.clone();
	drawing = src.clone();
	helper = drawing.clone();
	

	///convert to grayscale
	cvtColor(frame, hsv, CV_RGB2HSV);
	cvtColor(frame, yuv, CV_RGB2YUV);
	split(hsv, channels_hsv);
	split(yuv, channels_yuv);
	Mat temp;
	addWeighted(channels_hsv[1], 0.5, channels_hsv[2], 0.5, 0, temp);
	multiply(temp, channels_yuv[0], frame, 1.0 / color_coeff);

	if (ShowGrayImage)
	{
		imshow(GRAY_WINDOW, frame);
	}	
	//equalizeHist(frame, frame);

	GaussianBlur(frame, frame, Size(5, 5), 0, 0);
	gray_src = frame.clone();

	
}

void VisualControl::processContours(Mat &frame)
{
	vector<vector<Point> > contours;
	vector<Vec4i> hierarchy;
	/// Detect edges using canny
	Canny(frame, frame, thresh, thresh * 2, 3);
	if (ShowCannyImage)
	{
		imshow(CANNY_WINDOW, frame);
	}
	
	/// Find contours	
	findContours(frame, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0));

	/// Clear the vector with forms
	shapes.clear();
	
	allFiguresFrame = src.clone();
	for (int i = 0; i< contours.size(); i++)
	{			
		int shapeType = SHAPE_NONE;
		shapeType = Shape::classifyShape(contours[i], (double)(class_threshold / 100.0));
						
		Shape temp(contours[i], shapeType);
		if (temp.shapeType != SHAPE_NONE)
		{
			shapes.push_back(temp);
		}			
	}
	if (showAllFigures || writeHtmlProtocol)
	{
		///Draw shapes
		vector<vector<Point> > resultContours;

		for (int i = 0; i<shapes.size(); ++i) {
			resultContours.push_back(shapes[i].shapeContour);	
			drawContours(allFiguresFrame, resultContours, i,  SHAPE_COLORS[shapes[i].shapeType], 1, 8, hierarchy, 0, Point());
		}
		putText(allFiguresFrame, "Gelb -> Quadrat", Point(1, 25), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(allFiguresFrame, "Rot -> Kreis", Point(1, 40), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(allFiguresFrame, "Gruen -> Dreieck", Point(1, 55), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(allFiguresFrame, "Blau -> Hexagon", Point(1, 70), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(allFiguresFrame, "Lila -> nach Iteration gefunden", Point(1, 85), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		
		if (showAllFigures)
		{
			imshow("All Shapes", allFiguresFrame);
		}
	}	
}

void VisualControl::processContours(Mat frame, iteration_not_detected newPictureFrame)
{
	//Crop Pic
	vector<vector<Point> > contours;
	vector<Vec4i> hierarchy;
	vector<Shape> cropShapes;

	int width = abs(newPictureFrame.end_x-newPictureFrame.start_x);
	int height = abs(newPictureFrame.end_y - newPictureFrame.start_y);

	if (width == 0 || height == 0)
	{
		return;
	}
	cv::Rect* rectangle = new Rect(newPictureFrame.start_x, newPictureFrame.start_y,width , height);
	Mat* imgCrop = new Mat(frame, *rectangle);

	vector<vector<Point> > resultContours;
	Canny(*imgCrop, *imgCrop, thresh, thresh * 2, 3);
	findContours(*imgCrop, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0));

	delete rectangle;
	delete imgCrop;

	for (int i = 0; i < contours.size(); i++)
	{
		int shapeType = SHAPE_NONE;
		std::map<std::string, double> features;
		//new Function
		Shape::calculateFeaturesForCropedFrame(contours[i], features);

		if (features["area"] > 100 && features["isClosed"] && features["eccentricity"] < 0.2)
		{
			if (features["triangularity"] > Shape::prototypesFeatures[0][2]/100.0 && features["triangularity"] < 1.0  && features["sides"] == 3)
			{
				shapeType = SHAPE_TRIANGLE;
			}
			else if (features["rectangularity"] > Shape::prototypesFeatures[1][1]/100.0 && (features["sides"] == 4 || features["sides"] == 5))
			{
				shapeType = SHAPE_SQUARE;
			}
			else if (features["roundness"] > Shape::prototypesFeatures[3][0]/100.0 && features["sides"] == 8)
			{
				shapeType = SHAPE_CIRCLE;
			}
			else if (features["sides"] == 6 || features["sides"] == 7)
			{
				shapeType = SHAPE_HEXAGON;
			}
		}
		
		if (shapeType == newPictureFrame.expected_type)
		{
			//fix Coordiantes to original image
			for (int j = 0; j < contours[i].size(); j++)
			{
				contours[i][j].x += newPictureFrame.start_x;
				contours[i][j].y += newPictureFrame.start_y;
			}

			Shape temp(contours[i], shapeType);
			shapes.push_back(temp);
			cropShapes.push_back(temp);
		}			
	}

	Shape temp;
	temp.shapeArea = 0;
	for (int i = 0; i < cropShapes.size(); i++)
	{
		if (cropShapes[i].shapeArea > temp.shapeArea)
		{
			temp = cropShapes[i];
		}
	}
	if (cropShapes.size() > 0)
	{
		switch (newPictureFrame.expected_type)	
			{
				case TypeRectangle:
					squareShape =  temp;
					break;
				case TypeTriangle:
					triangleShape = temp;
					break;
				case TypeCircle:
					circleShape = temp;
					break;
				case TypeHexagon:
					hexagonShape = temp;
					break;
				default:
					break;
			}
	}

	if (showAllFigures)
	{
		///Draw shapes
		vector<vector<Point> > resultContours;

		for (int i = 0; i<cropShapes.size(); ++i) {
			resultContours.push_back(cropShapes[i].shapeContour);	
			drawContours(allFiguresFrame, resultContours, i,  Scalar(255,0,200), 1, 8, hierarchy, 0, Point());
		}

		imshow("All Shapes", allFiguresFrame);
	}	
	

}

void  VisualControl::processContours(Mat frame, iteration_wrong_detected newPictureFrame)
{
	//Crop Pic
	vector<vector<Point> > contours;
	vector<Vec4i> hierarchy;
	vector<Shape> cropShapes;

	int width = abs(newPictureFrame.end_x-newPictureFrame.start_x);
	int height = abs(newPictureFrame.end_y - newPictureFrame.start_y);

	if (width == 0 || height == 0)
	{
		return;
	}
	cv::Rect* rectangle = new Rect(newPictureFrame.start_x, newPictureFrame.start_y,width , height);
	Mat* imgCrop = new Mat(frame, *rectangle);

	vector<vector<Point> > resultContours;
	Canny(*imgCrop, *imgCrop, thresh, thresh * 2, 3);
	findContours(*imgCrop, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0));

	delete rectangle;
	delete imgCrop;

	for (int i = 0; i < contours.size(); i++)
	{
		int shapeType = SHAPE_NONE;
		std::map<std::string, double> features;
		//new Function
		Shape::calculateFeaturesForCropedFrame(contours[i], features);

		if (features["area"] > 100 && features["isClosed"] && features["eccentricity"] < 0.2)
		{
			if (features["triangularity"] > Shape::prototypesFeatures[0][2]/100.0 && features["triangularity"] < 1.0  && features["sides"] == 3)
			{
				shapeType = SHAPE_TRIANGLE;
			}
			else if (features["rectangularity"] > Shape::prototypesFeatures[1][1]/100.0 && (features["sides"] == 4 || features["sides"] == 5))
			{
				shapeType = SHAPE_SQUARE;
			}
			else if (features["roundness"] > Shape::prototypesFeatures[3][0]/100.0 && features["sides"] == 8)
			{
				shapeType = SHAPE_CIRCLE;
			}
			else if (features["sides"] == 6 || features["sides"] == 7)
			{
				shapeType = SHAPE_HEXAGON;
			}
		}
		
		if (shapeType == newPictureFrame.expected_type)
		{
			//fix Coordiantes to original image
			for (int j = 0; j < contours[i].size(); j++)
			{
				contours[i][j].x += newPictureFrame.start_x;
				contours[i][j].y += newPictureFrame.start_y;
			}

			Shape temp(contours[i], shapeType);
			shapes.push_back(temp);
			cropShapes.push_back(temp);
		}			
	}

	Shape temp;
	temp.shapeArea = 0;
	for (int i = 0; i < cropShapes.size(); i++)
	{
		if (cropShapes[i].shapeArea > temp.shapeArea)
		{
			temp = cropShapes[i];
		}
	}
	if (cropShapes.size() > 0)
	{
		switch (newPictureFrame.expected_type)	
			{
				case TypeRectangle:
					squareShape =  temp;
					break;
				case TypeTriangle:
					triangleShape = temp;
					break;
				case TypeCircle:
					circleShape = temp;
					break;
				case TypeHexagon:
					hexagonShape = temp;
					break;
				default:
					break;
			}
	}

	if (showAllFigures)
	{
		///Draw shapes
		vector<vector<Point> > resultContours;

		for (int i = 0; i<cropShapes.size(); ++i) {
			resultContours.push_back(cropShapes[i].shapeContour);	
			drawContours(allFiguresFrame, resultContours, i,  Scalar(255,0,200), 1, 8, hierarchy, 0, Point());
		}

		imshow("All Shapes", allFiguresFrame);
	}	
}

void VisualControl::processShapes() {
	/// Clear formes arraysS
	triangles.clear();
	squares.clear();
	hexagons.clear();
	circles.clear();

	/// Put shapes into formes arrays
	for (int i = 0; i < shapes.size(); ++i) 
	{
		switch (shapes[i].shapeType) {
		case SHAPE_TRIANGLE:
			triangles.push_back(shapes[i]);
			break;
		case SHAPE_SQUARE:
			squares.push_back(shapes[i]);
			break;
		case SHAPE_HEXAGON:
			hexagons.push_back(shapes[i]);
			break;
		case SHAPE_CIRCLE:
			circles.push_back(shapes[i]);
			break;
		default:
			break;
		}
	}

	/// Clear shapes base arrays
	shapes.clear();

	/// Detect platform with simple method
	if (simpleDetection()) {
		//qDebug() << "Simple method";
	}
	else if (centerFirstDetection()) {
		//qDebug() << "Center first method";
	}
	else {
		//qDebug() << "No platform";
	}

	calculateAltitude();

	//processEdgeShapes();
	if (0 == processEdgeShapes())
	{
		calculateOffset();
		calculatePlatformAngle();
		mDataValid = true;
	}
	else
	{
		mDataValid = false;
	}
}

void VisualControl::drawShapes() {
	shapes.clear();
	if (centerShape.shapeArea > 0) {
		shapes.push_back(centerShape);

		if (triangleShape.shapeArea > 0)
			shapes.push_back(triangleShape);

		if (squareShape.shapeArea > 0)
			shapes.push_back(squareShape);

		if (hexagonShape.shapeArea > 0)
			shapes.push_back(hexagonShape);

		if (circleShape.shapeArea > 0)
			shapes.push_back(circleShape);

		if (platformShape.shapeArea > 0)
			shapes.push_back(platformShape);
	}

	///Draw shapes
	vector<vector<Point> > resultContours;



	for (int i = 0; i<shapes.size(); ++i) 
	{
		resultContours.push_back(shapes[i].shapeContour);

		/// Draw contour
		drawContours(drawing, resultContours, i, SHAPE_COLORS[shapes[i].shapeType], 2, 8, NULL, 0, Point());

		/// Draw center of shape
		Point2f center;
		float radius;
		minEnclosingCircle(resultContours[i], center, radius);
		circle(drawing, center, 3, SHAPE_COLORS[shapes[i].shapeType], 2);
	}

	if (centerShape.shapeArea > 0) {
		stringstream ss;

		putText(drawing, "Center of platform", Point(1, 10), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);

		ss << "x = " << (int)centerShape.shapeCenter.x;
		putText(drawing, ss.str(), Point(1, 25), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);

		ss.clear();
		ss << "y = " << (int)centerShape.shapeCenter.y;
		putText(drawing, ss.str(), Point(1, 40), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
	}

	if (ShowResultImage)
	{
		imshow(RESULT_WINDOW, drawing);
	}
}

void VisualControl::pushShape_1(vector<Shape> &items) {
	for(int i = 0; i< items.size(); i++)
	{
		double eucliadianDistance = 0;
		bool centerIsInside = platformShape.centerIsInside(items[i].shapeContour, eucliadianDistance);
		double area_ratio = centerShape.shapeArea / items[i].shapeArea;

		if (centerIsInside && eucliadianDistance > min_eucl_dist && area_ratio < 2 && area_ratio > 0.5) 
		{
			shapes.push_back(items[i]);
			break;
		}
	}
}

bool VisualControl::simpleDetection() 
{
	centerShape = Shape();
	platformShape = Shape();
	triangleShape = Shape();
	squareShape = Shape();
	hexagonShape = Shape();
	circleShape = Shape();

	for (int circle_item = 0; circle_item < circles.size(); ++circle_item) 
	{
		for (int square_item = 0; square_item < squares.size(); ++square_item) 
		{
			double eucliadianDistance = 0;
			bool centerIsInside = circles[circle_item].centerIsInside(squares[square_item].shapeContour, eucliadianDistance);
			double area_ratio = squares[square_item].shapeArea / circles[circle_item].shapeArea;

			if (centerIsInside && eucliadianDistance < min_eucl_dist && area_ratio > 2.5) 
			{
				if (squares[square_item].shapeArea > platformShape.shapeArea && circles[circle_item].shapeArea > centerShape.shapeArea) 
				{
					platformShape = squares[square_item];
					platformShape.shapeType = SHAPE_PLATFORM;

					centerShape = circles[circle_item];
					centerShape.shapeType = SHAPE_CENTER;
				}
			}
		}
	}

	//Adding center and found a platform in an array of figures
	if (platformShape.shapeArea > 0 && centerShape.shapeArea > 0) 
	{
		shapes.push_back(platformShape);
		shapes.push_back(centerShape);

		pushShape_1(triangles);
		pushShape_1(squares);
		pushShape_1(hexagons);
		pushShape_1(circles);

		return true;
	}
	else 
	{
		return false;
	}
}

void VisualControl::pushShape_2(vector<Shape> &items, Shape &center) {
	for(int i = 0; i< items.size(); i++)
	{
		double eucliadianDistance = 0;

		Point diff = items[i].shapeCenter - center.shapeCenter;
		eucliadianDistance = sqrt((double)(diff.x*diff.x + diff.y*diff.y));

		if (eucliadianDistance <= center.shapeRadius*raduis_coef / 100.0 && eucliadianDistance > 10) {
			shapes.push_back(items[i]);
		}
	}
}

bool VisualControl::centerFirstDetection() {
	centerShape = Shape();
	platformShape = Shape();

	int centerCrossingCount = 32000;
	for(int i = 0; i< circles.size(); i++)
	{
		int count = Shape::detectCentralShape(gray_src, circles[i].shapeCenter, circles[i].shapeRadius*center_detect_radius, center_detect_threshold);

		if (count < centerCrossingCount /*&& item.shapeArea > centerShape.shapeArea*/)
		{
			centerShape = circles[i];
			centerCrossingCount = count;
		}
	}

	if (centerShape.shapeArea > 0) {
		centerShape.shapeType = SHAPE_CENTER;
		shapes.push_back(centerShape);

		double radius = centerShape.shapeRadius*raduis_coef / 100.0;
		circle(src, centerShape.shapeCenter, radius, cv::Scalar(96, 115, 27), 1);

		pushShape_2(triangles, centerShape);
		pushShape_2(squares, centerShape);
		pushShape_2(hexagons, centerShape);
		pushShape_2(circles, centerShape);

		return true;
	}
	else {
		return false;
	}
}

int VisualControl::processEdgeShapes() 
{

	double eucliadianDistance = 0;
	bool is_inside = false;
	bool one_shape_found = false;
	
	if (shapes.size() == 0)
	{
		return -1;
	}

	for(int i = 0; i< shapes.size(); i++)
	{
		switch (shapes[i].shapeType) 
		{
			case SHAPE_TRIANGLE:

				if (circleShape.shapeArea > 0) {
					is_inside = circleShape.centerIsInside(shapes[i].shapeContour, eucliadianDistance);
				}
				else 
				{
					is_inside = false;
				}
				if (shapes[i].shapeArea > triangleShape.shapeArea && !is_inside) 
				{
					triangleShape = shapes[i];
				}
				
				one_shape_found = true;
				break;

			case SHAPE_SQUARE:
				if (shapes[i].shapeArea > squareShape.shapeArea) 
				{
					squareShape = shapes[i];
				}
				
				one_shape_found = true;
				break;

			case SHAPE_HEXAGON:
				if (shapes[i].shapeArea > hexagonShape.shapeArea) 
				{
					hexagonShape = shapes[i];
				}
				
				one_shape_found = true;
				break;

			case SHAPE_CIRCLE:
				if (hexagonShape.shapeArea > 0) 
				{
					is_inside = hexagonShape.centerIsInside(shapes[i].shapeContour, eucliadianDistance);
				}
				else 
				{
					is_inside = false;
				}

				if (shapes[i].shapeArea > circleShape.shapeArea && !is_inside)
				{
					circleShape = shapes[i];
				}
				
				one_shape_found = true;
				break;
		}
	}

	if (one_shape_found)
	{
		return -1;
	}
	
	if (one_shape_found)
	{
		return 0;
	}
	else
	{
		return -1;
	}
}

float VisualControl::calculateAltitude() {
	if (centerShape.shapeArea > 0 || platformShape.shapeArea > 0) {
		double x, distance;
		double shapeFactor;
		double frameFactor = sqrt(frameHeight*frameWidth);
		
		if(platformShape.shapeArea > 0) {
		shapeFactor = sqrt(platformShape.shapeArea);
		x = frameFactor / shapeFactor * 4.0;
		//double distance = 0.0064*x*x + 38.112*x + 5;
		distance = param1*x*x + param2*x + param3;
		//double distance = x;
		}

		else {
		shapeFactor = sqrt(centerShape.shapeArea);
		x = frameFactor / shapeFactor;
		//double distance = 0.0064*x*x + 38.112*x + 5;
		distance = param1*x*x + param2*x + param3;
		//double distance = x;
		}

		stringstream dist,rel_dist;
		dist << "Distance = " << distance;

		//QString text = QString("Distance = %1").arg(distance);
		//putText(drawing, text.toStdString(), Point(1, 55), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(drawing, dist.str(), Point(1, 55), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);

		rel_dist << "Relative distance = " << x;
		//text = QString("Relative distance = %1").arg(x);
		//putText(drawing, text.toStdString(), Point(1, 70), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(drawing, rel_dist.str(), Point(1, 70), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);

		mPlatformAltitude = distance;
		mPlatformRelativeAltitude = x;
	}

	return 0;
}

void VisualControl::calculateOffset() {
	if (centerShape.shapeArea > 0) {
		double x = (1 - (2 * centerShape.shapeCenter.x / frameWidth)) * 100;
		double y = (1 - (2 * centerShape.shapeCenter.y / frameHeight)) * 100;
		stringstream ss;
#
		ss << "X offset =" << (int)x << " Y offset = " << (int)y;
		//QString text = QString("X offset = %1% Y offset = %2%").arg(QString::number((int)x), QString::number((int)y));
		//putText(drawing, text.toStdString(), Point(1, 85), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
		putText(drawing, ss.str(), Point(1, 85), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);

		mPlatformOffsetX = x;
		mPlatformOffsetY = y;
	}
}

double VisualControl::calculateAngle(Shape &item) {
	double x = item.shapeCenter.x - centerShape.shapeCenter.x;
	double y = item.shapeCenter.y - centerShape.shapeCenter.y;
	double angle = 0;


	if (x == 0) {
		if (y < 0) {
			angle = 180;
		}
		else {
			angle = 0;
		}
	}
	else {
		angle = atan(y / x)*180.0 / PI;
		if (x > 0) {
			angle += 90;
		}
		else {
			angle += 270;
		}

	}
	return angle;
}

void VisualControl::calculatePlatformAngle() {
	if (centerShape.shapeArea > 0) {
		vector<int> angles;
		int angle;

		if (triangleShape.shapeArea > 0) {
			angle = (int)(calculateAngle(triangleShape) + (360 - 219)) % 360;
			angles.push_back(angle);
		}

		if (squareShape.shapeArea > 0) {
			angle = (int)(calculateAngle(squareShape) + (360 - 45)) % 360;
			angles.push_back(angle);
		}

		if (hexagonShape.shapeArea > 0) {
			angle = (int)(calculateAngle(hexagonShape) + (360 - 315)) % 360;
			angles.push_back(angle);
		}

		if (circleShape.shapeArea > 0) {
			angle = (int)(calculateAngle(circleShape) + (360 - 135)) % 360;
			angles.push_back(angle);
		}

		if (angles.size() > 0) {

			int average = 0;

			int min = *min_element(angles.begin(), angles.end());
			int max = *max_element(angles.begin(), angles.end());
			if ((max - min) > 300) {
				for(int i = 0; i< angles.size(); i++)
				{
					if ((angles[i] + 300) < max) {
						average += angles[i] + 360;
					}
					else {
						average += angles[i];
					}
				}
				average /= angles.size();
			}
			else {
				//TODO WTF !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				//average = accumulate(angles, angles, 0) / angles.size();
				int test = 0;
				for (std::vector<int>::iterator it =angles.begin(); it != angles.end(); ++it)
				{
					test += *it;
				}
				average = test / angles.size();
			}

			if (average > 180) {
				average = -1 * (360 - average);
			}

			stringstream ss;
			ss << "Platform angle = " << average;
			//QString text = QString("Platform angle = %1").arg(QString::number(average));
			//putText(drawing, text.toStdString(), Point(1, 100), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
			putText(drawing, ss.str(), Point(1, 100), FONT_HERSHEY_PLAIN, 1, Scalar(28, 232, 0), 1, 8);
			mPlaformAngle = average;
		}

	}
}

bool VisualControl::in_picture_y(float coord)
{
	if (coord >= frameHeight)
	{
		return false;
	}

	if (coord < 0.0f)
	{
		return false;
	}

	return true;
}

bool VisualControl::in_picture_x(float coord)
{
	if (coord >= frameWidth)
	{
		return false;
	}

	if (coord < 0.0f)
	{
		return false;
	}

	return true;
}

vector<Point> VisualControl::generate_line(float radangle, Point start)
{
	vector<Point> _line;

	float xKoord = (float)start.x;
	float yKoord = (float)start.y;

	while (in_picture_x(xKoord) && in_picture_y(yKoord))
	{
		Point p = Point(xKoord, yKoord);
		_line.push_back(p);

		xKoord += 1.0f;
		yKoord += radangle;
	}


	line(helper, start, _line[_line.size() - 1], Scalar(0, 0, 0));

	xKoord = start.x;
	yKoord = start.y;

	while (in_picture_x(xKoord) && in_picture_y(yKoord))
	{
		Point p = Point(xKoord, yKoord);
		_line.push_back(p);

		xKoord -= 1.0f;
		yKoord -= radangle;
	}

	line(helper, start, _line[_line.size() - 1] , Scalar(0, 0, 0));

	imshow("OutputHelper", helper);

	return _line;
}

Point  VisualControl::find_intersection_point(vector<Point> line1, vector<Point> line2)
{
	Point retVal (-1, -1);
	float euclidean_distance = 100;

	for (int i = 0; i < line1.size(); i++)
	{
		for (int j = 0; j < line2.size(); j++)
		{
			int deltaX = line1[i].x - line2[j].x;
			int deltaY = line1[i].y - line2[j].y;

			float local_distance = sqrt((float)(deltaX * deltaX + deltaY * deltaY));
			if (local_distance < euclidean_distance)
			{
				retVal.x = line1[i].x;
				retVal.y = line1[i].y;

				euclidean_distance = local_distance;
			}
		}
	}

	return retVal;
}

bool VisualControl::check_missing(int shape, bool cross, bool clockwise, bool anticlockwise, int& x1, int& y1, int& x2, int& y2)
{
	if (!cross && !clockwise && !anticlockwise)
	{
		return false;
	}

	calculatePlatformAngle();

	switch (shape)
	{
		case SHAPE_TRIANGLE:

			if (cross)
			{
				float distance_square_center_x = squareShape.shapeCenter.x - centerShape.shapeCenter.x;
				float distance_square_center_y = squareShape.shapeCenter.y - centerShape.shapeCenter.y;

				float triangle_center_x = centerShape.shapeCenter.x - distance_square_center_x;
				float triangle_center_y = centerShape.shapeCenter.y - distance_square_center_y;


				float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.25f;
				float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.25f;

				float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.25f;
				float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.25f;

				if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
					!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
				{
					return false;
				}

				x1 = triangle_center_region_x1;
				x2 = triangle_center_region_x2;

				y1 = triangle_center_region_y1;
				y2 = triangle_center_region_y2;
				break;
			}


			else if (clockwise)
			{
				float radAngle = tanf ((mPlaformAngle + 90) * M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle) *  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, hexagonShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);

				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = hexagonShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = hexagonShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x + 
						center_intersection_distance_x + background_intersection_distance_x;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.3f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.3f;

					

					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}

			}

			else if (anticlockwise)
			{
				float radAngle = tanf((mPlaformAngle) * M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle + 90)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, circleShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);
				

				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = circleShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = circleShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.3f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.3f;



					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}
			}
			else
			{
				return false;
			}

			break;

		case SHAPE_HEXAGON:
			
			if (cross)
			{
				float distance_circle_center_x = circleShape.shapeCenter.x - centerShape.shapeCenter.x;
				float distance_circle_center_y = circleShape.shapeCenter.y - centerShape.shapeCenter.y;

				float hexagon_center_x = centerShape.shapeCenter.x - distance_circle_center_x;
				float hexagon_center_y = centerShape.shapeCenter.y - distance_circle_center_y;


				float hexagon_center_region_x1 = hexagon_center_x - centerShape.shapeRadius * 1.35f;
				float hexagon_center_region_x2 = hexagon_center_x + centerShape.shapeRadius * 1.35f;

				float hexagon_center_region_y1 = hexagon_center_y - centerShape.shapeRadius * 1.35f;
				float hexagon_center_region_y2 = hexagon_center_y + centerShape.shapeRadius * 1.35f;

				if (!in_picture_x(hexagon_center_region_x1) || !in_picture_x(hexagon_center_region_x2) ||
					!in_picture_y(hexagon_center_region_y1) || !in_picture_y(hexagon_center_region_y2))
				{
					return false;
				}

				x1 = hexagon_center_region_x1;
				x2 = hexagon_center_region_x2;

				y1 = hexagon_center_region_y1;
				y2 = hexagon_center_region_y2;
			}

			else if (clockwise)
			{
				float radAngle = tanf((mPlaformAngle + 90) * M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, squareShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);

				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = squareShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = squareShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.3f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.3f;



					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}

			}

			else if (anticlockwise)
			{
				float radAngle = tanf((mPlaformAngle)* M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle + 90)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, triangleShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);


				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = triangleShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = triangleShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x + 2.0f;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y - 2.0f;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.35f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.35f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.35f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.35f;



					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}
			}
			else
			{
				return false;
			}

			break;

		case SHAPE_SQUARE:
			if (cross)
			{
				float distance_square_center_x = triangleShape.shapeCenter.x - centerShape.shapeCenter.x;
				float distance_square_center_y = triangleShape.shapeCenter.y - centerShape.shapeCenter.y;

				float hexagon_center_x = centerShape.shapeCenter.x - distance_square_center_x;
				float hexagon_center_y = centerShape.shapeCenter.y - distance_square_center_y;


				float hexagon_center_region_x1 = hexagon_center_x - centerShape.shapeRadius * 1.35f;
				float hexagon_center_region_x2 = hexagon_center_x + centerShape.shapeRadius * 1.35f;

				float hexagon_center_region_y1 = hexagon_center_y - centerShape.shapeRadius * 1.35f;
				float hexagon_center_region_y2 = hexagon_center_y + centerShape.shapeRadius * 1.35f;

				if (!in_picture_x(hexagon_center_region_x1) || !in_picture_x(hexagon_center_region_x2) ||
					!in_picture_y(hexagon_center_region_y1) || !in_picture_y(hexagon_center_region_y2))
				{
					return false;
				}

				x1 = hexagon_center_region_x1;
				x2 = hexagon_center_region_x2;

				y1 = hexagon_center_region_y1;
				y2 = hexagon_center_region_y2;
			}

			else if (clockwise)
			{
				float radAngle = tanf((mPlaformAngle + 90) * M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, circleShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);

				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = circleShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = circleShape.shapeCenter.y - result.y;

					float square_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x;


					float square_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float square_center_region_x1 = square_center_x - centerShape.shapeRadius * 1.25f;
					float square_center_region_x2 = square_center_x + centerShape.shapeRadius * 1.25f;

					float square_center_region_y1 = square_center_y - centerShape.shapeRadius * 1.25f;
					float square_center_region_y2 = square_center_y + centerShape.shapeRadius * 1.25f;



					if (!in_picture_x(square_center_region_x1) || !in_picture_x(square_center_region_x2) ||
						!in_picture_y(square_center_region_y1) || !in_picture_y(square_center_region_y2))
					{
						return false;
					}

					x1 = square_center_region_x1;
					x2 = square_center_region_x2;

					y1 = square_center_region_y1;
					y2 = square_center_region_y2;

					break;

				}

			}

			else if (anticlockwise)
			{
				float radAngle = tanf((mPlaformAngle)* M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle + 90)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, hexagonShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);


				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = hexagonShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = hexagonShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.3f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.3f;



					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}
			}
			else
			{
				return false;
			}

			break;

		case SHAPE_CIRCLE:
			if (cross)
			{
				float distance_hexagon_center_x = hexagonShape.shapeCenter.x - centerShape.shapeCenter.x;
				float distance_hexagon_center_y = hexagonShape.shapeCenter.y - centerShape.shapeCenter.y;

				float circle_center_x = centerShape.shapeCenter.x - distance_hexagon_center_x;
				float circle_center_y = centerShape.shapeCenter.y - distance_hexagon_center_y;


				float circle_center_region_x1 = circle_center_x - centerShape.shapeRadius * 1.25f;
				float circle_center_region_x2 = circle_center_x + centerShape.shapeRadius * 1.25f;

				float circle_center_region_y1 = circle_center_y - centerShape.shapeRadius * 1.25f;
				float circle_center_region_y2 = circle_center_y + centerShape.shapeRadius * 1.25f;

				if (!in_picture_x(circle_center_region_x1) || !in_picture_x(circle_center_region_x2) ||
					!in_picture_y(circle_center_region_y1) || !in_picture_y(circle_center_region_y2))
				{
					return false;
				}

				x1 = circle_center_region_x1;
				x2 = circle_center_region_x2;

				y1 = circle_center_region_y1;
				y2 = circle_center_region_y2;
			}

			else if (clockwise)
			{
				float radAngle = tanf((mPlaformAngle + 90) * M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, triangleShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);

				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = triangleShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = triangleShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.3f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.3f;



					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}

			}

			else if (anticlockwise)
			{
				float radAngle = tanf((mPlaformAngle)* M_PI / 180);
				float radAngleClockwise = tanf((mPlaformAngle + 90)*  M_PI / 180);

				// generate vector pixelwise
				vector<Point> line1 = generate_line(radAngle, centerShape.shapeCenter);
				vector<Point> line2 = generate_line(radAngleClockwise, squareShape.shapeCenter);

				Point result = find_intersection_point(line1, line2);


				if (result.x != -1)
				{
					circle(helper, result, 2, Scalar(0, 255, 0), 2);
					imshow("OutputHelper", helper);

					float center_intersection_distance_x = centerShape.shapeCenter.x - result.x;
					float center_intersection_distance_y = centerShape.shapeCenter.y - result.y;

					float background_intersection_distance_x = squareShape.shapeCenter.x - result.x;
					float background_intersection_distance_y = squareShape.shapeCenter.y - result.y;

					float triangle_center_x = centerShape.shapeCenter.x +
						center_intersection_distance_x + background_intersection_distance_x;


					float triangle_center_y = centerShape.shapeCenter.y +
						center_intersection_distance_y + background_intersection_distance_y;

					float triangle_center_region_x1 = triangle_center_x - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_x2 = triangle_center_x + centerShape.shapeRadius * 1.3f;

					float triangle_center_region_y1 = triangle_center_y - centerShape.shapeRadius * 1.3f;
					float triangle_center_region_y2 = triangle_center_y + centerShape.shapeRadius * 1.3f;



					if (!in_picture_x(triangle_center_region_x1) || !in_picture_x(triangle_center_region_x2) ||
						!in_picture_y(triangle_center_region_y1) || !in_picture_y(triangle_center_region_y2))
					{
						return false;
					}

					x1 = triangle_center_region_x1;
					x2 = triangle_center_region_x2;

					y1 = triangle_center_region_y1;
					y2 = triangle_center_region_y2;

					break;

				}
			}
			else
			{
				return false;
			}

			break;
	}
	

	return true;
}

float VisualControl::calc_euclidean_distance(Shape s)
{
	if (s.shapeCenter.x == 0 || s.shapeCenter.y == 0)
	{
		return -1.0f;
	}
	float x_distance = abs(s.shapeCenter.x - centerShape.shapeCenter.x);
	float y_distance = abs(s.shapeCenter.y - centerShape.shapeCenter.y);

	float euclideanDistance =
		sqrt(float((x_distance * x_distance) + (y_distance * y_distance)));

	return euclideanDistance;
}

bool VisualControl::check_wrong_detected(int shape, int& expectedShape, int& x1, int& y1, int& x2, int& y2)
{

	float triangle_distance = calc_euclidean_distance(triangleShape);

	float square_distance = calc_euclidean_distance(squareShape);
	float circle_distance = calc_euclidean_distance(circleShape);
	float hexagon_distance = calc_euclidean_distance(hexagonShape);


	float array_distances[4] = { triangle_distance, square_distance , hexagon_distance, circle_distance };
	int start = 0;

	if (circle_distance != -1.0f)
	{
		start = 4;
	}

	else if (hexagon_distance != -1.0f)
	{
		start = 4;
	}

	else if (square_distance != -1.0f)
	{
		start = 2;
	}

	else if (triangle_distance != -1.0f)
	{
		start = 1;
	}
	else
	{
		return false;
	}

	for (int i = start; i < 4; i++)
	{
		if (array_distances[start - 1]  > array_distances[i] * 1.2f)
		{
			expectedShape = i + 1;

			switch (expectedShape)
			{
				case SHAPE_CIRCLE:
					x1 = circleShape.shapeCenter.x - centerShape.shapeRadius * 1.3f;
					x2 = circleShape.shapeCenter.x + centerShape.shapeRadius * 1.3f;


					y1 = circleShape.shapeCenter.y - centerShape.shapeRadius * 1.3f;
					y2 = circleShape.shapeCenter.y + centerShape.shapeRadius * 1.3f;

					break;

				case SHAPE_SQUARE:
					x1 = squareShape.shapeCenter.x - centerShape.shapeRadius * 1.3f;
					x2 = squareShape.shapeCenter.x + centerShape.shapeRadius * 1.3f;


					y1 = squareShape.shapeCenter.y - centerShape.shapeRadius * 1.3f;
					y2 = squareShape.shapeCenter.y + centerShape.shapeRadius * 1.3f;
					break;

				case SHAPE_HEXAGON:
					x1 = hexagonShape.shapeCenter.x - centerShape.shapeRadius * 1.3f;
					x2 = hexagonShape.shapeCenter.x + centerShape.shapeRadius * 1.3f;


					y1 = hexagonShape.shapeCenter.y - centerShape.shapeRadius * 1.3f;
					y2 = hexagonShape.shapeCenter.y + centerShape.shapeRadius * 1.3f;
					break;

				case SHAPE_TRIANGLE:
					x1 = triangleShape.shapeCenter.x - centerShape.shapeRadius * 1.3f;
					x2 = triangleShape.shapeCenter.x + centerShape.shapeRadius * 1.3f;


					y1 = triangleShape.shapeCenter.y - centerShape.shapeRadius * 1.3f;
					y2 = triangleShape.shapeCenter.y + centerShape.shapeRadius * 1.3f;
					break;

			}

		}
	}

	

	return true;
}

iteration_return_t * VisualControl::iterate_process_edge_shapes()
{
	double eucliadianDistance = 0;
	bool is_inside = false;
	bool one_shape_found = false;

	bool found[4] = { false, false, false, false };
	iteration_return_t * retVal = new iteration_return_t();

	retVal->state = Success;

	if (shapes.size() == 0)
	{
		retVal->state = NoEdgeShapesDetected;
		return retVal;
	}

	for (int i = 0; i< shapes.size(); i++)
	{
		switch (shapes[i].shapeType)
		{
		case SHAPE_TRIANGLE:

			if (circleShape.shapeArea > 0) {
				is_inside = circleShape.centerIsInside(shapes[i].shapeContour, eucliadianDistance);
			}
			else
			{
				is_inside = false;
			}
			if (shapes[i].shapeArea > triangleShape.shapeArea && !is_inside)
			{
				triangleShape = shapes[i];
			}

			found[SHAPE_TRIANGLE - 1] = true;

			one_shape_found = true;
			break;

		case SHAPE_SQUARE:
			if (shapes[i].shapeArea > squareShape.shapeArea)
			{
				squareShape = shapes[i];
			}

			found[SHAPE_SQUARE - 1] = true;

			one_shape_found = true;
			break;

		case SHAPE_HEXAGON:
			if (shapes[i].shapeArea > hexagonShape.shapeArea)
			{
				hexagonShape = shapes[i];
			}

			found[SHAPE_HEXAGON - 1] = true;

			one_shape_found = true;
			break;

		case SHAPE_CIRCLE:
			if (hexagonShape.shapeArea > 0)
			{
				is_inside = hexagonShape.centerIsInside(shapes[i].shapeContour, eucliadianDistance);
			}
			else
			{
				is_inside = false;
			}

			if (shapes[i].shapeArea > circleShape.shapeArea && !is_inside)
			{
				circleShape = shapes[i];
				found[SHAPE_CIRCLE - 1] = true;

				one_shape_found = true;
			}

			break;
		}
	}

	if (!one_shape_found)
	{
		retVal->state = NoEdgeShapesDetected;
		return retVal;
	}
	
	int x1, x2, y1, y2;
	int expected_shape;

	

	for (int i = 1; i < 5; i++)
	{
		x1 = 0; 
		x2 = 0;
		y1 = 0; 
		y2 = 0;
		if (!found[i - 1])
		{
			switch (i)
			{
				case SHAPE_TRIANGLE:
				{
					iteration_not_detected triangular;
					triangular.expected_type = TypeTriangle;

					bool bRetVal = check_missing(SHAPE_TRIANGLE, found[Shape::get_cross_neighbour(SHAPE_TRIANGLE) - 1],
						found[Shape::get_clockwise_neighbour(SHAPE_TRIANGLE) - 1], found[Shape::get_anticlockwise_neighbour(SHAPE_TRIANGLE) - 1],
						x1, y1, x2, y2);

					if (bRetVal)
					{
						triangular.start_x = x1;
						triangular.start_y = y1;

						triangular.end_x = x2;
						triangular.end_y = y2;

						retVal->nr_of_no_detections++;
						retVal->vector_not_detected.push_back(triangular);
						retVal->state = (iteration_state)(retVal->state | BackgroundFigureMissing);
					}


				}
				break;


				case SHAPE_SQUARE:
				{
					iteration_not_detected square;
					square.expected_type = TypeRectangle;

					bool bRetVal = check_missing(SHAPE_SQUARE, found[Shape::get_cross_neighbour(SHAPE_SQUARE) - 1],
						found[Shape::get_clockwise_neighbour(SHAPE_SQUARE) - 1], found[Shape::get_anticlockwise_neighbour(SHAPE_SQUARE) - 1],
						x1, y1, x2, y2);

					if (bRetVal)
					{
						square.start_x = x1;
						square.start_y = y1;

						square.end_x = x2;
						square.end_y = y2;

						retVal->nr_of_no_detections++;
						retVal->vector_not_detected.push_back(square);
						retVal->state = (iteration_state)(retVal->state | BackgroundFigureMissing);
					}
				}
				break;
				
				case SHAPE_HEXAGON:
				{
					iteration_not_detected hexagon;
					hexagon.expected_type = TypeHexagon;

					bool bRetVal = check_missing(SHAPE_HEXAGON, found[Shape::get_cross_neighbour(SHAPE_HEXAGON) - 1],
						found[Shape::get_clockwise_neighbour(SHAPE_HEXAGON) - 1], found[Shape::get_anticlockwise_neighbour(SHAPE_HEXAGON) - 1],
						x1, y1, x2, y2);

					if (bRetVal)
					{
						hexagon.start_x = x1;
						hexagon.start_y = y1;

						hexagon.end_x = x2;
						hexagon.end_y = y2;

						retVal->nr_of_no_detections++;
						retVal->vector_not_detected.push_back(hexagon);
						retVal->state = (iteration_state)(retVal->state | BackgroundFigureMissing);
					}
				}
				break;
				
				case SHAPE_CIRCLE:
				{
					iteration_not_detected circle;
					circle.expected_type = TypeCircle;

					bool bRetVal = check_missing(SHAPE_CIRCLE, found[Shape::get_cross_neighbour(SHAPE_CIRCLE) - 1],
						found[Shape::get_clockwise_neighbour(SHAPE_CIRCLE) - 1], found[Shape::get_anticlockwise_neighbour(SHAPE_CIRCLE) - 1],
						x1, y1, x2, y2);

					if (bRetVal)
					{
						circle.start_x = x1;
						circle.start_y = y1;

						circle.end_x = x2;
						circle.end_y = y2;

						retVal->nr_of_no_detections++;
						retVal->vector_not_detected.push_back(circle);
						retVal->state = (iteration_state)(retVal->state | BackgroundFigureMissing);
					}
				}
				break;

			}
		}
	}


	return retVal;
}

iteration_return_t * VisualControl::iterate_processShapes()
{
	/// Clear formes arraysS
	triangles.clear();
	squares.clear();
	hexagons.clear();
	circles.clear();

	/// Put shapes into formes arrays
	for (int i = 0; i < shapes.size(); ++i)
	{
		switch (shapes[i].shapeType) {
		case SHAPE_TRIANGLE:
			triangles.push_back(shapes[i]);
			break;
		case SHAPE_SQUARE:
			squares.push_back(shapes[i]);
			break;
		case SHAPE_HEXAGON:
			hexagons.push_back(shapes[i]);
			break;
		case SHAPE_CIRCLE:
			circles.push_back(shapes[i]);
			break;
		default:
			break;
		}
	}

	/// Clear shapes base arrays
	shapes.clear();

	/// Detect platform with simple method
	if (simpleDetection()) {
		//qDebug() << "Simple method";
	}
	else if (centerFirstDetection()) {
		//qDebug() << "Center first method";
	}
	else {
		//qDebug() << "No platform";
	}

	iteration_return_t * pData = iterate_process_edge_shapes();

	calculateAltitude();

	//processEdgeShapes();
	if (pData->state == Success)
	{
		calculateOffset();
		calculatePlatformAngle();
		mDataValid = true;
	}
	else
	{
		mDataValid = false;
	}

	return pData;
}

void VisualControl::doDetection()
{
	if (capture == NULL)
	{

		//maybe in a separate init function
		capture = cvCreateCameraCapture(mCameraIndex); //cvCaptureFromCAM( 0 );
		if (NULL == capture)
		{
			//throw 
			//return -1;
		}

		/// Parameters required for video capture card
		cvSetCaptureProperty(capture, CV_CAP_PROP_FRAME_WIDTH, 640);
		cvSetCaptureProperty(capture, CV_CAP_PROP_FRAME_HEIGHT, 480);
		cvSetCaptureProperty(capture, CV_CAP_PROP_FPS, 30);

		// know the width and height of the frame
		frameWidth = cvGetCaptureProperty(capture, CV_CAP_PROP_FRAME_WIDTH);
		frameHeight = cvGetCaptureProperty(capture, CV_CAP_PROP_FRAME_HEIGHT);

		//GoPro
		/*	frameHeight = 720;
			frameWidth = 1280;*/
		frameHeight = 480;
		frameWidth = 640;
		//printf("[i] %.0f x %.0f\n", frameWidth, frameHeight);
		//initInterface();
	}

	if (this->mlogVideo && outputVideo== NULL)
	{ 
		Size outputSize(frameWidth, frameHeight);
		outputVideo = new VideoWriter(this->logDirectory + "\\video.avi", CV_FOURCC('i', 'Y', 'U', 'V'), 10, outputSize, true);
	}
	
	/// get shot	
	if (!showSingleImage)
	{	
		working_frame = cvQueryFrame(capture);
	}
	else
	{
		working_frame = original_single_frame;
	}
	//need else, when working_frame is changed!
	if(working_frame.rows == 0 ||working_frame.cols==0 || working_frame.channels() != 3)
	{
		return;
	}
	///time open
	//double t = (double)getTickCount();
	preprocessImage(working_frame);
	processContours(working_frame);
	// processShapes();

	iteration_return_t * retVal = iterate_processShapes();

	for (int i = 0; i < retVal->nr_of_no_detections; i++)
	{
		rectangle(helper, Rect(Point(retVal->vector_not_detected[i].start_x, retVal->vector_not_detected[i].start_y), 
			Point(retVal->vector_not_detected[i].end_x, retVal->vector_not_detected[i].end_y)), SHAPE_COLORS[0]);
	}

	/// Draw contour
	imshow("OutputHelper", helper);

	Mat original = src.clone();
	if ((retVal->state & BackgroundFigureMissing) == BackgroundFigureMissing )
	{
		for (int i = 0; i < retVal->nr_of_no_detections; i++)
		{
			processContours(original, retVal->vector_not_detected[i]);
		}
	}

	if ( (retVal->state & WrongFigureDetected) == WrongFigureDetected)
	{
		for (int i = 0; i < retVal->nr_of_wrong_detections; i++)
		{
			processContours(original, retVal->vector_wrong_detected[i]);
		}
	}

	// Winkelschätzung mit den neuen Figuren
	calculatePlatformAngle();
	calculateAltitude();

	/// Show in a window
	if (ShowResultImage || this->mlogVideo)
	{
		
		drawShapes();
		
		if (outputVideo != NULL && outputVideo->isOpened())
		{
			this->outputVideo->write( drawing);
		}
	}

	if (writeHtmlProtocol)
	{
		do_logging(retVal->state != Success);
	}

	delete retVal;

	/// Calculate time
	//t = ((double)getTickCount() - t)/getTickFrequency();
	//qDebug() << "Times passed in seconds: " << t;	
}

void VisualControl::setCalculationParams(double param1, double param2, double param3)
{
	this->param1 = param1;
	this->param2 = param2;
	this->param3 = param3;
}

void VisualControl::setVideoLogging(bool log, String logDirectory)
{	
	if (log)
	{
		this->mlogVideo = log;
		this->logDirectory = logDirectory;
	}
	else
	{
		if (outputVideo != NULL && outputVideo->isOpened())
		{
			this->outputVideo->release();
		}
	}

}


#include <ctime>
#include <cwchar>

#include <direct.h>

bool VisualControl::do_logging(bool iteration_needed)
{
	int x_size = 0;
	int y_size = 0;

	if (mCameraIndex == 0)
	{
		x_size = 320;
		y_size = 240;
	}

	if (mCameraIndex == 1)
	{
		x_size = 320;
		y_size = 180;
	}

	if (_fpLogfile == NULL)
	{
		wchar_t buffer[250];
		wchar_t tmpbuffer[150];
		char currentWorkingDirectory[200];
		

		time_t t = time(0);  
		struct tm * now = localtime(&t);

		_getcwd(currentWorkingDirectory, sizeof(currentWorkingDirectory));
		
		mbstowcs(tmpbuffer, currentWorkingDirectory, sizeof(tmpbuffer));

		swprintf(buffer, L"%s\\%04d-%02d-%02d_%02d-%02d-%02d", tmpbuffer, (now->tm_year + 1900), now->tm_mon + 1,
			now->tm_mday, now->tm_hour, now->tm_min, now->tm_sec);

		sprintf(log_filename, "%s\\%04d-%02d-%02d_%02d-%02d-%02d", currentWorkingDirectory, (now->tm_year + 1900), now->tm_mon + 1,
			now->tm_mday, now->tm_hour, now->tm_min, now->tm_sec);

		CreateDirectory(buffer, NULL);

		swprintf(buffer, L"%s%s", buffer, L"\\Pictures");

		CreateDirectory(buffer, NULL);

		
		_fpLogfile = fopen(string(string(log_filename) + "\\protocol.html").c_str(), "w+");
		if (_fpLogfile == NULL)
		{
			return false;
		}

		fprintf(_fpLogfile, "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd \">\n");
		fprintf(_fpLogfile, "<html>\n");
		fprintf(_fpLogfile, "\t<head>\n");
		fprintf(_fpLogfile, "\t\t<title>Protokoll</title>\n");
		fprintf(_fpLogfile, "\t</head>\n");


		fprintf(_fpLogfile, "\t<body>\n");
		fprintf(_fpLogfile, "\t\t<h3>Protokoll UAVControl mit Kamera: %s</h3>\n\n", (mCameraIndex == 0) ? "Flycam" : "GoPro");


		fprintf(_fpLogfile, "\t\t<table border=\"1\">\n");
		fprintf(_fpLogfile, "\t\t\t<thead>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>Original Bild</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>LPM - Erkennung</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>Missing Erkennung</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>Figurenerkennung</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>Iteration benoetigt</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>Winkel berechnet</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>Hoehe berechnet</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>THR</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>CCOEF</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>MED</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>CDT</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>RCOEF</th>\n");
		fprintf(_fpLogfile, "\t\t\t\t<th>CLSTHR</th>\n");
		fprintf(_fpLogfile, "\t\t\t</thead>\n\n");
	}

	fprintf(_fpLogfile, "\t\t\t<tr>\n");

	// dump all pcitures to file
	time_t t = time(0);
	struct tm * now = localtime(&t);

	char current_time[100];
	sprintf(current_time, "%4d-%02d-%02d_%02d-%02d-%02d", (now->tm_year + 1900), now->tm_mon + 1,
		now->tm_mday, now->tm_hour, now->tm_min, now->tm_sec);

	char picture_names[200];
	
	strcpy (picture_names, string(string(log_filename) + string ("\\Pictures\\") + string(current_time)).c_str());

	imwrite(string(string(picture_names) + "_orig.jpg").c_str(), src);
	imwrite(string(string(picture_names) + "_all_figs.jpg").c_str(), allFiguresFrame);
	imwrite(string(string(picture_names) + "_iteration_detection.jpg").c_str(), helper);
	imwrite(string(string(picture_names) + "_detection.jpg").c_str(), drawing);
	

	fprintf(_fpLogfile, "\t\t\t\t<td><img src=\"%s\" width=\"%d\" height=\"%d\"\"/></td>\n", string(string ("Pictures\\") + string(current_time) + "_orig.jpg").c_str(), x_size, y_size);
	fprintf(_fpLogfile, "\t\t\t\t<td><img src=\"%s\" width=\"%d\" height=\"%d\"\"/></td>\n", string(string("Pictures\\") + string(current_time) + "_detection.jpg").c_str(), x_size, y_size);

	if (iteration_needed)
	{ 
		fprintf(_fpLogfile, "\t\t\t\t<td><img src=\"%s\" width=\"%d\" height=\"%d\"\"/></td>\n", string(string("Pictures\\") + string(current_time) + "_iteration_detection.jpg").c_str(), x_size, y_size);
		fprintf(_fpLogfile, "\t\t\t\t<td><img src=\"%s\" width=\"%d\" height=\"%d\"\"/></td>\n", string(string("Pictures\\") + string(current_time) + "_all_figs.jpg").c_str(), x_size, y_size);
		fprintf(_fpLogfile, "\t\t\t\t<td>true</td>\n");
	}
	else
	{
		fprintf(_fpLogfile, "\t\t\t\t<td></td>\n");
		fprintf(_fpLogfile, "\t\t\t\t<td></td>\n");
		fprintf(_fpLogfile, "\t\t\t\t<td>false</td>\n");
	}
	
	fprintf(_fpLogfile, "\t\t\t\t<td>%.3f</td>\n", mPlaformAngle);
	fprintf(_fpLogfile, "\t\t\t\t<td>%.3f</td>\n", mPlatformAltitude);
	fprintf(_fpLogfile, "\t\t\t\t<td>%d</td>\n", thresh);
	fprintf(_fpLogfile, "\t\t\t\t<td>%d</td>\n", color_coeff);
	fprintf(_fpLogfile, "\t\t\t\t<td>%d</td>\n", min_eucl_dist);
	fprintf(_fpLogfile, "\t\t\t\t<td>%d</td>\n", center_detect_threshold);
	fprintf(_fpLogfile, "\t\t\t\t<td>%d</td>\n", raduis_coef);
	fprintf(_fpLogfile, "\t\t\t\t<td>%d</td>\n", class_threshold);

	fprintf(_fpLogfile, "\t\t\t</tr>\n\n");



	return true;
}


bool VisualControl::finish_logfile()
{
	if (_fpLogfile == NULL)
	{
		return false;
	}
	fprintf(_fpLogfile, "\t\t</table>\n");
	fprintf(_fpLogfile, "\t</body>\n");
	fprintf(_fpLogfile, "</html>\n");

}
/// end private functions  //////////////////////////////////////////