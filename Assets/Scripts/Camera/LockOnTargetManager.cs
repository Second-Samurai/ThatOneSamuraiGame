using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOnTargetManager : MonoBehaviour
{
    public float lookSpeed = .5f;
    public CinemachineFreeLook cam;
    bool   _bLockedOn = false;
    public GameObject targetHolder;
    Transform _target, _player;

    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    private void FixedUpdate()
    {
        if (_bLockedOn)
            MoveTarget();
    }

    void MoveTarget()
    {
        targetHolder.transform.position = _target.position + (_player.position - _target.position) / 2;
    }


    public void SetTarget(Transform target, Transform player)
    {

        targetHolder.transform.position = target.position + (player.position - target.position) / 2;
        cam.LookAt = targetHolder.transform;
        _bLockedOn = true;
        _target = target;
        _player = player;
    }

    public void ClearTarget()
    {
        _target = null;
        _player = null;
        _bLockedOn = false;
    }
}
