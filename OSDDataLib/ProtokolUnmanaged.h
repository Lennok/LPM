#ifndef PROTOCOL_H
#define PROTOCOL_H

#include <stdint.h>
#pragma pack(push, 1) // exact fit - no padding
struct Data3D
{
	int16_t AngleNick;  // in 0.1 deg
	int16_t AngleRoll;   // in 0.1 deg
	int16_t Heading;    // in 0.1 deg
	uint8_t StickNick;
	uint8_t StickRoll;
	uint8_t StickYaw;
	uint8_t StickGas;
	uint8_t reserve[4];
};


typedef struct
{
	uint8_t      Digital[2];
	uint8_t      RemoteButtons;
	int8_t      Nick;
	int8_t      Roll;
	int8_t      Yaw;
	uint8_t      Gas;
	int8_t      Height;
	uint8_t      free;
	uint8_t      Frame;
	uint8_t      Config;
} ExternControl;

typedef struct
{
	uint16_t Distance;                                   // distance to target in dm
	int16_t  Bearing;                                    // course to target in deg
} GPS_PosDev_t;

typedef struct
{
	int32_t Longitude;  // in 1E-7 deg
	int32_t Latitude;   // in 1E-7 deg
	int32_t Altitude;   // in mm
	uint8_t Status;// validity of data
} GPS_Pos_t;


#define NAVIDATA_VERSION 5

typedef struct
{
	uint8_t Version;                                     // version of the data structure
	GPS_Pos_t CurrentPosition;                          // see ubx.h for details
	GPS_Pos_t TargetPosition;
	GPS_PosDev_t TargetPositionDeviation;
	GPS_Pos_t HomePosition;
	GPS_PosDev_t HomePositionDeviation;
	uint8_t  WaypointIndex;                              // index of current waypoints running from 0 to WaypointNumber-1
	uint8_t  WaypointNumber;                             // number of stored waypoints
	uint8_t  SatsInUse;                                  // number of satellites used for position solution
	int16_t  Altimeter;                                  // hight according to air pressure
	int16_t  Variometer;                                 // climb(+) and sink(-) rate
	uint16_t FlyingTime;                                 // in seconds
	uint8_t  UBat;                                       // Battery Voltage in 0.1 Volts
	uint16_t GroundSpeed;                                // speed over ground in cm/s (2D)
	int16_t  Heading;                                    // current flight direction in ° as angle to north
	int16_t  CompassHeading;                             // current compass value in °
	int8_t   AngleNick;                                  // current Nick angle in 1°
	int8_t   AngleRoll;                                  // current Rick angle in 1°
	uint8_t  RC_Quality;                                 // RC_Quality
	uint8_t  FCStatusFlags;                              // Flags from FC
	uint8_t  NCFlags;                                    // Flags from NC
	uint8_t  Errorcode;                                  // 0 --> okay
	uint8_t  OperatingRadius;                            // current operation radius around the Home Position in m
	int16_t  TopSpeed;                                   // velocity in vertical direction in cm/s
	uint8_t  TargetHoldTime;                             // time in s to stay at the given target, counts down to 0 if target has been reached
	uint8_t  FCStatusFlags2;                             // StatusFlags2 (since version 5 added)
	int16_t  SetpointAltitude;                           // setpoint for altitude
	uint8_t  Gas;                                        // for future use
	uint16_t Current;                                    // actual current in 0.1A steps
	uint16_t UsedCapacity;                               // used capacity in mAh
} NaviData;



#define NC_FLAG_FREE                    0x01
#define NC_FLAG_PH                              0x02
#define NC_FLAG_CH                              0x04
#define NC_FLAG_RANGE_LIMIT             0x08
#define NC_FLAG_NOSERIALLINK    0x10
#define NC_FLAG_TARGET_REACHED  0x20
#define NC_FLAG_MANUAL_CONTROL  0x40
#define NC_FLAG_GPS_OK                  0x80
#pragma pack(pop) //back to whatever the previous packing mode was 
#endif // PROTOCOL_H
