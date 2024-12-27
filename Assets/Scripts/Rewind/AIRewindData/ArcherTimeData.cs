using UnityEngine;

public class ArcherTimeData
{
    public Vector3 lastDirection, shotDirection;
    public CurrentState currentState;
    public float shotTimer;
    public bool bColEnabled;

    public ArcherTimeData(Vector3 _LastDirection, Vector3 _shotDirection, CurrentState _currentState, float _shotTimer, bool _bColEnabled) 
    {
        lastDirection = _LastDirection;
        shotDirection = _shotDirection;
        currentState = _currentState;
        shotTimer = _shotTimer;
        bColEnabled = _bColEnabled;
    }

}
