using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Obsolete]
public class FinisherCam : MonoBehaviour
{
    CinemachineVirtualCamera _cam;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _animator = GetComponentInParent<Animator>();
    }

    public void TransitionToCamera(Transform target)
    {
        _cam.m_Priority = 20;
        _cam.m_LookAt = target;
        //_animator.SetBool("FinisherSetup", true);
        _animator.SetTrigger("FinisherSetupTrigger");
    }

    public void LeaveCamera()
    {
        _cam.m_Priority = 1;
        _cam.m_LookAt = null;
        //_animator.SetBool("FinisherSetup", false);
    }
}
