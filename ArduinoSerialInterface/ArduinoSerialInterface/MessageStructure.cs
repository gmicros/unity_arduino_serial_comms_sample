using System;
using System.Collections.Generic;
using System.Text;

namespace UnityArduinoComms
{
    static class MessageStructure
    {
        internal static string identifier = "UNITY";
        internal static char start_delimiter_ = '$';
        internal static char checksum_delimiter_ = '*';
        internal static char field_delimiter_ = ',';
    }
}
