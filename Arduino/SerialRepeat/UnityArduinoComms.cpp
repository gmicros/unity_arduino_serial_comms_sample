#include "UnityArduinoComms.h"


String identifier = "UNITY";
String start_delimiter = "$";
String field_delimiter = ",";
String checksum_delimiter = "*";

// TODO(gmicros): hardcoded for testing
bool output_pulse = false;
int num_pulses = 5;
int pulse_output_pin = 5;
int sample_time = 20;

bool validateDuration(int const duration) {
  if (duration < MIN_DURATION || duration > MAX_DURATION) {
    return false;
  }
  num_pulses = duration;

  return true;
}

bool validateIntensity(int const intensity) {
  if (intensity < MIN_INTENSITY || intensity > MAX_INTENSITY) {
    return false;
  }
  return true;
}

bool validateVibrotactor(int const vibrotactor) {
  // check valid value
  if (vibrotactor < MIN_VIBROTACTOR || vibrotactor > MAX_VIBROTACTOR ) {
    return false;
  }

  switch (vibrotactor) {
    case 0:
      // front vib
      //num_pulses = 1;
      break;

    case 1:
      // left vib
      //num_pulses = 2;
      break;

    case 2:
      // back vib
      //num_pulses = 3;
      break;

    case 3:
      // right vib
      //num_pulses = 4;
      break;
  }
  return true;
}

int parseIntString(String const str) {
  if (str == "") {
    return -1;
  }

  int const val = atoi(str.c_str());
  if (val == 0 && str != "0") {
    // error in parsing
    return -1;
  }
  return val;
}

// $UNITY,0,100,6,*30
bool parseCommandString(String command) {
  int vibrotactor = -1;
  int duration = -1;
  int intensity = -1;

  // no start delimiter - early exit
  if (command.substring(0, 1) != start_delimiter) {
    Serial.println("could not find start_delimiter\n\r");
    return false;
  }

  // no identifier - early exit
  if (command.substring(1, 6) != identifier) {
    Serial.println("could not find id\n\r");
    return false;
  }

  // TODO(gmicros): do the checksum early and exit if invalid;

  char buf[200];
  command.toCharArray(buf, sizeof(buf));
  char *p = &buf[7];
  char* str;
  int num_tokens = 0;

  // parse on "," delimiter
  // TODO(gmicros): make this handle only the message body
  while ( (str = strtok_r(p, ",",  &p)) != NULL) {
    // parse valid strings
    String const token = String(str);
    int const token_val = parseIntString(str);


    // keep track of the token position
    switch (num_tokens)
    {
      // vibrotactor
      case 0:
        {
          if (!validateVibrotactor(token_val) ) {
            Serial.write("invalid vib value\n");
            return false;
          }
          vibrotactor = token_val;
          break;
        }

      // duration
      case 1:
        {

          if (!validateDuration(token_val)) {
            Serial.write("invalid duration\n");
            return false;
          }
          duration = token_val;
          break;
        }

      // intensity
      case 2:
        {
          if (!validateIntensity(token_val)) {
            Serial.write("invalid intensity\n");
            return false;
          }
          intensity = token_val;
          break;
        }

      // checksum
      case 3:
        {
          // checksum
          int checksum = atoi(str + 1);
          // TODO(gmicros): validate checksum
          break;
        }
    }

    num_tokens++;
  }

  //TODO(gmicros): verify checksum

  //TODO(gmicros): command vibrotactor
  output_pulse = true;
  // Debug output
  String resp = "";
  resp += " vib[ " + String(vibrotactor) + " ] ";
  resp += " intes[ " + String(intensity) + " ] ";
  resp += " dur [ " + String(duration) + " ] \n\r";
  Serial.println(resp);

  return false;
}

void generateWaveform() {
  // TODO(gmicros): figure out if we need args or just use globals
  // TODO(gmicros): add all the pulse gen and output code here

  const float rad_per_deg = 0.01745329251;      // value of a radian per degree
  float theta = 0;
  int pulse_width_sin = 127;
  for (unsigned int i = 0; i < 360 * num_pulses; i ++) {
    analogWrite( pulse_output_pin , pulse_width_sin );    // PWM output at the given pins

    theta = theta + rad_per_deg;

    pulse_width_sin = 127 * sin(theta) + 128;

    delayMicroseconds(sample_time);  // control the frequency here
  }

  analogWrite( pulse_output_pin, 0 );
}
