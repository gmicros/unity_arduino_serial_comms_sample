# unity_arduino_serial_comms

A sample project to demonstrate communication between Unity and Arduino over serial port.

## Message Structure 
The messaging interface "$" as a start delimiter for the message, followed by the string "UNITY" and then a comma separated list of integers denoting which vibrotactor, the duration and intensity. This string is followed by the checksum of the message body, which is delimited with "\*" (star). 

## Testing

For testing and troubleshooting the Arduino code it is useful use the Serial Monitor that comes with the Arduino IDE. 

The Arduino requires the framed message to actually do anything. So try this string for testing:

`$UNITY,0,100,6,*30`

Otherwise you will get a response telling you what is wrong. 



