using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

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
        // Don't show lock on if bool is true
        if (_bStopLockOnTut || _popup.activeSelf)
        {
            _popup.SetActive(false);
        }
        else if (!_bStopLockOnTut)
        {
            _popup.SetActive(true);
        }
    }

    public void StartLockOnTut()
    {
        _bStopLockOnTut = false;
    }
    public void StopLockOnTut()
    {
        _bStopLockOnTut = true;
    }
}
