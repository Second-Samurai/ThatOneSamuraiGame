using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    FreeLookAddOn _camScript, _lockedCamScript;
    public GameObject unlockedCam, lockedCam;
    Vector2 rotationVector;
    public Transform lockOnTarget;
    public bool bLockedOn = false;

    private void Start()
    {
        _camScript = unlockedCam.GetComponent<FreeLookAddOn>();
        _lockedCamScript = lockedCam.GetComponent<FreeLookAddOn>();
    }

    void OnRotateCamera(InputValue rotDir) 
    {
        rotationVector = rotDir.Get<Vector2>();
    }

    private void Update()
    {
        if(rotationVector != Vector2.zero && !bLockedOn)
            _camScript.RotateCam(rotationVector);
    }

    public void SetTarget(Transform target)
    {
        _lockedCamScript.SetTarget(target);
    }

    public void LockOn()
    {
        bLockedOn = true;
        SetTarget(lockOnTarget);
        _lockedCamScript.cam.Priority = 11;
    }

    public void UnlockCam()
    {
        bLockedOn = false;
        _lockedCamScript.cam.Priority = 9;
    }



}
