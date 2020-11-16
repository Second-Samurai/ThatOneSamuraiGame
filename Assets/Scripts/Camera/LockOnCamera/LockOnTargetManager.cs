using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOnTargetManager : MonoBehaviour
{
    public float lookSpeed = .5f;
    public CinemachineFreeLook cam;
    public bool   _bLockedOn = false;
    public GameObject targetHolder;
    public Transform _target, _player;
    public float swapSpeed = 10f;
    public FinisherCam finisherCam;
    CinemachineBrain mainCam;
    Animator animator;

    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
        mainCam = Camera.main.GetComponent<CinemachineBrain>();
        animator = GameManager.instance.playerController.gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_bLockedOn)
            MoveTarget();
        if((!_bLockedOn || !animator.GetBool("LockedOn"))&& cam.Equals((object)mainCam.ActiveVirtualCamera))
        {
            cam.m_Priority = 3;
        }
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

    public void GuardBreakCam(Transform target)
    {
        finisherCam.TransitionToCamera(target);
    }

    public void EndGuardBreakCam()
    {
        finisherCam.LeaveCamera();
    }
}
