/*
  Serial Event example

  When new serial data arrives, this sketch adds it to a String.
  When a newline is received, the loop prints the string and clears it.

  A good test for this is to try it with a GPS receiver that sends out
  NMEA 0183 sentences.

  NOTE: The serialEvent() feature is not available on the Leonardo, Micro, or
  other ATmega32U4 based boards.

  created 9 May 2011
  by Tom Igoe

  This example code is in the public domain.

  http://www.arduino.cc/en/Tutorial/SerialEvent
*/
#include "UnityArduinoComms.h"

// related to pulse generation
#define sin_out_put_pin 5
#define cos_out_put_pin 6

const float rad_per_deg = 0.01745329251;      // value of a radian per degree
float theta = 0;
int pulse_width_sin = 0;
int pulse_width_cos = 0;
// to control the increments or decrements of angle parameter
int sign = 1;

String inputString = "";         // a String to hold incoming data

void setup() {
  // initialize serial:
  Serial.begin(9600);
  // reserve 200 bytes for the inputString:
  inputString.reserve(200);
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.flush();
}

// this loop is constantly running in the background
void loop() {
  // TODO(gmicros): check for valid flag and do stuff

  // TODO(gmicros): reset the flag and wait

  if (output_pulse) {
    for (int i = 0; i < 200 * num_pulses; i ++) {
      analogWrite( sin_out_put_pin , pulse_width_sin );    // PWM output at the given pins
      analogWrite( cos_out_put_pin , pulse_width_cos );

      if (theta == 3.14159265359) sign = -1;   // keep increasing the value of theta till pi and the decrease till zero
      else if (theta == 0) sign = 1;

      theta = theta + (rad_per_deg * sign);

      pulse_width_sin = 255 * sin(theta);
      pulse_width_cos = 255 * cos(theta);

      pulse_width_sin = abs(pulse_width_sin);
      pulse_width_cos = abs(pulse_width_cos);

      delay(4);  // control the frequency here
    }
    output_pulse = false;
  }

  analogWrite( sin_out_put_pin , 0 );
}

/*
  SerialEvent occurs whenever a new data comes in the hardware serial RX. This
  routine is run between each time loop() runs, so using delay inside loop can
  delay response. Multiple bytes of data may be available.
*/
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:   
    //output_pulse = true; 
    if (inChar == '\n') {
      output_pulse = true;
      //Serial.write(inputString.c_str());
      bool val = parseCommandString(inputString);
      if (val) {
        //Serial.write(inputString.c_str());
        output_pulse = true;
        
      }
      inputString = "";
    }
  }
}
