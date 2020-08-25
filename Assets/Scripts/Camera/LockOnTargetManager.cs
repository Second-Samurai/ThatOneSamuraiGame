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
    public float swapSpeed = 10f;

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
        //if (targetHolder.transform.position != _target.position)
        //    targetHolder.transform.Translate(_target.position - targetHolder.transform.position * swapSpeed * Time.deltaTime);
            
    }


    public void SetTarget(Transform target, Transform player)
    {

       // targetHolder.transform.position = target.position;
        cam.LookAt = target.transform;
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
