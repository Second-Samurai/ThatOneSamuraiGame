using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    Vector2 _inputVector, lastVector;
    public bool bIsMoving = false; //Used for enemy AISystem to change their target position on player move
    public bool bCanMove = true, bLockedOn = false;
    public float rotationSpeed = 4f, smoothingValue = .1f;
    Animator _animator;
    CameraControl _camControl;
    PlayerFunctions _functions;
    IPlayerCombat _playerCombat;
    public Transform target;

    float _turnSmoothVelocity;
     /// <summary>
     /// ///
     /// </summary>
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _functions = GetComponent<PlayerFunctions>();
        _camControl = GetComponent<CameraControl>();
        _playerCombat = this.GetComponent<IPlayerCombat>();
    }

    void OnMovement(InputValue dir)
    {
        bIsMoving = true;
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

    //NOTE: The combat component requires to be instantiated early. Suggest input script to be instantated late.
    private void OnAttack(InputValue value)
    {
        if (_playerCombat == null)
        {
            Debug.Log(">> Combat Component is missing");
            _playerCombat = this.GetComponent<IPlayerCombat>();
            return;
        }

        if (!value.isPressed)
        {
            Debug.Log(">> Light attack Triggered");
            _playerCombat.RunLightAttack();
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
