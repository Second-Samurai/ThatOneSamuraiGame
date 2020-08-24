using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TimeData
{
    public Vector3 position;
    public Quaternion rotation;

    public TimeData(Vector3 _position, Quaternion _rotation) 
    {
        position = _position;
        rotation = _rotation;
    }
}
