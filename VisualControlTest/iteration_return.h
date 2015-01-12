#ifndef _VISUAL_CONTROL_LIB_
#define _VISUAL_CONTROL_LIB_ 1

#include <iostream>
#include <vector>

typedef enum
{
	Success = 0x0000,
	BackgroundFigureMissing = 0x0001,
	WrongFigureDetected = 0x0002,
	NoLPMDetected = 0x0004,
	NoEdgeShapesDetected = 0x0008,
	UndefinedError = 0xFFFF

} iteration_state;

typedef enum
{
	TypeRectangle = 2,
	TypeCircle = 4,
	TypeTriangle  = 1,
	TypeHexagon  = 3

} figure_type;

typedef struct
{
	int start_x;
	int start_y;

	int end_x;
	int end_y;

	figure_type given_type;
	figure_type expected_type;

} iteration_wrong_detected;

typedef struct iteration_not
{
	iteration_not ()
	{
		start_x = start_y = end_x = end_y = 0;
	}
	
	int start_x;
	int start_y;

	int end_x;
	int end_y;

	figure_type expected_type;

} iteration_not_detected;

typedef struct Iteration_Return
{
	Iteration_Return()
	{
		state = Success;
		nr_of_no_detections = 0;
		nr_of_wrong_detections = 0;
	}
	// bit encoded return state 
	// there can be wrong detected figures and also not detected figures
	iteration_state state;

	// store the number of not detected figures
	int nr_of_no_detections;
	std::vector<iteration_not_detected> vector_not_detected;

	// store the number of wrong detected figures
	int nr_of_wrong_detections;
	std::vector<iteration_wrong_detected> vector_wrong_detected;

} iteration_return_t;

#endif