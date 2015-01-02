#include "NaviControl.h"
#using <System.dll>
#include "Protokol.h"
#include "SerialTester.h"

using namespace System;
using namespace System::IO::Ports;
using namespace System::Threading;
using namespace OSDDataLib;
using namespace System::Runtime::InteropServices;

//NaviData globNaviData;

NaviControl::NaviControl()
{	
	serial = nullptr;
}

bool NaviControl::IsConnected()
{
	if (serial != nullptr)
	{
		return serial->IsOpen;
	}
	else
	{
		return false;
	}
}

Int16 NaviControl::connect(String^ COMPort)
{
	this->COMPort = COMPort;
	if (serial == nullptr)
	{
		

		try
		{
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

			serial->DataReceived += gcnew SerialDataReceivedEventHandler(this, &NaviControl::OnDataReceived);

			//if (tester == nullptr)
			//{
			//	//Init Tester
			//	tester = gcnew SerialTester("COM6");
			//}

			if (serial->IsOpen) {
				return 0;
			}
			else
			{
				return -1;
			}
		}
		catch (Exception^ e)
		{
			serial = nullptr;
			throw e;
		}
	}
	return -1;
}

Int16 NaviControl::disconnect()
{
	if (serial != nullptr)
	{
		if (serial->IsOpen)
		{
			serial->Close();
			serial = nullptr;
		}
	}
	else
	{
		return false;
	}

	return true;
}

//void NaviControl::mainLoop()
//{
//
//	String^ raw_data;
//	String^ data = gcnew String("");
//	//send request
//	std::vector< unsigned char > temp;// = { 10, 171, 171 };
//	temp.push_back(10);
//	temp.push_back(171);
//	temp.push_back(171);
//	String^ text = generateMsg('o', 1, temp);
//	serial->Write(text);
//
//	int i = 0;
//	int blankCount = 0;
//
//	while (mainLoopRunning) {
//		try
//		{
//			raw_data = serial->ReadLine();
//
//			if (raw_data[0] == '#') {
//				++i;
//				u_char cmd;
//				u_char addr;
//				std::vector< unsigned char > sensorsData;
//
//				String^ stringData(data);
//				if (parseMessage(stringData, cmd, addr, sensorsData)) {
//					//Console::WriteLine("packet #" + i + " -> ");
//
//					if (readOsdData(data, globNaviData)) {
//						//logToConsole(lastNaviData);
//						//logToFile(lastNaviData, out);
//					}
//				}
//				data = String::Empty;
//			}
//			data += raw_data;
//			if (raw_data->Length == 0) {
//				blankCount++;
//			}
//			if (blankCount > 5) {
//				blankCount = 0;
//				serial->WriteLine(generateMsg('o', 1, temp));
//			}
//		}
//		catch (TimeoutException ^) {
//			Console::WriteLine("Timeout");
//			serial->WriteLine(generateMsg('o', 1, temp));
//		}
//	}
//}

int NaviControl::requestReadOSDData()
{ 
	if (serial != nullptr && serial->IsOpen)
	{
		std::vector< unsigned char > temp = *(new std::vector< unsigned char >());
		temp.push_back(10);
		temp.push_back(171);
		temp.push_back(171);
		serial->WriteLine(generateMsg('o', 1, temp));
		//delete &temp;
		return 0;
	}
	return -1; 
}

int NaviControl::requestSendExtControlData(array<Char>^ data)
{

	if (serial != nullptr && serial->IsOpen)
	{		
		std::vector< unsigned char > temp = *(new std::vector< unsigned char >());
		for each(Char item in data)
		{
			temp.push_back(item);
		}
		serial->WriteLine(generateMsg('b', 1, temp));
		//delete &temp;
		return 0;
	}
	//SendOutData('b', 1, (unsigned char *)&ExternControl, sizeof(struct str_ExternControl));
	return -1; 
};

void NaviControl::calcCRC(String^ message, u_char &CRC1, u_char &CRC2) {
	//calculate CRC
	unsigned int tmpCRC = 0;

	for (int i = 0; i < message->Length; i++)
	{
		tmpCRC += message[i];
	}

	tmpCRC %= 4096;
	CRC1 = '=' + tmpCRC / 64;
	CRC2 = '=' + tmpCRC % 64;

}

