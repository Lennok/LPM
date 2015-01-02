#include "SerialTester.h"
#using <System.dll>

using namespace System;
using namespace System::IO::Ports;
using namespace OSDDataLib;

SerialTester::SerialTester(String^ COMPort)
{
	this->COMPort = COMPort;
	serial = nullptr;

	//setup serial port
	serial = gcnew SerialPort();
	serial->PortName = COMPort;
	serial->BaudRate = 57600;
	serial->DataBits = 8;
	serial->Parity = Parity::None;
	serial->StopBits = StopBits::One;
	serial->NewLine = "\r";
	serial->ReadTimeout = 500;
	serial->WriteTimeout = 500;
	serial->Open();
	serial->DataReceived += gcnew SerialDataReceivedEventHandler(this, &SerialTester::OnDataReceived);
}

void SerialTester::OnDataReceived(System::Object ^sender, SerialDataReceivedEventArgs ^e)
{
	String^ raw_data;

	raw_data = serial->ReadExisting();
	System::Text::StringBuilder^ sb = gcnew System::Text::StringBuilder(raw_data);

	//

	if (raw_data[0] == '#') {
	}

	switch (raw_data[2])
	{
	case 'o':
	{
				String^ data = gcnew String("#cO>M======wWMrSCuD==A==================================================]======d=====?D=Ly=ny=N=Le========R>M=t>M==vI");
				serial->WriteLine(data);
	}
		break;
	case 'b':
	{
				sb[2] = 'B';
				serial->WriteLine(sb->ToString());
	}
		break;

	default:
		break;
	}

	
}