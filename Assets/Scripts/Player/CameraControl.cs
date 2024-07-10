using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;
using UnityEngine.InputSystem;

/* -----------------------------------------------------------
 * Noted Issues:
 * - Should attempt to decouple this from the player. Otherwise make a specific player camera handler to send commands
 *   to the Camera.
 * - Boolean logic appears not to be always consistent.
 * -----------------------------------------------------------
 */

public class CameraControl : MonoBehaviour, IControlledCameraState, ICameraController
{
    public PlayerCamTargetController camTargetScript;
    public ThirdPersonCamController camScript;
    LockOnTargetManager _lockedCamScript;
    public GameObject unlockedCam, lockedCam;
    private Animator _animator;
    public Transform lockOnTarget, player, lockOnNullDummy;
    public bool bLockedOn = false;
    public LockOnTracker lockOnTracker;

    public CinematicBars cinematicBars;
    
    public GameEvent onLockOnEvent;
    private bool _bRunLockCancelTimer = false;
    private const float maxLockCancelTimer = 0.4f;
    public float _lockCancelTimer;

    private PlayerTargetTrackingState m_PlayerTargetTrackingState;

    #region - - - - - - Properties - - - - - -

    bool ICameraController.IsLockedOn
        => this.bLockedOn;
    
    #endregion Properties

    private void Start()
    {
        // _camScript = unlockedCam.GetComponent<FreeLookAddOn>();
        // _lockedCamScript = lockedCam.GetComponent<LockOnTargetManager>();
        // _playerInput = GetComponent<PlayerInputScript>();

        this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;
    }
 
    //NOTE: this is called in player controller
    public void Init(Transform playerTarget)
    {
        //Debug.Log("Test");
        
        GameManager gameManager = GameManager.instance;
        CinematicBars cinematicBars = gameManager.mainCamera.GetComponentInChildren<CinematicBars>();

        this.player = playerTarget;
        this.unlockedCam = gameManager.thirdPersonViewCamera;
        this._animator = gameManager.playerController.GetComponent<Animator>();
        this.cinematicBars = cinematicBars;

        if (!lockOnTracker)
        {
            this.lockOnTracker = gameManager.lockOnTracker;
        }
        
        if (!unlockedCam)
        {
            Debug.LogError("Third person camera object not assigned in inspector! Please assign");
        }

        if (!camScript)
        {
            Debug.LogWarning("Third Person Camera not assigned in inspector! Assigning via Init call");
            camScript = unlockedCam.GetComponent<ThirdPersonCamController>();
            camTargetScript = camScript.camTargetController;
        }

        //_camScript = unlockedCam.GetComponent<Player>();
       
       
        _lockedCamScript = lockedCam.GetComponent<LockOnTargetManager>();
    }

    public void RotateCamera(Vector2 rotationInput)
    {
        camTargetScript.RotateCam(rotationInput);
        
        if (_bRunLockCancelTimer) RunLockCancelTimer();
        else _lockCancelTimer = maxLockCancelTimer;
    }

    public void SetTarget(Transform target)
    {
        lockOnTracker.SetTarget(target);
        _lockedCamScript.SetTarget(target, player);
    }

    public void ToggleLockOn()
    {
        if (!bLockedOn)
        {
            if (LockOn())
            {
                bLockedOn = true;
            }
        }
        else
        {
            UnlockCam();
        }
        onLockOnEvent.Raise();
    }

    public bool LockOn()
    {
        _bRunLockCancelTimer = false;
        if (GetTarget())
        {
            _bRunLockCancelTimer = false;
            _animator.SetBool("LockedOn", true);
            _lockedCamScript.cam.Priority = 15;
            cinematicBars.ShowBars(200f, .3f);
            return true;
        }
        else return false;

    }

    public void UnlockCam()
    {
        lockOnTracker.ClearTarget();

        bLockedOn = false;
        _lockedCamScript.cam.Priority = 9;
        _lockedCamScript.ClearTarget();
        lockOnTarget = null;
        cinematicBars.HideBars(.3f);
        Debug.Log(300000);
        _animator.SetBool("LockedOn", false);
    }

