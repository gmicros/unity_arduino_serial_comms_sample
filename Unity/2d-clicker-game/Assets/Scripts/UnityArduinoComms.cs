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

        public static string SendMessage(string message)
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
                        return resp;
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
            return "";
        }
    }

    public enum Vibrotactor
    {
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }

    static class MessageStructure
    {
        internal static string identifier = "UNITY";
        internal static char start_delimiter_ = '$';
        internal static char checksum_delimiter_ = '*';
        internal static char field_delimiter_ = ',';
    }

    public class MessageUtils
    {
        public static string FillMessage(Vibrotactor vib, int duration, int intensity)
        {
            string msg_body = FillMessageBody(vib, duration, intensity);
            string msg = MessageUtils.AddHeaderAndChecksum(msg_body);
            return msg;
        }

        internal static string FillMessageBody(Vibrotactor vib, int duration, int intensity)
        {
            string msg_body = "";
            msg_body += MessageStructure.identifier + MessageStructure.field_delimiter_;
            msg_body += vib.ToString() + MessageStructure.field_delimiter_;
            msg_body += duration.ToString() + MessageStructure.field_delimiter_;
            msg_body += intensity.ToString() + MessageStructure.field_delimiter_;
            return msg_body;
        }

        public static string FillTestMessage()
        {
            string msg = "GPRMC,092751.000,A,5321.6802,N,00630.3371,W,0.06,31.66,280511,,,A";
            //string msg = "GPGGA,092750.000,5321.6802,N,00630.3372,W,1,8,1.03,61.7,M,55.2,M,,";
            int check = MessageUtils.checksum(msg.Trim());
            msg += MessageStructure.checksum_delimiter_ + check.ToString("X2");
            return msg;
        }
        internal static string AddHeaderAndChecksum(string msg_body)
        {
            int check = checksum(msg_body);
            string msg = MessageStructure.start_delimiter_ + msg_body + MessageStructure.checksum_delimiter_ + check.ToString() + System.Environment.NewLine;
            return msg;
        }

        internal static int checksum(string msg)
        {
            byte[] msg_bytes = Encoding.ASCII.GetBytes(msg);
            int checksum = 0;
            foreach (byte c in msg_bytes)
            {
                checksum ^= c;
            }
            return checksum;
        }
    }
}