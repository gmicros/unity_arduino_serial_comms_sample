using System;
using System.Collections.Generic;
using System.Text;

namespace UnityArduinoComms
{
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
