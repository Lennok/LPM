#ifndef _NAVICONTROL_H_
#define _NAVICONTROL_H_

#using <System.dll>
#include "Protokol.h"
#include "SerialTester.h"

#include <vector>

using namespace System;
using namespace System::IO::Ports;
using namespace System::Threading;

typedef unsigned char u_char;
typedef std::vector< unsigned char > mk_data;

namespace OSDDataLib
{



#pragma once
	public delegate void OSDDataHandler(Object^ sender, OSDData data, Boolean dataValid);
	public delegate void FlightCTRLDataHandler(Object^ sender, String^ raw_data);

	public ref class NaviControl
	{
	public:
		NaviControl();		
		int requestReadOSDData();
		int requestSendExtControlData(array<Char>^ data);

		Int16 connect(String^ COMPort);
		Int16 disconnect();

		//events
		event OSDDataHandler^ OSDDataReceived;
		event FlightCTRLDataHandler^ FlightCTRLDataReceived;
		bool IsConnected();




	private:
		String^ COMPort;		
		SerialPort^ serial;
		//SerialTester^ tester = nullptr;

		OSDData readOsdData(String^ msg, Boolean& dataValid);
		bool parseMessage(String^ message, u_char &cmd, u_char &address, std::vector< unsigned char > &data);
		String^ generateMsg(u_char cmd, u_char address, std::vector< unsigned char > &data);
		void calcCRC(String^ message, u_char &CRC1, u_char &CRC2);

		void OnDataReceived(System::Object ^sender, System::IO::Ports::SerialDataReceivedEventArgs ^e);
	};

}


#endif
