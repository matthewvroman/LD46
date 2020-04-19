using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventDispatcher : MonoBehaviour
{
    public Action<string>OnEventReceived;

    public void Event(string data)
    {
        if(OnEventReceived != null) OnEventReceived(data);
    }
}
