#ifndef SHAPE_H
#define SHAPE_H

#include <opencv2/core/core.hpp>
#include <opencv2/imgproc/imgproc.hpp>

namespace VisualControlLib
{

	enum {
		SHAPE_NONE = 0,
		SHAPE_TRIANGLE = 1,
		SHAPE_SQUARE = 2,
		SHAPE_HEXAGON = 3,
		SHAPE_CIRCLE = 4,
		SHAPE_CENTER = 5,
		SHAPE_PLATFORM = 6
	};

	/// Define shapes colors

	const cv::Scalar SHAPE_COLORS[7] = {
		cv::Scalar(0, 0, 0),
		cv::Scalar(28, 232, 0),
		cv::Scalar(28, 247, 255),
		cv::Scalar(212, 96, 0),
		cv::Scalar(28, 0, 255),
		cv::Scalar(255, 222, 63),
		cv::Scalar(255, 255, 255)
	};

	const int MINIMAL_AREA = 200;
	const double PI = 3.141592653589793238462;


	const double prototypesFactors[4][4] = {
			{ 1, 1, 1.5, 0.8 },
			{ 1, 1.5, 1, 1 },
			{ 1, 1, 1, 1 },
			{ 1.5, 1, 1, 1 },
	};

	public class Shape
	{
	public:
		std::vector<cv::Point> shapeContour;
		cv::Point2f shapeCenter;
		float shapeArea;
		float shapeRadius;
		int shapeChildrenCount;
		int shapeType;

	public:
		Shape();
		Shape(std::vector<cv::Point> &contour, int type);
		bool centerIsInside(std::vector<cv::Point> &contour, double &eucliadianDistance);
		void mergeContours(std::vector<cv::Point> &contour);

	public:
		static int prototypesFeatures[4][4];
		static void calculateFeatures(std::vector<cv::Point> &contour, std::map<std::string, double> &features);
		static int classifyShape(std::vector<cv::Point> &contour, double threshold);
		static int detectCentralShape(cv::Mat &image, cv::Point2f center, double radius, int threshold);

	};

	
}

#endif // SHAPE_H
