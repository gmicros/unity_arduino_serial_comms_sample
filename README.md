# unity_arduino_serial_comms

A library to communicate messages from Unity to Arduino. 

The communication is done over serial port with an Arduino connected with USB to the computer running the Unity app. 

The library contains a component for Unity and Arduino that can be easily included into other projects. The messages contain commands to actuate vibrotactors. 

Sample projects are included to demonstrate the communication.

## Library Structure

The library source code is contained in the Src directory. The Unity and Arduino directories contain the respective components of the library. 

### Unity 

The Unity part of the library consits of UnityArduinoComms.cs. 
Copy this file into the Assets/Scripts directory in your Unity project. 

In the scripts that you want to use the library add the line: 
`using UnityArduinoComms;`

In the callback that will invoke the library include:
```
ArduinoSerialInterface.Init();
string msg = MessageUtils.FillMessage(vibro, duration, intensity);
string resp = ArduinoSerialInterface.SendMessage(msg);
```
This will initialize the interface, if it has not been already. 
The parameters are passed to construct the framed message. 
The resulting framed message is sent. 
### Arduino

Under Src/Arduino/VibrotactorActuator. 
The Arduino part of the library is contained in UnityArduinoComms.h and UnityArduinoComms.cpp. 
These files can be added to the Arduino libraries directory (this is a system directory) or can be added to the desired project. 

There are two calls that this library exposes:
`parseCommandString(received_message)` that parses the framed message for the contained parameters. 
`generateWaveform()` that produces a sinusoid of the desired parameters at the specified output pin. 

### Configuration Details

In order to changes the pins that are used by Arduino changes must be made in the library for both Unity and Arduino. 

In Unity/UnityArduinoComms.cs the enumration Vibrotactor defines the mapping and naming of the enumration. The name itself is not important, only the value and that it maps to a corresponding value on the Arduino side. 

In Arduino/UnityArduinoComms.h the #defines starting with VIBROTACTOR define the pins that map to the each enumration. 

Following the naming convention used will help maintain consistency. So that a 0 unity maps to a 0 in arduino which output on the pin defined by VIBROTACTOR\_0.


## Sample Applications

As part of the project are apps provided to demonstrate how the library is used. 

The UnitySampleApp is a simple 2D Unity game with widgets that trigger a message to be send to Arduino. 

Under Src/Arduino/VibrotactorActutor, VibrotactorActuator.ino is a simple sketch that receives messages over the serial port and parse the values to generate the desired waveform. This sketch can be used as is or as a good starting point for a project that actuate vibrotactors. 


## Message Structure 
The messaging interface "$" as a start delimiter for the message, followed by the string "UNITY" and then a comma separated list of integers denoting which vibrotactor, the duration and intensity. This string is followed by the checksum of the message body, which is delimited with "\*" (star). 

## Testing

For testing and troubleshooting the Arduino code it is useful use the Serial Monitor that comes with the Arduino IDE. The Arduino program requires the framed message to actually do anything. So try this string for testing:

`$UNITY,0,100,6,*30`

Otherwise you will get a response telling you what is wrong. 

Also the Unity "2d-clicker-game" is an example game that uses the library to send messages when a series of buttons are pressed. 

## CIRCUIT DETAILS 

The Arduino does not drive the vibrotactor directly. The signal from the Arduino feeds into a common collector amplifier using an NPN transistor. 



