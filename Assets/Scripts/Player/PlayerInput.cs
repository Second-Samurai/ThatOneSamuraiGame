using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    Vector2 _inputVector, lastVector;
    public bool bCanMove = true, bLockedOn = false;
    public float rotationSpeed = 4f, smoothingValue = .1f;
    Animator _animator;
    CameraControl _camControl;
    PlayerFunctions _functions;
    public Transform target;

    float _turnSmoothVelocity;
     
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _functions = GetComponent<PlayerFunctions>();
        _camControl = GetComponent<CameraControl>();
    }

    void OnMovement(InputValue dir) 
    {
        _inputVector = dir.Get<Vector2>(); 
    }

    void OnLockOn()
    {

        if (!bLockedOn)
        {
            bLockedOn = true;
            _camControl.LockOn();
            _animator.SetBool("LockedOn", bLockedOn);
            _camControl.bLockedOn = bLockedOn;
        }
        else
        {
            _camControl.LockOn();
        }
 
    }

    void OnReleaseLockOn()
    {
        if (bLockedOn)
        {
            bLockedOn = false;
            _camControl.UnlockCam();
            _animator.SetBool("LockedOn", bLockedOn);
            _camControl.bLockedOn = bLockedOn;
        }
    }
  
    void OnStartBlock()
    {
        _functions.StartBlock();
    }

    void OnEndBlock()
    {
        _functions.EndBlock();
    }

    void OnDodge()
    {
        _animator.SetTrigger("Dodge");
    }



    private void FixedUpdate()
    {
        if (bCanMove)
        {
            Vector3 _direction = new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
            if (_direction != Vector3.zero && !bLockedOn)
            {
                float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, .1f);

                transform.rotation = Quaternion.Euler(0f, _angle, 0f);

            } 
            
            else if (bLockedOn) 
            {
                Vector3 lookDir = target.transform.position - transform.position;
                Quaternion lookRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);


                _animator.SetFloat("XInput", _inputVector.x, smoothingValue, Time.deltaTime);
                _animator.SetFloat("YInput", _inputVector.y, smoothingValue, Time.deltaTime);
            
            }
            
            _animator.SetFloat("InputSpeed", _inputVector.magnitude, smoothingValue, Time.deltaTime);

        }
        lastVector = _inputVector;
    }
     
}
