using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    FreeLookAddOn _camScript; 
    LockOnTargetManager _lockedCamScript;
    public GameObject unlockedCam, lockedCam;
    Vector2 rotationVector;
    public Transform lockOnTarget, player;
    public bool bLockedOn = false;
    public EnemyTracker enemyTracker;
    PlayerInput _playerInput;

    private void Start()
    {
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
            _lockedCamScript.cam.Priority = 11;
    }

    public void UnlockCam()
    {
        bLockedOn = false;
        _lockedCamScript.cam.Priority = 9;
        _lockedCamScript.ClearTarget();
        lockOnTarget = null;
    }

    public bool GetTarget()
    {
        float closest = Mathf.Infinity;
        Transform nextEnemy = null;


        foreach (Transform enemy in enemyTracker.currentEnemies)
        {
            float distance = Vector3.Distance(player.position, enemy.position);
            if (distance < closest && enemy != lockOnTarget)
            { 
                closest = distance;
                nextEnemy = enemy; 
            }
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

}
