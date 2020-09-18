using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGameEvent : MonoBehaviour
{
    public GameEvent gameEvent;

    public void CallEvent()
    {
        gameEvent.Raise();
    }
}
