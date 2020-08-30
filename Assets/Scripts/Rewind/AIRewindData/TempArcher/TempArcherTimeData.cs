using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempArcherTimeData
{

    public int currentState;
    public Vector3 shotDirection;
    public float shotTimer;
    public float aimCounter;
    public bool lineRenderer;


    public TempArcherTimeData(int _currentState, Vector3 _shotDirection, float _shotTimer, float _aimCounter, bool _lineRenderer)  
    {
        currentState = _currentState;
        shotDirection = _shotDirection;
        shotTimer = _shotTimer;
        aimCounter = _aimCounter;
        lineRenderer = _lineRenderer;
    }

}
