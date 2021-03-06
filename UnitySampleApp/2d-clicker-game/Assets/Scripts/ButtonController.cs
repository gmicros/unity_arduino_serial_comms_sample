﻿using UnityEngine;
using UnityArduinoComms;

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
        string msg = MessageUtils.FillMessage(vibro, duration, intensity);
        Debug.Log("Filled msg: " + msg);
        string resp = ArduinoSerialInterface.SendMessage(msg);
        Debug.Log("Resp: " + resp);
    }
}
