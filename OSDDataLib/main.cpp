#using <System.dll>

#include "ProtokolUnmanaged.h"

#include <vector>
#include <algorithm>
#include <iostream>
#include <string>
#include <ctime>
#include <fstream>
#include <array>
#include <sstream>

using namespace System;
using namespace System::IO::Ports;
using namespace System::Threading;
//using namespace std;


typedef std::vector< unsigned char > mk_data;
typedef unsigned char u_char;


void calcCRC(String^ message, u_char &CRC1, u_char &CRC2) {
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

String^ generateMsg(u_char cmd, u_char address, mk_data &data) {
	String^ message= gcnew String("");
	
	message+="#";
	message+=Convert::ToChar('a' + address);
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

bool parseMessage(String^ message, u_char &cmd, u_char &address, mk_data &data) {
	if (message->Length < 6) {
		return false;
	}
	if (message[0] != '#') {
		return false;
	}

	u_char CRC1;
	u_char CRC2;
	String^ crcString = message->Substring(0, message->Length - 2);	
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


bool readOsdData(String^ msg, NaviData &lastNaviData){
	u_char cmd;
	u_char address = 0;
	mk_data data (sizeof(NaviData));

	memset(&data[0], 0, sizeof(NaviData));
	memset(&lastNaviData, 0, sizeof(NaviData));

	if (parseMessage(msg, cmd, address, data))
	{
		memcpy(&lastNaviData, &data[0], sizeof(NaviData));
		return true;
	}
	else {
		return false;
	}
}

void logToConsole(NaviData &lastNaviData) {

	// current date/time based on current system
	time_t now = time(0);
	// convert now to string form
	char* dt = ctime(&now);
	//Console::WriteLine("Time: " + dt.T);
//	std::cout << (currentDateTime()).toString();
	system("cls");
	Console::WriteLine("lastNaviData.Version=" +lastNaviData.Version.ToString());
	Console::WriteLine("lastNaviData.CurrentPosition.Longitude =" + lastNaviData.CurrentPosition.Longitude.ToString());
	Console::WriteLine("lastNaviData.CurrentPosition.Latitude =" + lastNaviData.CurrentPosition.Latitude.ToString());
	Console::WriteLine("lastNaviData.CurrentPosition.Altitude =" + lastNaviData.CurrentPosition.Altitude.ToString());
	Console::WriteLine("lastNaviData.CurrentPosition.Status =" + lastNaviData.CurrentPosition.Status.ToString());
	Console::WriteLine("lastNaviData.TargetPosition.Longitude =" + lastNaviData.TargetPosition.Longitude.ToString());
	Console::WriteLine("lastNaviData.TargetPosition.Latitude =" + lastNaviData.TargetPosition.Latitude.ToString());
	Console::WriteLine("lastNaviData.TargetPosition.Altitude =" + lastNaviData.TargetPosition.Altitude.ToString());
	Console::WriteLine("lastNaviData.TargetPosition.Status =" + lastNaviData.TargetPosition.Status.ToString());
	Console::WriteLine("lastNaviData.TargetPositionDeviation.Distanc =" + lastNaviData.TargetPositionDeviation.Distance.ToString());
	Console::WriteLine("lastNaviData.TargetPositionDeviation.Bearing =" + lastNaviData.TargetPositionDeviation.Bearing.ToString());
	Console::WriteLine("lastNaviData.HomePosition.Longitude =" + lastNaviData.HomePosition.Longitude.ToString());
	Console::WriteLine("lastNaviData.HomePosition.Latitude =" + lastNaviData.HomePosition.Latitude.ToString());
	Console::WriteLine("lastNaviData.HomePosition.Altitude =" + lastNaviData.HomePosition.Altitude.ToString());
	Console::WriteLine("lastNaviData.HomePosition.Status =" + lastNaviData.HomePosition.Status.ToString());
	Console::WriteLine("lastNaviData.HomePositionDeviation.Distance =" + lastNaviData.HomePositionDeviation.Distance.ToString());
	Console::WriteLine("lastNaviData.HomePositionDeviation.Bearin =" + lastNaviData.HomePositionDeviation.Bearing.ToString());
	Console::WriteLine("lastNaviData.WaypointIndex =" + lastNaviData.WaypointIndex.ToString());
	Console::WriteLine("lastNaviData.WaypointNumber =" + lastNaviData.WaypointNumber.ToString());
	Console::WriteLine("lastNaviData.SatsInUse =" + lastNaviData.SatsInUse.ToString());
	Console::WriteLine("lastNaviData.Altimeter =" + lastNaviData.Altimeter.ToString());
	Console::WriteLine("lastNaviData.Variometer =" + lastNaviData.Variometer.ToString());
	Console::WriteLine("lastNaviData.FlyingTime =" + lastNaviData.FlyingTime.ToString());
	Console::WriteLine("lastNaviData.UBat =" + lastNaviData.UBat.ToString());
	Console::WriteLine("lastNaviData.GroundSpeed =" + lastNaviData.GroundSpeed.ToString());
	Console::WriteLine("lastNaviData.Heading =" + lastNaviData.Heading.ToString());
	Console::WriteLine("lastNaviData.CompassHeading =" + lastNaviData.CompassHeading.ToString());
	Console::WriteLine("lastNaviData.AngleNick =" + lastNaviData.AngleNick.ToString());
	Console::WriteLine("lastNaviData.AngleRoll =" + lastNaviData.AngleRoll.ToString());
	Console::WriteLine("lastNaviData.RC_Quality =" + lastNaviData.RC_Quality.ToString());
	Console::WriteLine("lastNaviData.FCStatusFlags =" + lastNaviData.FCStatusFlags.ToString());
	Console::WriteLine("lastNaviData.NCFlags =" + lastNaviData.NCFlags.ToString());
	Console::WriteLine("lastNaviData.Errorcode =" + lastNaviData.Errorcode.ToString());
	Console::WriteLine("lastNaviData.OperatingRadius =" + lastNaviData.OperatingRadius.ToString());
	Console::WriteLine("lastNaviData.TopSpeed =" + lastNaviData.TopSpeed.ToString());
	Console::WriteLine("lastNaviData.TargetHoldTime =" + lastNaviData.TargetHoldTime.ToString());
	Console::WriteLine("lastNaviData.FCStatusFlags2 =" + lastNaviData.FCStatusFlags2.ToString());
	Console::WriteLine("lastNaviData.SetpointAltitude =" + lastNaviData.SetpointAltitude.ToString());
	Console::WriteLine("lastNaviData.Gas =" + lastNaviData.Gas.ToString());
	Console::WriteLine("lastNaviData.Current =" + lastNaviData.Current.ToString());
	Console::WriteLine("lastNaviData.UsedCapacity =" + lastNaviData.UsedCapacity.ToString());
	Console::WriteLine("\n\n" + "================================");


	

}

void logToFile(NaviData &lastNaviData, std::ofstream &out) {

	out.open("out.txt", std::ofstream::out);
	// current date/time based on current system
	//time_t now = time(0);
	//// convert now to string form
	//char* dt = ctime(&now);
	//out << "Time: " + dt << std::endl;
	//out << "lastNaviData.Version=" + lastNaviData.Version << "\n";
	//out << "lastNaviData.CurrentPosition.Longitude =" + lastNaviData.CurrentPosition.Longitude << "\n";
	//out << "lastNaviData.CurrentPosition.Latitude =" + lastNaviData.CurrentPosition.Latitude << "\n";
	//out << "lastNaviData.CurrentPosition.Altitude =" + lastNaviData.CurrentPosition.Altitude << "\n";
	//out << "lastNaviData.CurrentPosition.Status =" + lastNaviData.CurrentPosition.Status << "\n";
	//out << "lastNaviData.TargetPosition.Longitude =" + lastNaviData.TargetPosition.Longitude << "\n";
	//out << "lastNaviData.TargetPosition.Latitude =" + lastNaviData.TargetPosition.Latitude << "\n";
	//out << "lastNaviData.TargetPosition.Altitude =" + lastNaviData.TargetPosition.Altitude << "\n";
	//out << "lastNaviData.TargetPosition.Status =" + lastNaviData.TargetPosition.Status << "\n";
	//out << "lastNaviData.TargetPositionDeviation.Distanc =" + lastNaviData.TargetPositionDeviation.Distance << "\n";
	//out << "lastNaviData.TargetPositionDeviation.Bearing =" + lastNaviData.TargetPositionDeviation.Bearing << "\n";
	//out << "lastNaviData.HomePosition.Longitude =" + lastNaviData.HomePosition.Longitude << "\n";
	//out << "lastNaviData.HomePosition.Latitude =" + lastNaviData.HomePosition.Latitude << "\n";
	//out << "lastNaviData.HomePosition.Altitude =" + lastNaviData.HomePosition.Altitude << "\n";
	//out << "lastNaviData.HomePosition.Status =" + lastNaviData.HomePosition.Status << "\n";
	//out << "lastNaviData.HomePositionDeviation.Distance =" + lastNaviData.HomePositionDeviation.Distance << "\n";
	//out << "lastNaviData.HomePositionDeviation.Bearin =" + lastNaviData.HomePositionDeviation.Bearing << "\n";
	//out << "lastNaviData.WaypointIndex =" + lastNaviData.WaypointIndex << "\n";
	//out << "lastNaviData.WaypointNumber =" + lastNaviData.WaypointNumber << "\n";
	//out << "lastNaviData.SatsInUse =" + lastNaviData.SatsInUse << "\n";
	//out << "lastNaviData.Altimeter =" + lastNaviData.Altimeter << "\n";
	//out << "lastNaviData.Variometer =" + lastNaviData.Variometer << "\n";
	//out << "lastNaviData.FlyingTime =" + lastNaviData.FlyingTime << "\n";
	//out << "lastNaviData.UBat =" + lastNaviData.UBat << "\n";
	//out << "lastNaviData.GroundSpeed =" + lastNaviData.GroundSpeed << "\n";
	//out << "lastNaviData.Heading =" + lastNaviData.Heading << "\n";
	//out << "lastNaviData.CompassHeading =" + lastNaviData.CompassHeading << "\n";
	//out << "lastNaviData.AngleNick =" + lastNaviData.AngleNick << "\n";
	//out << "lastNaviData.AngleRoll =" + lastNaviData.AngleRoll << "\n";
	//out << "lastNaviData.RC_Quality =" + lastNaviData.RC_Quality << "\n";
	//out << "lastNaviData.FCStatusFlags =" + lastNaviData.FCStatusFlags << "\n";
	//out << "lastNaviData.NCFlags =" + lastNaviData.NCFlags << "\n";
	//out << "lastNaviData.Errorcode =" + lastNaviData.Errorcode << "\n";
	//out << "lastNaviData.OperatingRadius =" + lastNaviData.OperatingRadius << "\n";
	//out << "lastNaviData.TopSpeed =" + lastNaviData.TopSpeed << "\n";
	//out << "lastNaviData.TargetHoldTime =" + lastNaviData.TargetHoldTime << "\n";
	//out << "lastNaviData.FCStatusFlags2 =" + lastNaviData.FCStatusFlags2 << "\n";
	//out << "lastNaviData.SetpointAltitude =" + lastNaviData.SetpointAltitude << "\n";
	//out << "lastNaviData.Gas =" + lastNaviData.Gas << "\n";
	//out << "lastNaviData.Current =" + lastNaviData.Current << "\n";
	//out << "lastNaviData.UsedCapacity =" + lastNaviData.UsedCapacity << "\n";
	//out << "================================" + "\n\n";

	out.close();


}

int init(void)
{
		SerialPort^ serial = gcnew SerialPort();
		NaviData lastNaviData;
		serial->PortName = "COM3";
		//serial.SetPortName("COM3");
		//serial.setBaudRate(QSerialPort::Baud57600);
		serial->BaudRate = 57600;
		//serial.setDataBits(QSerialPort::Data8);
		serial->DataBits = 8;
		//serial.setParity(QSerialPort::NoParity);
		serial->Parity = Parity::None;
		//serial.setStopBits(QSerialPort::OneStop);
		serial->StopBits = StopBits::One;
	
		serial->NewLine = "\r";
	
		serial->ReadTimeout = 500;
		serial->WriteTimeout = 500;
		serial->Open();
	
		//is_open = serial.open(QIODevice::ReadWrite);
		if (serial->IsOpen) {
			return 0;
		}
		else
		{
			return -1;
		}
}

int main(void)
{
	std::ofstream out("out.txt");
	NaviData lastNaviData;
	String^ data= gcnew String("#cO>M======wWMrSCuD==A==================================================]======d=====?D=Ly=ny=N=Le========R>M=t>M==vI");

	if (readOsdData(data, lastNaviData)) {
				logToConsole(lastNaviData);
				logToFile(lastNaviData, out);
			}
}


//int main(int argc, char *argv[])
//{
//	std::ofstream out("out.txt");
//	//file.open(QIODevice::WriteOnly | QIODevice::Text);
//	//QTextStream out(&file);
//
//	SerialPort^ serial = gcnew SerialPort();
//		;
//
//	NaviData lastNaviData;
//	serial->PortName = "COM3";
//	//serial.SetPortName("COM3");
//	//serial.setBaudRate(QSerialPort::Baud57600);
//	serial->BaudRate = 57600;
//	//serial.setDataBits(QSerialPort::Data8);
//	serial->DataBits = 8;
//	//serial.setParity(QSerialPort::NoParity);
//	serial->Parity = Parity::None;
//	//serial.setStopBits(QSerialPort::OneStop);
//	serial->StopBits = StopBits::One;
//
//	serial->NewLine = "\r";
//
//	serial->ReadTimeout = 500;
//	serial->WriteTimeout = 500;
//	serial->Open();
//
//	//is_open = serial.open(QIODevice::ReadWrite);
//	if (serial->IsOpen) {
//		
//
//		std::cout << "Port is ready to reading data";
//
//
//		//array<Byte> raw_data = gcnew array< Byte >(255); 
//		String^ raw_data;
//		String^ data = gcnew String("");
//		//array<Byte> data(255);
//
//		mk_data temp = { 10, 171,171 };
//		String^ text = generateMsg('o', 1, temp);
//		//serial.WriteLine(generateMsg('o', 1, temp));
//		serial->Write(text);
//		//serial.write(generateMsg('o', 1, temp).toLatin1());
//
//		int i = 0;
//		int blankCount = 0;
//
//		while (true) {
//			//Thread::Sleep(250);
//			try
//			{				
//				raw_data = serial->ReadLine();
//				
//					//serial.waitForReadyRead(100);
//					//raw_data = serial.readLine();
//				if (raw_data[0] == '#') {
//					++i;
//					u_char cmd;
//					u_char addr;
//					mk_data sensorsData;
//					
//					String^ stringData(data);
//					if (parseMessage(stringData, cmd, addr, sensorsData)) {
//						Console::WriteLine("packet #" + i + " -> ");
//						if (readOsdData(data, lastNaviData)) {
//							logToConsole(lastNaviData);
//							logToFile(lastNaviData, out);
//						}
//					}
//					data = String::Empty;
//				}
//				data += raw_data;
//				if (raw_data->Length == 0) {
//					blankCount++;
//				}
//				if (blankCount > 5) {
//					blankCount = 0;
//					serial->WriteLine(generateMsg('o', 1, temp));
//				}
//			}
//			catch (TimeoutException ^) {
//				Console::WriteLine("Timeout");
//			}
//
//			
//		}
//
//	}
//	return 0;
//}

