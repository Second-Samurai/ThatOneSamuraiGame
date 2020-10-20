using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLockOnTutorial : MonoBehaviour
{
    public GameEvent HideLockOnTutorialEvent;

    private void OnTriggerEnter(Collider other)
    {
        HideLockOnTutorialEvent.Raise();
    }
}
