
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

 
public class FreeLookAddOn : MonoBehaviour
{
    public float lookSpeed = .5f; 
    private CinemachineFreeLook _cam;
    bool _bFlipY = true;

    void Start()
    {
        _cam = GetComponent<CinemachineFreeLook>();
    }

     
    public void RotateCam(Vector2 rotDir)
    {
        rotDir = rotDir.normalized;
        if (_bFlipY)
            rotDir.y = -rotDir.y;
        rotDir.x = rotDir.x * 180f;
        Debug.LogWarning(rotDir);
        _cam.m_XAxis.Value += rotDir.x * lookSpeed * Time.deltaTime;
        _cam.m_YAxis.Value += rotDir.y * lookSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform target)
    {
        _cam.LookAt = target;
    }
}
