using System;
using System.Text;
using System.IO.Ports;

namespace UnityArduinoComms
{
    public static class ArduinoSerialInterface
    {
        private static bool init = false;
        private static SerialPort port_ = null;
        public static void Init()
        {
            if (port_ == null)
            {
                //Debug.Log("Initializing ArduinoInterface");
                port_ = new SerialPort();
                // TODO(gmicros): this should not be hardcoded
                port_.PortName = "COM5";
                port_.BaudRate = 9600;
                port_.Parity = Parity.None;
                port_.DataBits = 8;
                port_.StopBits = StopBits.One;
                port_.ReadTimeout = 500;
                port_.WriteTimeout = 500;
                port_.Open();
                init = true;
            }
        }

        public static bool SendMessage(string message)
        {
            if (!init)
            {
                Init();
            }

            if (port_.IsOpen)
            {
                try
                {
                    port_.Write(message);
                    try
                    {
                        string resp = port_.ReadLine();
                        //Debug.Log("resp: " + resp);
                    }
                    catch (System.TimeoutException)
                    {
                        //Debug.LogError("Read timeout");
                    }
                }
                catch (System.TimeoutException)
                {
                    //Debug.LogError("Write timeout");
                }
            }
            return false;
        }        
    }
}
