using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popup;
    private bool hasTriggered = false;

    public Image[] popupImageArray;
    public TMP_Text[] popupTextArray;
    private Tweener[] _popupImageTweeners;
    private Tweener[] _popupTextTweeners;
    
    public float maxTimeRemaining;
    private bool _bTimerRunning = false;
    private float _timeRemaining;
    
    //Only used for the lock on tutorial
    private bool _bStopLockOnTut = false;

    private void Start()
    {
        _popupImageTweeners = new Tweener[popupImageArray.Length];
        _popupTextTweeners = new Tweener[popupTextArray.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            popup.SetActive(true);
            
            for (int index = 0; index < _popupImageTweeners.Length; index++)
            {
                _popupImageTweeners[index] = popupImageArray[index].DOFade(1, 1.5f);
            }
            for (int index = 0; index < _popupTextTweeners.Length; index++)
            {
                _popupTextTweeners[index] = popupTextArray[index].DOFade(1, 1.5f);
            }
            
            StartDespawnTimer();
        }
    }

    private void StartDespawnTimer()
    {
        _timeRemaining = maxTimeRemaining;
        _bTimerRunning = true;
    }

    private void Update()
    {
        if (_bTimerRunning)
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0)
            {
                _bTimerRunning = false;
                HidePopup();
            }
        }
    }

    public void HidePopup()
    {
        for (int index = 0; index < _popupImageTweeners.Length; index++)
        {
            _popupImageTweeners[index].Complete();
            popupImageArray[index].DOFade(0, 0.5f);
        }
        for (int index = 0; index < _popupTextTweeners.Length; index++)
        {
            _popupTextTweeners[index].Complete();
            popupTextArray[index].DOFade(0, 0.5f);
        }
        _bTimerRunning = false;
    }

    public void LockOnPopup(GameObject _popup)
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

    public void StopLockOnTut()
    {
        
        _bStopLockOnTut = true;
    }
}
