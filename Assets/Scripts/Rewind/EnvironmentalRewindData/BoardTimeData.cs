using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTimeData
{
    public bool isBuilt;
    public Vector3 velocity;

    public BoardTimeData(bool _isBuilt, Vector3 _velocity) 
    {
        isBuilt = _isBuilt;
        velocity = _velocity;
    }


}
