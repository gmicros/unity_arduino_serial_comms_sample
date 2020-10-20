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
#define MIN_VIBROTACTOR 0
#define MAX_VIBROTACTOR 4
#define MIN_DURATION 0
#define MAX_DURATION 1000
#define MIN_INTENSITY 0
#define MAX_INTENSITY 10


#define LED_DELAY 200
String inputString = "";         // a String to hold incoming data
bool stringComplete = false;  // whether the string is complete

String identifier = "UNITY";
String start_delimiter = "$";
String field_delimiter = ",";
String checksum_delimiter = "*";

void setup() {
  // initialize serial:
  Serial.begin(9600);
  // reserve 200 bytes for the inputString:
  inputString.reserve(200);
  pinMode(LED_BUILTIN, OUTPUT);
}

// this loop is constantly running in the background 
void loop() {
  // TODO(gmicros): check for valid flag and do stuff

  // TODO(gmicros): reset the flag and wait
}

bool validateDuration(int const duration){
  if(duration < MIN_DURATION || duration > MAX_DURATION) {
    return false;
  }

  return true;
}

bool validateIntensity(int const intensity) {
  if(intensity < MIN_INTENSITY || intensity > MAX_INTENSITY) {
    return false;
  }
  return true;
}

bool validateVibrotactor(int const vibrotactor) {
  // check valid value
  if(vibrotactor < MIN_VIBROTACTOR || vibrotactor > MAX_VIBROTACTOR ) {
    return false; 
  }

  switch(vibrotactor){
    case 0:
    // front vib
    break;

    case 1:
    // left vib
    break;

    case 2:
    // back vib
    break;

    case 3:
    // right vib
    break;
  }
  return true;
}

int parseIntString(String const str){
  if(str == ""){
    return -1;
  }

  int const val = atoi(str.c_str());
  if(val == 0 && str != "0") {
    // error in parsing 
    return -1;
  }
  return val;
}

// $UNITY,0,100,6,*30
bool parseCommandString(String command){
  int vibrotactor = -1;
  int duration = -1;
  int intensity = -1;

  // no start delimiter - early exit
  if(command.substring(0,1) != start_delimiter){
    Serial.write("could not find start_delimiter\n");    
    return false;
  }

  // no identifier - early exit
  if(command.substring(1,6) != identifier){
    Serial.write("could not find id\n");    
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
  while( (str = strtok_r(p, ",",  &p)) != NULL){
    // parse valid strings
    String const token = String(str);
    int const token_val = parseIntString(str);
    

    // keep track of the token position 
    switch(num_tokens)
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
                
        if (!validateDuration(token_val)){
          Serial.write("invalid duration\n");
          return false;
        }
        duration = token_val;
        break;
      }

      // intensity
      case 2:
      {
          if(!validateIntensity(token_val)){
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
        int checksum = atoi(str+1);
        // TODO(gmicros): validate checksum
        break;
      }
    } 
    
    num_tokens++;
  }

  //TODO(gmicros): verify checksum 

  //TODO(gmicros): command vibrotactor 

  // Debug output
  String resp ="";
  resp += " vib[ " + String(vibrotactor) + " ] ";
  resp += " intes[ " + String(intensity) + " ] ";
  resp += " dur [ " + String(duration) + " ] \n";
  Serial.println(resp.c_str());  

  return false;
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
    if (inChar == '\n') {
      stringComplete = true;
      bool val = parseCommandString(inputString);
      if(val){
        Serial.write(inputString.c_str());
      }
      inputString = "";
    }
  }
}