String^ NaviControl::generateMsg(u_char cmd, u_char address, std::vector<unsigned char>& data) {
	String^ message = gcnew String("");

	message += "#";
	message += Convert::ToChar('a' + address);
	message += Convert::ToChar(cmd);

	bool done = false;
	int i = 0;
	int len = data.size();
	while ((i<data.size()) && !done) {
		u_char a = 0;
		u_char b = 0;
		u_char c = 0;

		try {

			if (len > 0) { a = data[i++]; len--; }
			else a = 0;
			if (len > 0) { b = data[i++]; len--; }
			else b = 0;
			if (len > 0) { c = data[i++]; len--; }
			else c = 0;
		}
		catch (...) {
			done = true;
		}

		message += Convert::ToChar(('=' + (a >> 2)));
		message += Convert::ToChar(('=' + (((a & 0x03) << 4) | ((b & 0xf0) >> 4))));
		message += Convert::ToChar(('=' + (((b & 0x0f) << 2) | ((c & 0xc0) >> 6))));
		message += Convert::ToChar(('=' + (c & 0x3f)));
	}


	//calculate CRC
	u_char CRC1;
	u_char CRC2;
	calcCRC(message, CRC1, CRC2);

	message += Convert::ToChar((CRC1));
	message += Convert::ToChar((CRC2));
	message += "\r";

	return message;
}

bool NaviControl::parseMessage(String^ message, u_char &cmd, u_char &address, std::vector< unsigned char > &data) {
	if (message->Length < 6) {
		return false;
	}
	if (message[0] != '#') {
		return false;
	}

	u_char CRC1;
	u_char CRC2;
	
	//message=message->TrimEnd(); 
	//message = message->TrimStart();
	int legth = message->Length;
	String^ crcString = gcnew String(message->Substring(0, message->Length - 2));
	calcCRC(crcString, CRC1, CRC2);



	if (CRC1 != (message[message->Length - 2]) && CRC2 != message[message->Length - 1]) {
		return false;
	}
	else {

		address = message[1];
		cmd = message[2];

		int dataLength = message->Length - 5;
		String^ data64 = message->Substring(3, dataLength);

		data.clear();
		bool done = false;
		int i = 0;

		while ((i<data64->Length) && !done) {
			int a = 0;
			int b = 0;
			int c = 0;
			int d = 0;
			try {
				a = data64[i] - '=';
				b = data64[i + 1] - '=';
				c = data64[i + 2] - '=';
				d = data64[i + 3] - '=';
				i = i + 4;
			}
			catch (...) {
				done = true;
			}


			data.push_back((a << 2) & 0xFF | (b >> 4));
			data.push_back(((b & 0x0f) << 4) & 0xFF | (c >> 2));
			data.push_back(((c & 0x03) << 6) & 0xFF | d);
		}

		return true;
	}
}


OSDData NaviControl::readOsdData(String^ msg, Boolean& dataValid){
	u_char cmd;
	u_char address = 0;
	OSDData lastNaviData = *(gcnew OSDData());
	int test = Marshal::SizeOf(lastNaviData);
	std::vector< unsigned char > data(Marshal::SizeOf(lastNaviData));
	dataValid = false;

	memset(&data[0], 0, Marshal::SizeOf(lastNaviData));

	memset(&lastNaviData, 0, Marshal::SizeOf(lastNaviData));

	if (parseMessage(msg, cmd, address, data))
	{
		memcpy(&lastNaviData, &data[0], Marshal::SizeOf(lastNaviData));
		dataValid = true;
	}
	return lastNaviData;
}

void NaviControl::OnDataReceived(System::Object ^sender, SerialDataReceivedEventArgs ^e)
{
	String^ raw_data;
	OSDData naviData;
	Boolean dataValid;

	try
	{
		raw_data = serial->ReadLine();
		serial->ReadExisting();
	}
	catch (Exception^ e)
	{
		return;
	}

	if (raw_data->Length > 0 && raw_data[0] == '#') {
		switch (raw_data[2])
		{
		case 'O':
		{
			naviData = readOsdData(raw_data, dataValid);
			OSDDataReceived(this, naviData, dataValid);
		}
			break;
		case 'B':
		{
			FlightCTRLDataReceived(this, raw_data);
		}
			break;

		default:
			break;
		}
	}	

	
}
