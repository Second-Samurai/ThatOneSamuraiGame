using UnityEngine;

public class ArmourTimeData 
{
    public Vector3 velocity;
    public bool isEnabled;
     public bool destroyed;
    public Transform parent;

    public ArmourTimeData(bool _isEnabled, bool _destroyed, Vector3 _velocity, Transform _parent)
    {
        isEnabled = _isEnabled;
        destroyed = _destroyed;
        velocity = _velocity;
        parent = _parent;
    }
}
