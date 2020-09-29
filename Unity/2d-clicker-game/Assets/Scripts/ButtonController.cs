using UnityEngine;
using gmicros_arduino_interface;

public class ButtonController : MonoBehaviour
{
    public Vibrotactor vibro;
    public int duration;
    public int intensity;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void TaskOnClick()
    {
        ArduinoSerialInterface.Init();
        string msg = ArduinoSerialInterface.FillMessage(vibro, duration, intensity);
        Debug.Log("Filled msg: " + msg);
        ArduinoSerialInterface.SendMessage(msg);        
    }
}
