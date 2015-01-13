#include "VisualControlWrapper.h"

using namespace VisualControlLib;


VisualControlWrapper::VisualControlWrapper(int CameraIndex)
{	

	this->visContrHandle = new VisualControl(CameraIndex);
	mCameraIndex = CameraIndex;
	//mainLoopThread = gcnew Thread(gcnew ThreadStart(this, &VisualControlWrapper::mainLoop));
}

void VisualControlWrapper::end()
{
	delete this->visContrHandle;
}

VisualControlWrapper::~VisualControlWrapper()
{
	delete this->visContrHandle;
}
//int VisualControlWrapper::startPlatformDetection(int CameraIndex)
//{
//	mCameraIndex = CameraIndex;
//	if (!(mainLoopRunning == 1) && visContrHandle==NULL)
//	{
//		visContrHandle = new VisualControl();
//		mainLoopThread = gcnew Thread(gcnew ThreadStart(this, &VisualControlWrapper::mainLoop));
//		mainLoopRunning = true;
//		//start main loop which does the  image processing
//		mainLoopThread->Start();
//
//		// Loop until mainLoop thread activates. 
//		while (!mainLoopThread->IsAlive);
//	}
//	return 0;
//}
//
//int VisualControlWrapper::stopPlatformDetection()
//{
//	if (mainLoopRunning == 1 && visContrHandle != NULL)
//	{
//		mainLoopRunning = false;		
//		mainLoopThread->Join();
//		delete visContrHandle;
//		visContrHandle = NULL;
//	}
//	return 0;
//}

//void VisualControlWrapper::mainLoop()
//{
//	while (mainLoopRunning)
//	{
//		visContrHandle->doDetection(mCameraIndex);
//	}	
//}
//
//int VisualControlWrapper::getMainLoopRunning()
//{
//	if (visContrHandle != NULL)
//		return this->mainLoopRunning;
//	else
//	{
//		return NULL;
//	}
//}

int VisualControlWrapper::setShowGrayImage(bool show)
{
	return this->visContrHandle->setShowGrayImage(show);
}

int VisualControlWrapper::setShowCannyImage(bool show)
{
		return this->visContrHandle->setShowCannyImage(show);
}

int VisualControlWrapper::setShowResultImage(bool show)
{
	return this->visContrHandle->setShowResultImage(show);
}

int VisualControlWrapper::setShowSettings(bool show)
{
	return this->visContrHandle->setShowSettings(show);
}

int VisualControlWrapper::setShowImage(bool show)
{
	return this->visContrHandle->setShowImage(show);
}

int VisualControlWrapper::setShowAllShapes(bool show)
{
	return this->visContrHandle->setShowAllShapes(show);
}

int VisualControlWrapper::setWriteHtmlProtocol(bool write)
{
	return this->visContrHandle->setWriteHtmlProtocol(write);
}

str_VisualData VisualControlWrapper::doDetection()
{
	str_VisualData data;
	this->visContrHandle->doDetection();
	data.Altitude = this->visContrHandle->PlatformAltitude();
	data.Angle = this->visContrHandle->PlatformAngle();
	data.RelativeAltitude = this->visContrHandle->PlatformRelativeAltitude();
	data.Offset.X = this->visContrHandle->PlatformOffsetX();
	data.Offset.Y = this->visContrHandle->PlatformOffsetY();
	data.DataVlid = this->visContrHandle->DataValid();
	return data;
}

void VisualControlWrapper::setCalculationParams(str_CalibrationData calibrationData)
{
	this->visContrHandle->setCalculationParams(calibrationData.param1, calibrationData.param2, calibrationData.param3);
}

void VisualControlWrapper::setVideoLogging(bool log, System::String^ logDirectory)
{
	this->visContrHandle->setVideoLogging(log, msclr::interop::marshal_as<std::string>(logDirectory));
}
