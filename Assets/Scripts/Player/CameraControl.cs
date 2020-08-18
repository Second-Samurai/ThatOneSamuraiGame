using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    FreeLookAddOn _camScript;
    public GameObject cam;
    Vector2 rotationVector;
    public Transform lockOnTarget;

    private void Start()
    {
        _camScript = cam.GetComponent<FreeLookAddOn>();
    }

    void OnRotateCamera(InputValue rotDir) 
    {
        rotationVector = rotDir.Get<Vector2>();
    }

    private void Update()
    {
        if(rotationVector != Vector2.zero)
            _camScript.RotateCam(rotationVector);
    }

    public void SetTarget(Transform target)
    {
        _camScript.SetTarget(target);
    }



}
