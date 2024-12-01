
using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

 
[Obsolete]
public class FreeLookAddOn : MonoBehaviour
{
    public float lookSpeed = .5f; 
    public CinemachineFreeLook cam;
    bool _bFlipY = true;

    void Awake()
    {
        cam = this.GetComponent<CinemachineFreeLook>();
        if (PlayerPrefs.GetFloat("Sensitivity") != 0) SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
    }

     
    public void RotateCam(Vector2 rotDir)
    {
        rotDir = rotDir.normalized;
        if (_bFlipY)
            rotDir.y = -rotDir.y;
        rotDir.x = rotDir.x * 180f;
 
        cam.m_XAxis.Value += rotDir.x * lookSpeed * Time.deltaTime;
        cam.m_YAxis.Value += rotDir.y * lookSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform target, Transform player)
    {
        cam.LookAt = target;
    }

    public void SetSensitivity(float sensitivity)
    {
        lookSpeed = sensitivity;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    public void SetPriority(int var)
    {
        cam.m_Priority = var;
    }
}
