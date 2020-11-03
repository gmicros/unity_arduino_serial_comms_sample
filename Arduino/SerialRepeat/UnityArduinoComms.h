/**
 *  \file UnityArduinoComms.h
 *  \brief Interface for Arduino communicating with Unity
 */
#ifndef UnityArduinoComms_n
#define UnityArduinoComms_n

#include "Arduino.h"

#define MIN_VIBROTACTOR 0
#define MAX_VIBROTACTOR 4
#define MIN_DURATION 0
#define MAX_DURATION 1000
#define MIN_INTENSITY 0
#define MAX_INTENSITY 10
extern bool output_pulse;


/**
 *  \brief Validate duration received
 *  
 *  \param [in] duration in milliseconds
 *  \return true for value within range
 *  
 *  \details More details
 */
bool validateDuration(int const duration);

/**
 *  \brief Validate intensity received
 *  
 *  \param [in] intensity of vibration
 *  \return true for value within range
 *  
 *  \details More details
 */
bool validateIntensity(int const intensity);

bool validateVibrotactor(int const vibrotactor);

int parseIntString(String const str);

// $UNITY,0,100,6,*30
bool parseCommandString(String command);

#endif