    public bool GetTarget()
    {
        float closest = Mathf.Infinity;
        Transform nextEnemy = null;

        if (lockOnTracker == null)
        {
            lockOnTracker = GameManager.instance.lockOnTracker;
        }
        if (lockOnTracker.targetableEnemies.Count > 0)
        {
            foreach (Transform enemy in lockOnTracker.targetableEnemies)
            {
                float distance = Vector3.Distance(player.position, enemy.position);
                if (distance < closest && enemy != lockOnTarget)
                {
                    closest = distance;
                    nextEnemy = enemy;
                }
                if (nextEnemy == null && lockOnTarget != null)
                    nextEnemy = lockOnTarget;
            }

            if (nextEnemy != lockOnTarget)
            {
                lockOnTarget = nextEnemy;
                this.m_PlayerTargetTrackingState.AttackTarget = lockOnTarget.gameObject;
            }

            if (lockOnTarget != null)
            {
                SetTarget(lockOnTarget);
                bLockedOn = true;
            }
            return bLockedOn;
        }
        else
        {
            
            //_playerInput.target = lockOnNullDummy;
           // SetTarget(lockOnNullDummy);
            bLockedOn = false;
            Debug.Log(500000);
            return bLockedOn;
        }
    }

    void ICameraController.RollCamera()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.RollCam());
    }

    public IEnumerator RollCam()
    {
        while(_lockedCamScript.cam.m_Lens.Dutch > -10)
        {
            _lockedCamScript.cam.m_Lens.Dutch -= Time.deltaTime * 3f;
            _lockedCamScript.cam.m_Lens.FieldOfView -= Time.deltaTime * 3f;
            yield return null;
        }
    }

    // Note: This is a duplicate method of the same name. Originally the usage of the
    //       Coroutine involes stopping all other coroutines which meant the management of
    //       the component's state has to be done externally.
    //       In this location
    void ICameraController.ResetCameraRoll()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.ResetCamRoll());
    }

    public IEnumerator ResetCamRoll()
    {
        while (_lockedCamScript.cam.m_Lens.Dutch < 0)
        {
            _lockedCamScript.cam.m_Lens.Dutch += Time.deltaTime * 30f;
            if (_lockedCamScript.cam.m_Lens.FieldOfView < 40)
            {
                _lockedCamScript.cam.m_Lens.FieldOfView += Time.deltaTime * 100f;
            }
            else _lockedCamScript.cam.m_Lens.FieldOfView = 60f;
            yield return null;
        }
        _lockedCamScript.cam.m_Lens.Dutch = 0f;
    }

    public void EndGuardBreakCam()
    {
        _lockedCamScript.EndGuardBreakCam();
    }

    //Called by the CancelLockOnEvent from the lock on tracker
    public void CancelLockOnResponse()
    {
        if (bLockedOn)
        {
            _bRunLockCancelTimer = true;
        }
    }
    
    private void RunLockCancelTimer()
    {
        _lockCancelTimer -= Time.deltaTime;
        if (_lockCancelTimer < 0)
        {
            _bRunLockCancelTimer = false;
            _lockCancelTimer = maxLockCancelTimer;
           
            if(bLockedOn) ToggleLockOn(); Debug.Log(100000);
        }
    }

    public Vector3 CurrentEulerAngles
    {
        get
        {
            return Camera.main.transform.eulerAngles;
        }
    }

    public Vector3 ForwardDirection
    {
        get
        {
            return Camera.main.transform.forward;
        }
    }

    public bool IsCameraViewTargetLocked
    {
        get
        {
            return this.bLockedOn;
        }
    }

    void ICameraController.ToggleSprintCameraState(bool isSprinting)
    {
        if (isSprinting)
            this.camScript.SprintOn();
        else
            this.camScript.SprintOff();
    }
}
