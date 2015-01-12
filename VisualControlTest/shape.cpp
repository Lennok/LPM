#include "shape.h"
#include <map>
#include <String.h>

using namespace VisualControlLib;

//init static member
/// Ranking features: Roundness, Rectangularity, Triangularity, Number of angles
int Shape::prototypesFeatures[][4] = {
		{ 56, 51, 85, 37 },		//Triangle
		{ 79, 88, 75, 50 },		//Square
		{ 83, 75, 69, 75 },			//Hexagon
		{ 80, 77, 68, 100 },		//Circle
};


Shape::Shape(std::vector<cv::Point> &contour, int type)
{
	shapeContour = contour;
	shapeArea = 0;
	shapeRadius = 0;
	shapeChildrenCount = 0;
	shapeType = SHAPE_NONE;


	float radius;
	cv::minEnclosingCircle(shapeContour, shapeCenter, radius);

	shapeArea = cv::contourArea(shapeContour);
	shapeType = type;
	shapeRadius = radius;

	shapeChildrenCount += 1;
}

Shape::Shape()
{	
	shapeContour = *(new std::vector<cv::Point>());
	shapeArea = 0;
	shapeRadius = 0;
	shapeChildrenCount = 0;
	shapeType = SHAPE_NONE;
}

bool Shape::centerIsInside(std::vector<cv::Point> &contour, double &eucliadianDistance)
{

	cv::Point2f center;
	float radius;
	cv::minEnclosingCircle(contour, center, radius);

	double distance;
	distance = cv::pointPolygonTest(shapeContour, center, false);

	eucliadianDistance = 0;
	cv::Point diff = shapeCenter - center;
	eucliadianDistance = cv::sqrt((double)(diff.x*diff.x + diff.y*diff.y));

	return (distance >= 0);
}

void Shape::mergeContours(std::vector<cv::Point> &contour)
{
	float area = cv::contourArea(contour);

	if (area > shapeArea) {
		/// Set new area of shape
		shapeArea = area;

		/// Calculate new center of shape
		float radius;
		cv::minEnclosingCircle(contour, shapeCenter, radius);

		shapeContour = contour;
	}

	shapeChildrenCount += 1;
}

void Shape::calculateFeatures(std::vector<cv::Point> &contour, std::map<std::string, double> &features)
{

	/// Approximate contour
	double perimeter = arcLength(contour, true);
	double epsilon = 0.02*perimeter;
	std::vector<cv::Point> approx;
	cv::approxPolyDP(contour, approx, epsilon, true);
	

	double min_wtf = perimeter / 15;
	for (int item = 0; item < approx.size(); ++item) {

		int next;
		if (item < approx.size() - 1) {
			next = item + 1;
		}
		else {
			next = 0;
		}

		cv::Point diff = approx[item] - approx[next];
		double eucliadianDistance = cv::sqrt((double)(diff.x*diff.x + diff.y*diff.y));

		if (eucliadianDistance < min_wtf) {
			approx.erase(approx.begin() + item);
		}

	}
	if (approx.size() < 3)
	{
		return;
	}
	bool wasErased = false;
	for (int n = 0; n < approx.size(); n++)
	{
		for (int j = 0; j < approx.size(); j++)
		{
			double dist1 = approx[n].x - approx[j].x;
			double dist2 = approx[n].y - approx[j].y;
			double tmpDistance =  cv::sqrt(dist1*dist1 + dist2*dist2);
			if (tmpDistance < 5 && tmpDistance != 0)
			{
				approx.erase(approx.begin() + j);
				//restart
				n = 0;
				j = 0;
				wasErased = true;
				break;
			}
		}
	}
	if (wasErased)
	{
		cv::convexHull(approx, approx);
	}
	if (approx.size() < 3)
	{
		return;
	}
	bool isClosed = cv::isContourConvex(approx);

	/// Calculate basic features
	double area = cv::contourArea(approx);
	double sides = (double)approx.size();

	/// Calculate moments of contour
	cv::Moments curMnts = moments(approx, false);

	cv::RotatedRect mbr = cv::minAreaRect(approx);
	double mbrArea = mbr.size.area();
	double roundness = 4 * PI*area / (perimeter*perimeter);
	double rectangularity = area / mbrArea;
	double eccentricity = (pow((curMnts.mu20 - curMnts.mu02), 2) - 4 * curMnts.mu11*curMnts.mu11) / pow((curMnts.mu20 + curMnts.mu02), 2);
	double affineMomentInvariant = (curMnts.mu20*curMnts.mu02 - curMnts.mu11*curMnts.mu11) / pow(curMnts.m00, 4);
	double triangularity;
	if (affineMomentInvariant <= 1.0 / 108) {
		triangularity = 108 * affineMomentInvariant;
	}
	else {
		triangularity = 1 / (108 * affineMomentInvariant);
	}
	

	features.clear();
	features.insert(std::pair<std::string, double>("perimeter", perimeter));
	features.insert(std::pair<std::string, double>("area", area));
	features.insert(std::pair<std::string, double>("sides", sides));
	features.insert(std::pair<std::string, double>("roundness", roundness));
	features.insert(std::pair<std::string, double>("rectangularity", rectangularity));
	features.insert(std::pair<std::string, double>("eccentricity", eccentricity));
	features.insert(std::pair<std::string, double>("triangularity", triangularity));
	features.insert(std::pair<std::string, double>("isClosed", isClosed));
}

