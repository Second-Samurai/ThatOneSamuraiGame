using UnityEngine;

[System.Serializable]
public class PositionalTimeData
{
    public Vector3 position;
    public Quaternion rotation;
   

    public PositionalTimeData(Vector3 _position, Quaternion _rotation) 
    {
        position = _position;
        rotation = _rotation;

    }
}
