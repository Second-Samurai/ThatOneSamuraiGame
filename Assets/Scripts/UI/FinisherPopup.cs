using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FinisherPopup : Popup
{
    private bool bHasTriggeredHeavy;
    
    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent;
    
    public void ShowFinisherPopup()
    {
        if (!bHasTriggered)
        {
            bHasTriggered = true;
            Invoke("ShowPopup", 4.2f);
        }
    }
    
    public void ShowHeavyPopup()
    {
        if (!bHasTriggeredHeavy)
        {
            bHasTriggeredHeavy = true;
            ShowPopup();
        }
    }


    private void ShowPopup()
    {
        hidePopupEvent.Raise();
        hideLockOnPopupEvent.Raise();
            
        popup.SetActive(true);
            
        for (int index = 0; index < popupImageTweeners.Length; index++)
        {
            popupImageTweeners[index] = popupImageArray[index].DOFade(1, 1.5f);
        }
        for (int index = 0; index < popupTextTweeners.Length; index++)
        {
            popupTextTweeners[index] = popupTextArray[index].DOFade(1, 1.5f);
        }
            
        StartDespawnTimer();
    }
}