int Shape::classifyShape(std::vector<cv::Point> &contour, double threshold, bool methode)
{
	int shapeType = SHAPE_NONE;
	if (methode == false)
	{
		shapeType = Shape::classifyShape(contour, threshold);
	}
	else
	{
		std::map<std::string, double> features;
		calculateFeatures(contour, features);
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
	}
	return shapeType;
}
int Shape::classifyShape(std::vector<cv::Point> &contour, double threshold)
{
	std::map<std::string, double> features;

	calculateFeatures(contour, features);

	const double featuresValues[4] = {
		features["roundness"],
		features["rectangularity"],
		features["triangularity"],
		features["sides"] / 8.0,
	};

	double resultDistance[4] = { 0 };

	/// Classify shape
	int shapeType = SHAPE_NONE;
	if (features["area"] > MINIMAL_AREA && features["isClosed"] == true && features["eccentricity"] < 0.03) {
		for (int shape_index = 0; shape_index<4; ++shape_index) {
			for (int feature_index = 0; feature_index<4; ++feature_index) {
				resultDistance[shape_index] += abs(featuresValues[feature_index] - (Shape::prototypesFeatures[shape_index][feature_index]/100.0));
			}
		}
		double minValue = 10;
		double minIndex = 0;
		
		for (int i = 0; i<4; ++i) {
			if (resultDistance[i] < minValue) {
				minValue = resultDistance[i];
				minIndex = i;
			}
		}
		if (minValue <= threshold) {
			//qDebug() << minIndex+1 << " -> " << minValue;
			shapeType = SHAPE_NONE + minIndex + 1;
		}
	}
	else {
		shapeType = SHAPE_NONE;
	}
	return shapeType;
}

int Shape::detectCentralShape(cv::Mat &image, cv::Point2f center, double radius, int threshold)
{
	cv::Mat src;
	circle(src, center, radius, cv::Scalar(0, 0, 0), 1);

	int count = 0;

	for (int i = 0; i<360; ++i) {
		double angle = PI / 180.0 * (double)i;
		int x = (int)(center.x + radius * cos(angle));
		int y = (int)(center.y + radius * sin(angle));

		if (x >= 0 && y >= 0 && x < image.cols && y < image.rows) {
			int pixel = image.at<uchar>(y, x);

			if (pixel < threshold) {
				circle(src, cv::Point(x, y), 1, cv::Scalar(0, 65, 255), 2);
				count += 1;
			}
		}
	}
	return count;
}

int Shape::get_cross_neighbour(int shape)
{
	switch (shape)
	{
		case SHAPE_TRIANGLE:
			return SHAPE_SQUARE;

		case SHAPE_SQUARE:
			return SHAPE_TRIANGLE;

		case SHAPE_HEXAGON:
			return SHAPE_CIRCLE;

		case SHAPE_CIRCLE:
			return SHAPE_HEXAGON;
	}
}


int Shape::get_clockwise_neighbour(int shape)
{
	switch (shape)
	{
		case SHAPE_TRIANGLE:
			return SHAPE_HEXAGON;

		case SHAPE_SQUARE:
			return SHAPE_CIRCLE;

		case SHAPE_HEXAGON:
			return SHAPE_SQUARE;

		case SHAPE_CIRCLE:
			return SHAPE_TRIANGLE;
	}
}

int Shape::get_anticlockwise_neighbour(int shape)
{
	switch (shape)
	{
		case SHAPE_TRIANGLE:
			return SHAPE_CIRCLE;

		case SHAPE_SQUARE:
			return SHAPE_HEXAGON;

		case SHAPE_HEXAGON:
			return SHAPE_TRIANGLE;

		case SHAPE_CIRCLE:
			return SHAPE_SQUARE;
	}
}
