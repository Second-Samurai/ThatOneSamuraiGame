using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public PlayerCamTargetController camTargetScript;
    public ThirdPersonCamController camScript;
    LockOnTargetManager _lockedCamScript;
    public GameObject unlockedCam, lockedCam;
    private Animator _animator;
    Vector2 rotationVector;
    public Transform lockOnTarget, player, lockOnNullDummy;
    public bool bLockedOn = false;
    public LockOnTracker lockOnTracker;
    PlayerInputScript _playerInput;

    public CinematicBars cinematicBars;
    
    public GameEvent onLockOnEvent;
    private const float maxLockCancelTimer = 0.7f;
    private float lockCancelTimer;

    /*private void Start()
    {
        _camScript = unlockedCam.GetComponent<FreeLookAddOn>();
        _lockedCamScript = lockedCam.GetComponent<LockOnTargetManager>();
        _playerInput = GetComponent<PlayerInputScript>();
    }*/

 
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
        _playerInput = GetComponent<PlayerInputScript>();

        lockCancelTimer = maxLockCancelTimer;
    }

    void OnRotateCamera(InputValue rotDir) 
    {
        rotationVector = rotDir.Get<Vector2>();
    }

    private void Update()
    {
        if (rotationVector != Vector2.zero && !bLockedOn)
            camTargetScript.RotateCam(rotationVector);
        else if (GameManager.instance.rewindManager.isTravelling)
        {
            camTargetScript.RotateCam(rotationVector);
        }
    }

    public void SetTarget(Transform target)
    {
        lockOnTracker.SetTarget(target);
        _lockedCamScript.SetTarget(target, player);
    }

    public void ToggleLockOn()
    {
        onLockOnEvent.Raise();
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
    }

    public bool LockOn()
    {
        if (GetTarget())
        {
            lockCancelTimer = maxLockCancelTimer;
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
                _playerInput.target = lockOnTarget;
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
            return bLockedOn;
        }
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
            lockCancelTimer -= Time.deltaTime;
            if (lockCancelTimer < 0)
            {
                lockCancelTimer = maxLockCancelTimer;
                ToggleLockOn();
            }
        }
    }

}
