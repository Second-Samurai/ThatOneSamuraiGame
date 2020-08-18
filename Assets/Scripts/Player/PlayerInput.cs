using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    Vector3 heading, forward, right, xMove, yMove;
    Vector2 inputVector, rotationVector;
    public bool canMove = true;
    public float rotationSpeed = 4f, smoothingValue = .1f;
    Animator animator;


    float turnSmoothVelocity;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMovement(InputValue dir) 
    {
        inputVector = dir.Get<Vector2>();
       
       
    }

    private void Update()
    {
        //InitialiseDirections();
    }

    private void FixedUpdate()
    {

        //if (canMove)
        //{
        //    heading = new Vector3(inputVector.x,0,inputVector.y);
        //    if (heading != Vector3.zero)
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), Time.deltaTime * rotationSpeed);
        //}
        //Vector3 moveDir = Vector3.zero;
        Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, .1f);

           //moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
         
        animator.SetFloat("InputSpeed", inputVector.magnitude, smoothingValue, Time.deltaTime);
    }

    void InitialiseDirections()
    {
        forward = Camera.main.transform.forward; //forward direction
        forward.y = 0; //sets y to 0
        forward = Vector3.Normalize(forward); //normalises

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward; //gets right as a direction relative to forward

        xMove = inputVector.x * right;
        yMove = inputVector.y * forward;
        inputVector = xMove + yMove;
        inputVector = inputVector.normalized;
        Debug.Log(inputVector.magnitude);
        Debug.Log(inputVector);

        


    }
}
