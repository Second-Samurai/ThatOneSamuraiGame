using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public FreeLookAddOn _camScript; 
    LockOnTargetManager _lockedCamScript;
    public GameObject unlockedCam, lockedCam;
    Vector2 rotationVector;
    public Transform lockOnTarget, player, lockOnNullDummy;
    public bool bLockedOn = false;
    public EnemyTracker enemyTracker;
    PlayerInput _playerInput;

    public CinematicBars cinematicBars;

    /*private void Start()
    {
        _camScript = unlockedCam.GetComponent<FreeLookAddOn>();
        _lockedCamScript = lockedCam.GetComponent<LockOnTargetManager>();
        _playerInput = GetComponent<PlayerInput>();
    }*/

    //NOTE: this is called in player controller
    public void Init(Transform playerTarget)
    {
        GameManager gameManager = GameManager.instance;
        CinematicBars cinematicBars = gameManager.mainCamera.GetComponentInChildren<CinematicBars>();

        this.player = playerTarget;
        this.unlockedCam = gameManager.thirdPersonViewCamera;
        this.enemyTracker = gameManager.enemyTracker;
        this.cinematicBars = cinematicBars;

        _camScript = unlockedCam.GetComponent<FreeLookAddOn>();
        _lockedCamScript = lockedCam.GetComponent<LockOnTargetManager>();
        _playerInput = GetComponent<PlayerInput>();
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
        _lockedCamScript.SetTarget(target, player);
    }

    public void LockOn()
    {
        if (GetTarget())
        {
            _lockedCamScript.cam.Priority = 11;
            cinematicBars.ShowBars(200f, .3f);
        }

    }

    public void UnlockCam()
    {
        bLockedOn = false;
        _lockedCamScript.cam.Priority = 9;
        _lockedCamScript.ClearTarget();
        lockOnTarget = null;
        cinematicBars.HideBars(.3f);
    }

    public bool GetTarget()
    {
        float closest = Mathf.Infinity;
        Transform nextEnemy = null;

        if (enemyTracker == null)
        {
            enemyTracker = GameManager.instance.enemyTracker;
        }
        if (enemyTracker.currentEnemies.Count > 0)
        {
            foreach (Transform enemy in enemyTracker.currentEnemies)
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
            
            _playerInput.target = lockOnNullDummy;
            SetTarget(lockOnNullDummy);
            bLockedOn = true;
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
            else _lockedCamScript.cam.m_Lens.FieldOfView = 40f;
            yield return null;
        }
        _lockedCamScript.cam.m_Lens.Dutch = 0f;
    }

}
