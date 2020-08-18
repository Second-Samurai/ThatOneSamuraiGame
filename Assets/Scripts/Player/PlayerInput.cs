using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    Vector3 heading;
    Vector2 inputVector, rotationVector;
    public bool canMove = true;
    public float rotationSpeed = 4f, smoothingValue = .1f;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMovement(InputValue dir) 
    {
        inputVector = dir.Get<Vector2>();
        Debug.Log(inputVector.magnitude); 
    }

    void OnRotateCamera(InputValue dir) 
    {
        rotationVector = dir.Get<Vector2>();
    }


    private void FixedUpdate()
    {
        Debug.LogWarning(rotationVector);
        animator.SetFloat("InputSpeed", inputVector.magnitude, smoothingValue, Time.deltaTime);
        if (canMove)
        {
            heading = new Vector3(inputVector.x,0,inputVector.y);
            if (heading != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), Time.deltaTime * rotationSpeed);
        }

    }
}
