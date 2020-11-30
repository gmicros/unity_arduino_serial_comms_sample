using System;
using UnityArduinoComms;

namespace CommsTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string test_msg = MessageUtils.FillTestMessage();
            string resp = ArduinoSerialInterface.SendMessage(test_msg);
            Console.WriteLine("response: ", resp);
        }
    }
}
