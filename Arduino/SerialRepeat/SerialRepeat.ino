/*
 * TODO(gmicros): rename this file and folder to something meaningful
 * eg. ExampleUnityArduinoComms.ino
  
  An example program for using UnityArduinoComms to receive commands from a Unity game
  and generate waveforms based on received parameters to actuate vibrotactors.

  When new serial data arrives, this sketch adds it to a String.
  When a newline is received the string is considered terminated. The string is parsed
  for waveform parameters which are validated. If valid the loop calls generateWaveform()
  that triggers the vibrotactors.

  NOTE: The serialEvent() feature is not available on the Leonardo, Micro, or
  other ATmega32U4 based boards.
  
*/
#include "UnityArduinoComms.h"


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
  if (output_pulse) {
    generateWaveform();
    output_pulse = false;
  }
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
    // append to input string
    inputString += inChar;
    //find the end of line
    if (inChar == '\n') {
      // parse string for a valid command
      bool val = parseCommandString(inputString);
      if (val) {
        output_pulse = true;
      }
      // clear the receive buffer
      inputString = "";
    }
  }
}
