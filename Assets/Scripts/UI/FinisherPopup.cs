using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FinisherPopup : Popup
{
    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent;
    
    public void ShowFinisherPopup()
    {
        if (!bHasTriggered)
        {
            bHasTriggered = true;
            Invoke("DelayedShowFinisherPopup", 4.2f);
        }
    }

    private void DelayedShowFinisherPopup()
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
