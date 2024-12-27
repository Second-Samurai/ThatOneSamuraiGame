﻿// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// /// <summary>
// /// DEPRECATED: Input handler for player. Currently used as a reference for current input system.
// /// </summary>
// public class PlayerInputScript : MonoBehaviour
// {
//     //FIELDS
//     #region Gameplay Bools
//     public bool bCanMove = true, bMoveLocked = false, bIsDodging = false, bCanDodge = true, bCanAttack = false, bGotParried = false, bIsSheathed = false, bCanRotate = true;
//     [HideInInspector] public bool bAlreadyAttacked = false;
//     [HideInInspector] public bool bCanBlock = true;
//     [HideInInspector] public bool bOverrideMovement = false;
//     bool _bDodgeCache = false;
//     #endregion
//
//     #region Script References
//     [HideInInspector] public CameraControl camControl;
//     [HideInInspector] public PlayerFunctions _functions;
//     [HideInInspector] public FinishingMoveController finishingMoveController;
//     [HideInInspector] public PlayerInput _inputComponent;
//     ICombatController _playerCombat;
//     HitstopController hitstopController;
//     Animator _animator;
//     Rigidbody rb;
//     PDamageController _pDamageController;
//     public PCombatController _pCombatController;
//     Camera _cam;
//     LockOnTracker _lockOnTracker;
//     #endregion
//
//     #region Gameplay parameters
//     public float rotationSpeed = 4f, smoothingValue = .1f;
//     float dodgeForce = 10f;
//     public Transform target;
//     float _turnSmoothVelocity;
//     #endregion
//
//     #region Movement vectors
//     Vector2 _inputVector, lastVector, _cachedVector;
//     private Vector2 cachedDir;
//     private bool isSprintHeld;
//     #endregion
//
//     #region Heavy Charging
//     float heavyTimer, heavyTimerMax = 2f;
//     bool bHeavyCharging = false, bPlayGleam = true;
//     public GameEvent showHeavyTutorialEvent;
//     public GameEvent startHeavyTelegraphEvent;
//     public GameEvent endHeavyTelegraphEvent;
//     #endregion
//
//
//     //INIT
//     #region Initialization 
//     private void Start()
//     {
//         _inputComponent = GetComponent<PlayerInput>();
//         _animator = GetComponent<Animator>();
//         _functions = GetComponent<PlayerFunctions>();
//         camControl = GetComponent<CameraControl>();
//         _playerCombat = this.GetComponent<ICombatController>();
//         rb = GetComponent<Rigidbody>();
//         _pDamageController = GetComponent<PDamageController>();
//         _pCombatController = GetComponent<PCombatController>();
//         _cam = Camera.main;
//         finishingMoveController = GetComponentInChildren<FinishingMoveController>();
//         hitstopController = GameManager.instance.GetComponent<HitstopController>();
//         _lockOnTracker = GameManager.instance.lockOnTracker;
//         heavyTimer = heavyTimerMax;
//     }
//     #endregion
//
//     //INPUT
//     #region Input Functions
//     
//     // NOTE: Method is called OnMovement but no movement logic exists
//     // void OnMovement(InputValue dir) 
//     // {
//     //     if (!finishingMoveController.bIsFinishing)
//     //     {
//     //         cachedDir = dir.Get<Vector2>();
//     //
//     //         if (cachedDir == Vector2.zero)
//     //         {
//     //             _animator.SetBool("IsSprinting", false);
//     //         }
//     //
//     //
//     //         if (!bMoveLocked) //normal movement - Method actively using this variable is not being used
//     //             _inputVector = cachedDir;
//     //         else //input during dodge
//     //         {
//     //             _cachedVector = cachedDir;
//     //             
//     //             // logic makes no sense, its basically performing the same code in previous statement
//     //             // logically this should merge and instead have the special action component couple with this method
//     //             if (cachedDir == Vector2.zero)
//     //                 _inputVector = cachedDir;
//     //         }
//     //     }
//     // }
//
//     // This method appears to only be used when pressed and instead is a gateway method to other methods.
//     // void OnSprint(InputValue value)
//     // {
//     //     isSprintHeld = value.isPressed;
//     //     if (!camControl.bLockedOn)
//     //     {
//     //         if (isSprintHeld) camControl.camScript.SprintOn();
//     //         else camControl.camScript.SprintOff();
//     //     }
//     // }
//
//     // public void OnLockOn()
//     // {
//     //    camControl.ToggleLockOn();
//     // }
//     //
//     // void OnToggleLockLeft()
//     // {
//     //     if (camControl.bLockedOn) camControl.LockOn();
//     // }
//     //
//     // void OnToggleLockRight()
//     // {
//     //     if (camControl.bLockedOn) camControl.LockOn();
//     // }
//
//     // Summary: Input control for sword drawing
//     //
//     // private void OnSwordDraw(InputValue value)
//     // {
//     //     if (_playerCombat == null)
//     //     {
//     //         Debug.Log(">> Combat Component is missing");
//     //         _playerCombat = this.GetComponent<ICombatController>();
//     //         return;
//     //     }
//     //
//     //     _playerCombat.DrawSword();
//     // }
//
//    
//     //NOTE: The combat component requires to be instantiated early. Suggest input script to be instantated late.
//     // private void OnAttack(InputValue value)
//     // {
//     //     if (bCanAttack && !bAlreadyAttacked)
//     //     {
//     //         //Debug.LogError(">> Light attack Triggered");
//     //         if (_playerCombat == null)
//     //         {
//     //             Debug.Log(">> Combat Component is missing");
//     //             _playerCombat = this.GetComponent<ICombatController>();
//     //             return;
//     //         }
//     //
//     //         if (!value.isPressed)
//     //         {
//     //             _playerCombat.RunLightAttack();
//     //             bAlreadyAttacked = false;
//     //         }
//     //
//     //        
//     //         if (hitstopController.bIsSlowing) hitstopController.CancelEffects();
//     //     }
//     //     else if (bAlreadyAttacked)
//     //     {
//     //         bAlreadyAttacked = false;
//     //     }
//     //
//     //     if (_animator.GetBool("HeavyAttackHeld"))
//     //     {
//     //         ExecuteHeavyAttack();
//     //         bAlreadyAttacked = false;
//     //     }
//     // }
//     //   
//     // void OnStartHeavy()
//     // {
//     //     heavyTimer = 2f;
//     //     StartHeavy();
//     // }
//     
//     // void OnStartHeavyAlternative()
//     // {
//     //     //bPlayGleam = false;
//     //     //heavyTimer = 1.0f;
//     //     //StartHeavy();
//     // }
//
//     // void StartHeavy() // THIS IS NOT AN EVENT HANDLER
//     // {
//     //     if (bCanAttack)
//     //     {
//     //         if (!_animator.GetBool("HeavyAttackHeld"))
//     //         {
//     //             startHeavyTelegraphEvent.Raise();
//     //             
//     //             bHeavyCharging = true;
//     //             bIsSheathed = true;
//     //             _animator.SetBool("HeavyAttackHeld", true);
//     //             //_camControl.StopCoroutine(_camControl.RollCam());
//     //             //_camControl.StopCoroutine(_camControl.ResetCamRoll());
//     //             camControl.StopAllCoroutines();
//     //             camControl.StartCoroutine(camControl.RollCam());
//     //         }
//     //     }
//     // }
//
//     // void OnStartBlock()
//     // {
//     //     if (bCanBlock)
//     //     {
//     //         _functions.StartBlock();
//     //         if (bGotParried) EndSlowEffects();
//     //
//     //     }
//     // }
//       
//     // void OnEndBlock()
//     // {
//     //     _functions.bInputtingBlock = false;
//     // }
//
//     // void OnDodge()
//     // {
//     //     
//     //     if (_inputVector != Vector2.zero && !bIsDodging && bCanDodge)
//     //     {
//     //
//     //         bOverrideMovement = false;
//     //         _animator.SetTrigger("Dodge");
//     //         _animator.ResetTrigger("AttackLight");
//     //         EnableMovement();
//     //         EnableRotation();
//     //         if (bGotParried) EndSlowEffects();
//     //         if (camControl.bLockedOn)
//     //         {
//     //             StopCoroutine("DodgeImpulse");
//     //             StartCoroutine(_functions.DodgeImpulse(new Vector3(_inputVector.x, 0, _inputVector.y), dodgeForce));
//     //         }
//     //         //Debug.Log("On dodge triggered");
//     //         ResetAttack();
//     //     }
//     //     else if (_inputVector != Vector2.zero && !bIsDodging && !bCanDodge && bGotParried)
//     //     {
//     //          
//     //         bOverrideMovement = false;
//     //         _animator.SetTrigger("Dodge");
//     //         _animator.ResetTrigger("AttackLight");
//     //         EnableMovement();
//     //         EnableRotation();
//     //         if (bGotParried) EndSlowEffects();
//     //         if (camControl.bLockedOn)
//     //         {
//     //             StopCoroutine("DodgeImpulse");
//     //             StartCoroutine(_functions.DodgeImpulse(new Vector3(_inputVector.x, 0, _inputVector.y), dodgeForce));
//     //         }
//     //
//     //         ResetAttack();
//     //     }
//     //     else if (_inputVector != Vector2.zero && !bIsDodging && !bCanDodge)
//     //     {
//     //         _bDodgeCache = true;
//     //
//     //     }
//     //     else if (_inputVector == Vector2.zero && !bIsDodging && bCanDodge)
//     //     {
//     //         bOverrideMovement = false;
//     //         _animator.SetTrigger("Dodge");
//     //         _animator.ResetTrigger("AttackLight");
//     //         EnableMovement();
//     //         EnableRotation();
//     //         if (bGotParried) EndSlowEffects();
//     //         if (camControl.bLockedOn)
//     //         {
//     //             StopCoroutine("DodgeImpulse");
//     //             StartCoroutine(_functions.DodgeImpulse(new Vector3(0, 0, 1), dodgeForce));
//     //         }
//     //
//     //         ResetAttack();
//     //     }
//     //     //Debug.Log("On dodge input triggered");
//     // }
//
//     // void OnPause()
//     // {
//     //     _functions.Pause();
//     // }
//
//     #endregion
//
//     //OUTPUT
//     #region Execution
//
//     // public void ResetAttack()
//     // {
//     //     // _pCombatController.EndAttacking(); // Make these interface methods
//     //     // _pCombatController.ResetAttackCombo(); // Make these interface methods
//     // }
//
//     // private void ExecuteHeavyAttack()
//     // {
//     //     showHeavyTutorialEvent.Raise();
//     //     endHeavyTelegraphEvent.Raise();
//     //     
//     //     bHeavyCharging = false;
//     //     bIsSheathed = false;
//     //     bPlayGleam = true; // Note: This shouldn't need to be a variable but rather an invocation
//     //     _animator.SetBool("HeavyAttackHeld", false);
//     //     //_camControl.StopCoroutine(_camControl.ResetCamRoll());
//     //     //_camControl.StopCoroutine("RollCam");
//     //     camControl.StopAllCoroutines();
//     //     camControl.StartCoroutine(camControl.ResetCamRoll());
//     // }
//
//     // void HeavyTimer()
//     // {
//     //     heavyTimer -= Time.deltaTime;
//     //     if(heavyTimer < .5f && bPlayGleam)
//     //     {
//     //         bPlayGleam = false;
//     //         _functions.parryEffects.PlayGleam();
//     //     }
//     //     if(heavyTimer <= 0)
//     //     {
//     //         bAlreadyAttacked = true;
//     //         ExecuteHeavyAttack();
//     //         heavyTimer = heavyTimerMax;
//     //     }
//     // }
//     //
//
//     // private void FixedUpdate()
//     // {
//         //MovePlayer(); 
//         // if (bHeavyCharging) HeavyTimer();
//         // else if (heavyTimer != heavyTimerMax) heavyTimer = heavyTimerMax;
//     // }
//
//     // INVESTIGATION: Move logic emplaced in similarly named location
//     // private void MovePlayer()
//     // {
//     //     if (bCanMove && !finishingMoveController.bIsFinishing)
//     //     
//     //     {
//     //         Vector3 _direction = new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
//     //         if (_direction != Vector3.zero && !camControl.bLockedOn && !bOverrideMovement && !bIsSheathed && bCanRotate)
//     //         {
//     //             float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
//     //             float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, .1f);
//     //
//     //             transform.rotation = Quaternion.Euler(0f, _angle, 0f);
//     //
//     //         }
//     //
//     //         else if (camControl.bLockedOn)
//     //         {
//     //             Vector3 lookDir = target.transform.position - transform.position;
//     //             Quaternion lookRot = Quaternion.LookRotation(lookDir);
//     //             transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed);
//     //             transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
//     //         }
//     //         
//     //         if (!bMoveLocked) // Note: According to the logic this will always be true
//     //         {
//     //             _animator.SetFloat("XInput", _inputVector.x, smoothingValue, Time.deltaTime);
//     //             _animator.SetFloat("YInput", _inputVector.y, smoothingValue, Time.deltaTime);
//     //         }
//     //         
//     //         _animator.SetFloat("InputSpeed", _inputVector.magnitude, smoothingValue, Time.deltaTime);
//     //
//     //         //Checks for sprinting
//     //         if (cachedDir == Vector2.zero)
//     //         {
//     //             _animator.SetBool("IsSprinting", false);
//     //         }
//     //         else
//     //         {
//     //             if (_animator.GetBool("IsSprinting") != isSprintHeld)
//     //             {
//     //                 _animator.SetBool("IsSprinting", isSprintHeld);
//     //             }
//     //         }
//     //
//     //         if (bOverrideMovement)
//     //         {
//     //             //transform.Translate(_direction * 3 * Time.deltaTime);
//     //             //StartCoroutine(_functions.DodgeImpulse(transform.forward, 3));
//     //         }
//     //
//     //     }
//     //     lastVector = _inputVector; // Note: This is not used
//     // }
//
//     // public void StartDodging()
//     // {
//         //Debug.Log("Dodge");
//         // bIsSheathed = false; -- Weapon
//         // bPlayGleam = true; -- Weapon
//         // bHeavyCharging = false; -- Weapon
//         // bCanAttack = false; -- Weapon
//         // bIsDodging = true; -- SpecialAction
//         // _pDamageController.DisableDamage(); 
//         // _functions.DisableBlock();
//         // ResetAttack(); -- Weapon
//
//     // }
//
//     public void EndDodging()
//     {
//         //Debug.Log("EndDodge");
//         // bCanAttack = true;
//         // bIsDodging = false;
//         // _pDamageController.EnableDamage();
//         // _functions.EnableBlock();
//         // ResetAttack();
//     }
//     #endregion
//
//     //Public Functions for bools and other Parameters
//     #region Parameter Controls
//     // public void OverrideMovement() // This is not used
//     // {
//     //     // bOverrideMovement = true;
//     // }
//     // public void RemoveOverride()
//     // {
//     //     // bOverrideMovement = false;
//     // }
//
//     // public void LockMoveInput() // This is not being used anywhere
//     // {
//     //     if (!bMoveLocked)
//     //     {
//     //         //Debug.LogWarning("Start Dodge");
//     //         bMoveLocked = true;
//     //         StartDodging();
//     //     }
//     // }
//
//     // public void UnlockMoveInput() //This method is not being used anywhere else
//     // {
//     //     if (bMoveLocked)
//     //     {
//     //         //Debug.LogWarning("End Dodge");
//     //         bMoveLocked = false; 
//     //         if (_inputVector != _cachedVector && _cachedVector != Vector2.zero)
//     //         {
//     //             //Debug.Log("set " + _inputVector + " to " + _cachedVector);
//     //             _inputVector = _cachedVector;
//     //             _cachedVector = Vector2.zero;
//     //         }
//     //         EndDodging();
//     //     }
//     // }
//      
//     // public void Test()
//     // {
//     //     _pDamageController.OnEntityDamage(1, this.gameObject, false);
//     // } 
//     // public void ResetDodge()
//     // {
//     //     bCanDodge = true;
//     //     bIsDodging = false;
//     //   
//     // }
//
//     // public void BlockDodge()
//     // {
//     //     //bCanDodge = false;
//     //  
//     // }
//
//     // public void EnableInput()
//     // {
//     //     _inputComponent.enabled = true;
//     // }
//     //
//     // public void DisableInput()
//     // {
//     //     _inputComponent.enabled = false;
//     // }
//
//     // public void DisableMovement()
//     // {
//     //     bCanMove = false;
//     // }
//     //
//     // public void EnableMovement()
//     // {
//     //     bCanMove = true;
//     // }
//
//     // public void GotParried()
//     // {
//     //     bGotParried = true; 
//     // }
//
//     // public void EndGotParried()
//     // {
//     //     bGotParried = false;
//     // }
//     //
//     // private void EndSlowEffects()
//     // {
//     //     EndGotParried();
//     //     hitstopController.CancelEffects();
//     // }
//
//     // public void EnableRotation()
//     // {
//     //     bCanRotate = true;
//     // }
//     // public void DisableRotation()
//     // {
//     //     bCanRotate = false;
//     // }
//
//     #endregion
// }
