#ifndef _SERIALTESTER_H_
#define _SERIALTESTER_H_

#using <System.dll>
using namespace System;
using namespace System::IO::Ports;

namespace OSDDataLib
{
#pragma once
	public ref class SerialTester
	{
	public:
		SerialTester(String^ COMPort);

		void OnDataReceived(System::Object ^sender, System::IO::Ports::SerialDataReceivedEventArgs ^e);

	private:
		String^ COMPort;
		SerialPort^ serial;
	};

}

#endif;