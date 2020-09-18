using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : MonoBehaviour
{

    Vector2 _inputVector, lastVector, _cachedVector;
    public bool bCanMove = true, bLockedOn = false, bMoveLocked = false, bIsDodging = false, bCanDodge = true;
    public float rotationSpeed = 4f, smoothingValue = .1f;
    Animator _animator;
    public CameraControl _camControl;
    public PlayerFunctions _functions;
    ICombatController _playerCombat;
    public Transform target;
    Rigidbody rb;
    PDamageController _pDamageController;
    PCombatController _pCombatController;
    public PlayerInput _inputComponent;
    Camera _cam;
    public FinishingMoveController finishingMoveController;

    float dodgeForce = 10f;

    float _turnSmoothVelocity;

    bool _bDodgeCache = false;
    public bool bCanBlock = true;
    public bool bOverrideMovement = false;
     
    private void Start()
    {
        _inputComponent = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _functions = GetComponent<PlayerFunctions>();
        _camControl = GetComponent<CameraControl>();
        _playerCombat = this.GetComponent<ICombatController>();
        rb = GetComponent<Rigidbody>();
        _pDamageController = GetComponent<PDamageController>();
        _pCombatController = GetComponent<PCombatController>();
        _cam = Camera.main;
        finishingMoveController = GetComponentInChildren<FinishingMoveController>();
    }

    void OnMovement(InputValue dir) 
    {
        Vector2 cachedDir;
        cachedDir = dir.Get<Vector2>();
        if (!bMoveLocked) //normal movement
            _inputVector = cachedDir;
        else //input during dodge
        {
            _cachedVector = cachedDir;
            if (cachedDir == Vector2.zero) 
                _inputVector = cachedDir;
        }
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
            bLockedOn = false;
            _camControl.UnlockCam();
            _animator.SetBool("LockedOn", bLockedOn);
            _camControl.bLockedOn = bLockedOn;
        }
 
    }

    void OnToggleLockLeft()
    {
        if (bLockedOn) _camControl.LockOn();
    }

    void OnToggleLockRight()
    {
        if (bLockedOn) _camControl.LockOn();
    }

    

    //void OnReleaseLockOn()
    //{
    //    if (bLockedOn)
    //    {
    //        bLockedOn = false;
    //        _camControl.UnlockCam();
    //        _animator.SetBool("LockedOn", bLockedOn);
    //        _camControl.bLockedOn = bLockedOn;
    //    }
    //}

    //NOTE: The combat component requires to be instantiated early. Suggest input script to be instantated late.
    private void OnAttack(InputValue value)
    {
        //Debug.LogError(">> Light attack Triggered");
        if (_playerCombat == null)
        {
            Debug.Log(">> Combat Component is missing");
            _playerCombat = this.GetComponent<ICombatController>();
            return;
        }

        if (!value.isPressed)
        {
            _playerCombat.RunLightAttack();
        }

        if (_animator.GetBool("HeavyAttackHeld"))
        {
            _animator.SetBool("HeavyAttackHeld", false);
            //_camControl.StopCoroutine(_camControl.ResetCamRoll());
            //_camControl.StopCoroutine("RollCam");
            _camControl.StopAllCoroutines();
            _camControl.StartCoroutine(_camControl.ResetCamRoll());
        }
    }

    void OnStartHeavy()
    {
        if (!_animator.GetBool("HeavyAttackHeld"))
        {
            _animator.SetBool("HeavyAttackHeld", true);
            //_camControl.StopCoroutine(_camControl.RollCam());
            //_camControl.StopCoroutine(_camControl.ResetCamRoll());
            _camControl.StopAllCoroutines();
            _camControl.StartCoroutine(_camControl.RollCam());
        }
    }

    void OnStartBlock()
    {
        if(bCanBlock)
            _functions.StartBlock();
    }

    void OnEndBlock()
    {
        _functions.EndBlock();
    }

    void OnDodge()
    {
        if (_inputVector != Vector2.zero && !bIsDodging && bCanDodge)
        {
            _animator.SetTrigger("Dodge");
            if (bLockedOn)
            {
                StopCoroutine("DodgeImpulse");
                StartCoroutine(_functions.DodgeImpulse(new Vector3(_inputVector.x, 0, _inputVector.y), dodgeForce));
            }
            if (_functions.bCanBlock == false)
                _functions.EnableBlock();
            _functions.DisableBlock();
            bOverrideMovement = false;
        }
        else if (_inputVector != Vector2.zero && !bIsDodging && !bCanDodge)
        {
            _bDodgeCache = true;

        }
    }

    void OnPause()
    {
        _functions.Pause();
    }
   

    private void FixedUpdate()
    {
        MovePlayer();
        //if (_bDodgeCache && bCanDodge)
        //{
        //    _animator.SetTrigger("Dodge");
        //    StopCoroutine("DodgeImpulse");
        //    StartCoroutine(_functions.DodgeImpulse(new Vector3(_inputVector.x, 0, _inputVector.y), dodgeForce));
        //    if (_functions.bCanBlock == false)
        //        _functions.EnableBlock();
        //    _bDodgeCache = false;
        //}

    }

    private void MovePlayer()
    {
        if (bCanMove)
        {
            Vector3 _direction = new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
            if (_direction != Vector3.zero && !bLockedOn && !bOverrideMovement)
            {
                float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, .1f);

                transform.rotation = Quaternion.Euler(0f, _angle, 0f);

            }

            else if (bLockedOn)
            {
                Vector3 lookDir = target.transform.position - transform.position;
                Quaternion lookRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                if (!bMoveLocked)
                {
                    _animator.SetFloat("XInput", _inputVector.x, smoothingValue, Time.deltaTime);
                    _animator.SetFloat("YInput", _inputVector.y, smoothingValue, Time.deltaTime);
                }
            }

            
            _animator.SetFloat("InputSpeed", _inputVector.magnitude, smoothingValue, Time.deltaTime);

            if (bOverrideMovement)
            {
                //transform.Translate(_direction * 3 * Time.deltaTime);
                //StartCoroutine(_functions.DodgeImpulse(transform.forward, 3));
            }

        }
        lastVector = _inputVector;
    }

    public void StartDodging()
    {
        bIsDodging = true; 
        _pDamageController.DisableDamage();
       
            
    }

    public void EndDodging()
    {
        bIsDodging = false;
        _pDamageController.EnableDamage();
    }

    public void OverrideMovement()
    {
        bOverrideMovement = true;
    }
    public void RemoveOverride()
    {
        bOverrideMovement = false;
    }

    public void LockMoveInput()
    {
        if (!bMoveLocked)
        {
            //Debug.LogWarning("Start Dodge");
            bMoveLocked = true;
            StartDodging();
        }
    }

    public void UnlockMoveInput()
    {
        if (bMoveLocked)
        {
            //Debug.LogWarning("End Dodge");
            bMoveLocked = false; 
            if (_inputVector != _cachedVector && _cachedVector != Vector2.zero)
            {
                Debug.Log("set " + _inputVector + " to " + _cachedVector);
                _inputVector = _cachedVector;
                _cachedVector = Vector2.zero;
            }
            EndDodging();
        }
    }


    public void Test()
    {
        _pDamageController.OnEntityDamage(1, this.gameObject, false);
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
            Test();
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            finishingMoveController.PlayFinishingMove(target.gameObject);
        }
    }

    public void ResetDodge()
    {
        bCanDodge = true;
      
    }

    public void BlockDodge()
    {
        bCanDodge = false;
     
    }

    public void EnableInput()
    {
        _inputComponent.enabled = true;
    }

    public void DisableInput()
    {
        _inputComponent.enabled = false;
    }

}
