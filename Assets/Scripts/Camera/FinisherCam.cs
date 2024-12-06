using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Player.Animation;

public class FinisherCam : MonoBehaviour
{
    CinemachineVirtualCamera _cam;
    PlayerAnimationComponent m_PlayerAnimationComponent;

    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        m_PlayerAnimationComponent = GetComponentInParent<PlayerAnimationComponent>();
    }

    public void TransitionToCamera(Transform target)
    {
        _cam.m_Priority = 20;
        _cam.m_LookAt = target;
        //_animator.SetBool("FinisherSetup", true);
        m_PlayerAnimationComponent.TriggerFinisher();
    }

    public void LeaveCamera()
    {
        _cam.m_Priority = 1;
        _cam.m_LookAt = null;
        //_animator.SetBool("FinisherSetup", false);
    }
}
