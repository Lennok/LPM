#include "VisualControl.h"
#include <msclr/marshal_cppstd.h>

#using <System.dll>
using namespace System::Threading;


namespace VisualControlLib
{

	public value struct str_VisualData
	{
		float Angle;
		float Altitude;
		float RelativeAltitude;
		value struct str_Offst
		{
			float X;
			float Y;
		}Offset;
		bool DataVlid;
	};

	public value struct str_CalibrationData
	{
		double param1; double param2; double param3;
		str_CalibrationData(double param1, double param2, double param3)
		{
			this->param1 = param1;
			this->param2 = param2;
			this->param3 = param3;
		}
	};

#pragma once
	public ref class VisualControlWrapper
	{
	public:
		VisualControlWrapper(int CameraIndex);
		~VisualControlWrapper();
		//int startPlatformDetection(int CameraIndex);
		//int stopPlatformDetection();
		//float getPlatformAngle();
		//float getPlatformAltitude();
		//float getPlatformOffset();
		//int getMainLoopRunning();
		int setShowGrayImage(bool show);
		int setShowCannyImage(bool show);
		int setShowResultImage(bool show);
		int setShowSettings(bool show);
		str_VisualData doDetection();
		void setCalculationParams(str_CalibrationData calibrationData);
		void setVideoLogging(bool log, System::String^ logDirectory);
		void end();

	private:
		//bool mainLoopRunning;
		//Thread^ mainLoopThread;
		int mCameraIndex;
		VisualControl* visContrHandle;

		//void mainLoop();
	};
}