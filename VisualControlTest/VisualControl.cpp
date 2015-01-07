#include "VisualControl.h"

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
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
	showFigureCharacteristics = false;
	showSingleImage = false;
	mPlaformAngle = 0;
	mPlatformAltitude = 0;
	mPlatformOffsetX = 0;
	mPlatformOffsetY = 0;
	mPlatformRelativeAltitude = 0;
	mlogVideo = false;
	outputVideo = NULL;

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
	CIRCLE_CHARACTERISTIC_WINDOW = "Circle Characteristics" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	SQUARE_CHARACTERISTIC_WINDOW = "Square Characteristics" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	HEXAGON_CHARACTERISTIC_WINDOW = "Hexagon Characteristics" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();
	TRIANGLE_CHARACTERISTIC_WINDOW = "Triangle Characteristics" + static_cast<ostringstream*>( &(ostringstream() << mCameraIndex) )->str();

	capture = NULL;

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
		}
		else
		{
			showSingleImage = false;
		}

	}
	return 0;
}
int VisualControl::setShowFigureCharasteristics(bool show)
{
	showFigureCharacteristics = show;
	if (show)
	{
		//prototypesFeatures[4][4] -> Triangle, Square, Hexagon, Circle
		//2nd Dimension: Roundness, Rectangularity, Triangularity, Number of angles

		cvNamedWindow(TRIANGLE_CHARACTERISTIC_WINDOW.c_str(), CV_WINDOW_NORMAL);
		//createTrackbar("Roundness:", TRIANGLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[0][0], 100);
		//createTrackbar("Rectangularity", TRIANGLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[0][1], 100);
		createTrackbar("Triangularity", TRIANGLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[0][2], 100);
		//createTrackbar("Number of angles", TRIANGLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[0][3], 100);
		cvMoveWindow(TRIANGLE_CHARACTERISTIC_WINDOW.c_str(), 0 , 0);

		cvNamedWindow(SQUARE_CHARACTERISTIC_WINDOW.c_str(), CV_WINDOW_NORMAL);
		//createTrackbar("Roundness:", SQUARE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[1][0], 100);
		createTrackbar("Rectangularity", SQUARE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[1][1], 100);
		//createTrackbar("Triangularity", SQUARE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[1][2], 100);
		//createTrackbar("Number of angles", SQUARE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[1][3], 100);
		cvMoveWindow(SQUARE_CHARACTERISTIC_WINDOW.c_str(), 200 , 0);
		
		cvNamedWindow(HEXAGON_CHARACTERISTIC_WINDOW.c_str(), CV_WINDOW_NORMAL);
		/*createTrackbar("Roundness:", HEXAGON_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[2][0], 100);
		createTrackbar("Rectangularity", HEXAGON_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[2][1], 100);
		createTrackbar("Triangularity", HEXAGON_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[2][2], 100);
		createTrackbar("Number of angles", HEXAGON_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[2][3], 100);*/
		cvMoveWindow(HEXAGON_CHARACTERISTIC_WINDOW.c_str(), 400 , 0);

		cvNamedWindow(CIRCLE_CHARACTERISTIC_WINDOW.c_str(), CV_WINDOW_NORMAL);
		createTrackbar("Roundness:", CIRCLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[3][0], 100);
	/*	createTrackbar("Rectangularity", CIRCLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[3][1], 100);
		createTrackbar("Triangularity", CIRCLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[3][2], 100);
		createTrackbar("Number of angles", CIRCLE_CHARACTERISTIC_WINDOW, &Shape::prototypesFeatures[3][3], 100);*/
		cvMoveWindow(CIRCLE_CHARACTERISTIC_WINDOW.c_str(), 600 , 0);

	}
	else
	{
		cvDestroyWindow(CIRCLE_CHARACTERISTIC_WINDOW.c_str());
		cvDestroyWindow(HEXAGON_CHARACTERISTIC_WINDOW.c_str());
		cvDestroyWindow(SQUARE_CHARACTERISTIC_WINDOW.c_str());
		cvDestroyWindow(TRIANGLE_CHARACTERISTIC_WINDOW.c_str());
	}	
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
void VisualControl::Intensity(Mat inputArray, double* energy, double* entropy, double* contrast)
{
	//needs "opencv_legecy.lib"
	//Direction 0°
	//LdGray = 7

	/*IplImage lagacyImage = inputArray;
	CvGLCM* coocMatrix = cvCreateGLCM(&lagacyImage,7);
	*energy = cvGetGLCMDescriptor(coocMatrix, 0, CV_GLCMDESC_ENERGY);
	*entropy = cvGetGLCMDescriptor(coocMatrix, 0, CV_GLCMDESC_ENTROPY);
	*contrast = cvGetGLCMDescriptor(coocMatrix, 0, CV_GLCMDESC_CONTRAST);*/
}


void VisualControl::preprocessImage(Mat &frame) {

	Mat hsv, yuv;
	vector<Mat> channels_hsv;
	vector<Mat> channels_yuv;
	src = frame.clone();
	drawing = src.clone();

	

	///convert to grayscale
	cvtColor(frame, hsv, CV_RGB2HSV);
	cvtColor(frame, yuv, CV_RGB2YUV);
	split(hsv, channels_hsv);
	split(yuv, channels_yuv);
	double energy, entropy, contrast;
	Intensity(channels_hsv[1],&energy, &entropy, &contrast);

	/*if(!showSettings)
	{
		cv::Scalar meanS = mean(channels_hsv[1]);
		cv::Scalar meanV, deviationV;
		meanStdDev(channels_hsv[2], meanV, deviationV);
		cv::Scalar meanY,deviationY;
		meanStdDev(channels_yuv[0],meanY,deviationY);
		
		//no functions for entropy, energy and contrast
		cv::Scalar entropyV = mean(channels_hsv[1]);
		cv::Scalar entropyY = mean(channels_hsv[1]);
		cv::Scalar energyV = mean(channels_hsv[1]);
		cv::Scalar contrastV = mean(channels_hsv[1]);
		cv::Scalar energyY = mean(channels_hsv[1]);
		cv::Scalar contrastY = mean(channels_hsv[1]);
	}*/
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

	/// Detect edges using canny
	Canny(frame, frame, thresh, thresh * 2, 3);

	if (ShowCannyImage)
	{
		imshow(CANNY_WINDOW, frame);
	}
}


void VisualControl::processContours(Mat &frame)
{
	vector<vector<Point> > contours;
	vector<Vec4i> hierarchy;
	int count = 0;
	/// Find contours	
	findContours(frame, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0));

	int counter = 0;
	for (vector<vector<Point> >::iterator it = contours.begin(); it != contours.end(); ++it)
	{
		counter ++;
	}
	/// Clear the vector with forms
	shapes.clear();
	if (showAllFigures)
	{
		for (int i = 0; i< contours.size(); i++)
		{
			int shapeType = SHAPE_NONE;
			shapeType = Shape::classifyShape(contours[i], (double)(class_threshold / 100.0), showFigureCharacteristics);
						
			Shape temp(contours[i], shapeType);
			std::map<std::string, double> features;
			temp.calculateFeatures(contours[i], features);
			if (features["isClosed"] && temp.shapeType != SHAPE_NONE)
			{
				shapes.push_back(temp);
				count++;
			}			
		}
		///Draw shapes
		vector<vector<Point> > resultContours;

		for (int i = 0; i<shapes.size(); ++i) {
			resultContours.push_back(shapes[i].shapeContour);	
			drawContours(drawing, resultContours, i,  SHAPE_COLORS[shapes[i].shapeType], 1, 8, hierarchy, 0, Point());
		}
		if (ShowResultImage)
		{
			imshow(RESULT_WINDOW, drawing);
		}
	}
	else
	{
		/// Fill shapes vector
		for (int i = 0; i< contours.size(); i++)
		{
			/// Approximate contour to find border count
			int shapeType = SHAPE_NONE;
			shapeType = Shape::classifyShape(contours[i], (double)(class_threshold / 100.0));


			if (shapeType != SHAPE_NONE) {

				if (shapes.size() == 0) {
					Shape temp(contours[i], shapeType);
					shapes.push_back(temp);
				}
				else 
				{
					bool isAdded = false;
				
				
						///check whether the circuit to any figure, if so, to absorb less a figure greater
						for (int j = 0; j<shapes.size(); ++j) {
							double eucliadianDistance = 0;
							bool isInside = shapes[j].centerIsInside(contours[i], eucliadianDistance);

							if (shapes[j].shapeType == shapeType && isInside && eucliadianDistance < min_eucl_dist) {
								shapes[j].mergeContours(contours[i]);
								isAdded = true;
								break;
							}
						}

						if (!isAdded) {
							Shape temp(contours[i], shapeType);
							shapes.push_back(temp);
						}

				
				}
			}
		}
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
	//processEdgeShapes();
	if (0 == processEdgeShapes())
	{
		calculateAltitude();
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

	for (int i = 0; i<shapes.size(); ++i) {
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

		if (centerIsInside && eucliadianDistance > min_eucl_dist && area_ratio < 2 && area_ratio > 0.5) {
			shapes.push_back(items[i]);
			break;
		}
	}
}

bool VisualControl::simpleDetection() {
	centerShape = Shape();
	platformShape = Shape();
	triangleShape = Shape();
	squareShape = Shape();
	hexagonShape = Shape();
	circleShape = Shape();

	for (int circle_item = 0; circle_item < circles.size(); ++circle_item) {
		for (int square_item = 0; square_item < squares.size(); ++square_item) {
			double eucliadianDistance = 0;
			bool centerIsInside = circles[circle_item].centerIsInside(squares[square_item].shapeContour, eucliadianDistance);
			double area_ratio = squares[square_item].shapeArea / circles[circle_item].shapeArea;

			if (centerIsInside && eucliadianDistance < min_eucl_dist && area_ratio > 2.5) {
				if (squares[square_item].shapeArea > platformShape.shapeArea && circles[circle_item].shapeArea > centerShape.shapeArea) {
					platformShape = squares[square_item];
					platformShape.shapeType = SHAPE_PLATFORM;

					centerShape = circles[circle_item];
					centerShape.shapeType = SHAPE_CENTER;
				}
			}
		}
	}

	//Adding center and found a platform in an array of figures
	if (platformShape.shapeArea > 0 && centerShape.shapeArea > 0) {
		shapes.push_back(platformShape);
		shapes.push_back(centerShape);

		pushShape_1(triangles);
		pushShape_1(squares);
		pushShape_1(hexagons);
		pushShape_1(circles);

		return true;
	}
	else {
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

int VisualControl::processEdgeShapes() {
	double eucliadianDistance = 0;
	bool is_inside = false;
	bool one_shape_found = false;

	if (shapes.size() == 0)
	{
		return -1;
	}

	for(int i = 0; i< shapes.size(); i++)
	{
		switch (shapes[i].shapeType) {
		case SHAPE_TRIANGLE:

			if (circleShape.shapeArea > 0) {
				is_inside = circleShape.centerIsInside(shapes[i].shapeContour, eucliadianDistance);
			}
			else {
				is_inside = false;
			}
			if (shapes[i].shapeArea > triangleShape.shapeArea && !is_inside) {
				triangleShape = shapes[i];
			}
			one_shape_found = true;
			break;
		case SHAPE_SQUARE:
			if (shapes[i].shapeArea > squareShape.shapeArea) {
				squareShape = shapes[i];
			}
			one_shape_found = true;
			break;
		case SHAPE_HEXAGON:
			if (shapes[i].shapeArea > hexagonShape.shapeArea) {
				hexagonShape = shapes[i];
			}
			one_shape_found = true;
			break;
		case SHAPE_CIRCLE:
			if (hexagonShape.shapeArea > 0) {
				is_inside = hexagonShape.centerIsInside(shapes[i].shapeContour, eucliadianDistance);
			}
			else {
				is_inside = false;
			}
			if (shapes[i].shapeArea > circleShape.shapeArea && !is_inside) {
				circleShape = shapes[i];
			}
			one_shape_found = true;
			break;
		}
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
	if (centerShape.shapeArea > 0) {
		double frameFactor = sqrt(frameHeight*frameWidth);
		double shapeFactor = sqrt(centerShape.shapeArea);

		double x = frameFactor / shapeFactor;
		//double distance = 0.0064*x*x + 38.112*x + 5;
		double distance = param1*x*x + param2*x + param3;
		//double distance = x;

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
	showAllFigures = true;
	preprocessImage(working_frame);
	processContours(working_frame);
	processShapes();
	/// Show in a window
	if (ShowResultImage || this->mlogVideo)
	{
		if (!showAllFigures)
		{
			drawShapes();
		}
		if (outputVideo != NULL && outputVideo->isOpened())
		{
			this->outputVideo->write( drawing);
		}
	}

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

/// end private functions  //////////////////////////////////////////