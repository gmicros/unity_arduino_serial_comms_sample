using System;
using System.Text;
using System.IO.Ports;

/// <summary>
/// defines classes for serial communication between Unity and Arduino
/// </summary>
namespace UnityArduinoComms
{
    /// <summary>
    /// API class to communicate with Arduino
    /// </summary>
    public static class ArduinoSerialInterface
    {
        /// <summary>
        /// serial port object used for communication
        /// </summary>
        private static SerialPort port_ = null;
        /// <summary>
        /// initialization flag to track object state
        /// </summary>
        private static bool init = false;

        /// <summary>
        /// Init() creates a new port and initialize it's properties for communication over the serial port
        /// </summary>
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

        // TODO(gmicros): there needs to be a way to close the port, destructor/cleanup

        /// <summary>
        /// Send a framed message over the serial port to the Arduino
        /// </summary>
        /// <param name="message">a string containing a framed message</param>
        /// <returns>the response</returns>
        public static string SendMessage(string message)
        {
            // init uninitialized object
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
                        // read the respose
                        // TODO(gmicros): handle the reponse, check for valid
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

    /// <summary>
    /// Vibrotactor enumeration class
    /// </summary>
    public enum Vibrotactor
    {
        Front = 0,
        Left = 1,
        Back = 2,
        Right = 3,
        Top = 4,
        Bottom =5
    }

    /// <summary>
    /// tokens used in message framing
    /// </summary>
    static class MessageStructure
    {
        internal static string identifier = "UNITY";
        internal static char start_delimiter_ = '$';
        internal static char checksum_delimiter_ = '*';
        internal static char field_delimiter_ = ',';
    }

    /// <summary>
    /// functions used to frame messages
    /// </summary>
    public class MessageUtils
    {
        /// <summary>
        /// Frame a message with the values passed
        /// </summary>
        /// <param name="vib">vibrotactor to activate</param>
        /// <param name="duration">duration of stumulus</param>
        /// <param name="intensity">intensity of stimulus</param>
        /// <returns>the framed message</returns>
        public static string FillMessage(Vibrotactor vib, int duration, int intensity)
        {
            string msg_body = FillMessageBody(vib, duration, intensity);
            string msg = MessageUtils.AddHeaderAndChecksum(msg_body);
            return msg;
        }

        /// <summary>
        /// Frame the message body with the values passed
        /// </summary>
        /// <param name="vib">vibrotactor to activate</param>
        /// <param name="duration">duration of stimulus</param>
        /// <param name="intensity">intensity of stimulus</param>
        /// <returns>the delimited message body</returns>
        internal static string FillMessageBody(Vibrotactor vib, int duration, int intensity)
        {
            string msg_body = "";
            msg_body += MessageStructure.identifier + MessageStructure.field_delimiter_;
            msg_body += ((int)vib).ToString() + MessageStructure.field_delimiter_;
            msg_body += duration.ToString() + MessageStructure.field_delimiter_;
            msg_body += intensity.ToString() + MessageStructure.field_delimiter_;
            return msg_body;
        }

        /// <summary>
        /// Create a test message for troubleshooting
        /// </summary>
        /// <returns>the framed test message</returns>
        public static string FillTestMessage()
        {
            string msg = "GPRMC,092751.000,A,5321.6802,N,00630.3371,W,0.06,31.66,280511,,,A";
            //string msg = "GPGGA,092750.000,5321.6802,N,00630.3372,W,1,8,1.03,61.7,M,55.2,M,,";
            int check = MessageUtils.checksum(msg.Trim());
            msg += MessageStructure.checksum_delimiter_ + check.ToString("X2");
            return msg;
        }

        /// <summary>
        /// Prepend the header and append the checksum to the framed message body
        /// </summary>
        /// <param name="msg_body">delimited message body</param>
        /// <returns>framed message with valid checksum</returns>
        internal static string AddHeaderAndChecksum(string msg_body)
        {
            int check = checksum(msg_body);
            string msg = MessageStructure.start_delimiter_ + msg_body + MessageStructure.checksum_delimiter_ + check.ToString() + System.Environment.NewLine;
            return msg;
        }

        /// <summary>
        /// Generate checksum by XORing all the bytes
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>checksum of message</returns>
        internal static int checksum(string msg)
        {
            byte[] msg_bytes = Encoding.ASCII.GetBytes(msg);
            int checksum = 0;
            // XOR all the bytes together
            foreach (byte c in msg_bytes)
            {
                checksum ^= c;
            }
            return checksum;
        }
    }
}