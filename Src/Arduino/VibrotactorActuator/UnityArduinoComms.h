/**
    \file UnityArduinoComms.h
    \brief Interface for Arduino communicating with Unity
*/
#ifndef UnityArduinoComms_n
#define UnityArduinoComms_n

#include "Arduino.h"

// valid range for vibrotactors
#define MIN_VIBROTACTOR 0
#define MAX_VIBROTACTOR 4

// valid range for duration
#define MIN_DURATION 0
// max unsigned int is 2^16 and the same variable is used to 
// increment the angle so 2^16 / 360 
#define MAX_DURATION 182

// valid range for intensity
#define MIN_INTENSITY 0
#define MAX_INTENSITY 10

// pins used to output waveform
// this must match what is in C#
// NOTE: these are PWM pins. PWN pins need to be used to output varying amplitude
// the values here are the PWM pins on an arduino UNO, make sure these match your device
#define VIBROTACTOR_0_PIN 3
#define VIBROTACTOR_1_PIN 5
#define VIBROTACTOR_2_PIN 6
#define VIBROTACTOR_3_PIN 9
#define VIBROTACTOR_4_PIN 10
#define VIBROTACTOR_5_PIN 11


// flag to output pulse
extern bool output_pulse;
// number of pulses to ouput
extern int num_pulses;
// pins to ouput pulse to
extern int pulse_output_pin;
// time between samples
// TODO(gmicros): this needs units to be meaningful
extern int sample_time;

/**
    \brief Validate duration received

    \param [in] duration in milliseconds
    \return true for value within range

    \details More details
*/
bool validateDuration(int const duration);

/**
    \brief Validate intensity received

    \param [in] intensity of vibration
    \return true for value within range

    \details More details
*/
bool validateIntensity(int const intensity);

bool validateVibrotactor(int const vibrotactor);

int parseIntString(String const str);

// $UNITY,0,100,6,*30
bool parseCommandString(String command);

void generateWaveform();

#endif
