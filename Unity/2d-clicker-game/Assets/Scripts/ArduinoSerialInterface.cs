using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text;

namespace gmicros_arduino_interface
{
    public enum Vibrotactor
    {
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }
    
    public static class ArduinoSerialInterface
    {
        private static bool init = false;
        private static SerialPort port_ = null;
        private static string identifier = "UNITY";
        private static char start_delimiter_ = '$';
        private static char checksum_delimiter_ = '*';
        private static char field_delimiter_ = ',';

        #region public interface
        public static void Init()
        {
            if (port_ == null)
            {
                Debug.Log("Initializing ArduinoInterface");
                port_ = new SerialPort();
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
            if(!init)
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
                        Debug.Log("resp: " + resp);
                    }
                    catch (System.TimeoutException)
                    {
                        Debug.LogError("Read timeout");
                    }
                }
                catch (System.TimeoutException)
                {
                    Debug.LogError("Write timeout");
                }
            }
            return false;
        }

        public static string FillMessage(Vibrotactor vib, int duration, int intensity)
        {
            string msg_body = FillMessageBody(vib, duration, intensity);
            string msg = AddHeaderAndChecksum(msg_body);
            return msg;
        }
        #endregion

        private static string FillMessageBody(Vibrotactor vib, int duration, int intensity)
        {
            string msg_body = "";
            msg_body += identifier + field_delimiter_;
            msg_body += vib.ToString() + field_delimiter_;
            msg_body += duration.ToString() + field_delimiter_;
            msg_body += intensity.ToString() + field_delimiter_;
            return msg_body;
        }

        private static string AddHeaderAndChecksum(string msg_body)
        {
            int check = checksum(msg_body);
            string msg = start_delimiter_ + msg_body + checksum_delimiter_ + check.ToString() + System.Environment.NewLine;
            return msg;
        }

        private static int checksum(string msg)
        {
            byte[] msg_bytes = Encoding.ASCII.GetBytes(msg);
            int checksum = 0;
            foreach(byte c in msg_bytes) {
                checksum ^= c;
            }
            return checksum;
        }

        #region testing code
        public static string FillTestMessage()
        {
            string msg = "GPRMC,092751.000,A,5321.6802,N,00630.3371,W,0.06,31.66,280511,,,A";
            msg = AddHeaderAndChecksum(msg);
            return msg;
        }
        #endregion

    }
}
