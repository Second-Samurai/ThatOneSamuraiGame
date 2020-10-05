using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIAnimationTimeData
{
    public float currentFrame;
    public int currentClip;
    public float movementX;
    public float movementZ;

    public AIAnimationTimeData(float _currentFrame, int _currentClip, float _movementX, float _movementZ) 
    {
        currentFrame = _currentFrame;
        currentClip = _currentClip;
        movementX = _movementX;
        movementZ = _movementZ;
    }
}
