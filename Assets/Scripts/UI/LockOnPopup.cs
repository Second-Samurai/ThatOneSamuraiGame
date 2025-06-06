﻿using UnityEngine;

public class LockOnPopup : Popup
{
    private bool _bStopLockOnTut = true;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bHasTriggered)
        {
            _bStopLockOnTut = false;
        }
        base.OnTriggerEnter(other);
    }

    public void ToggleLockOnPopup(GameObject _popup)
    {
        if (_bStopLockOnTut)
        {
            _popup.SetActive(false);
            return;
        }
        
        // TODO: Change checks for the lock on check
        // Ticket: #53 - Decouple the UI from the Player's camera control fields.
        // if (GameManager.instance.CameraControl.bLockedOn)
        // {
        //     _popup.SetActive(true);
        // }
        // else
        // {
            _popup.SetActive(false);
        // }
    }

    public void StartLockOnTut()
    {
        _bStopLockOnTut = false;
    }
    
    public void StopLockOnTut(GameObject lockOnGameObject)
    {
        _bStopLockOnTut = true;
        ToggleLockOnPopup(lockOnGameObject);
    }
}
