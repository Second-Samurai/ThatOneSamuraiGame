using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimeData
{

    public float currentFrame;
    public string currentClip;
    public float inputSpeed, xInput, yInput;
    public bool lockedOn;

    public AnimationTimeData(float _currentFrame, string _currentClip, float _inputSpeed, float _xInput, float _yinput, bool _lockedOn)
    {
        currentFrame = _currentFrame;
        currentClip = _currentClip;
        inputSpeed = _inputSpeed;
        yInput = _yinput;
        xInput = _xInput;
        lockedOn = _lockedOn;


    }
}
