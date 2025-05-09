using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popup;
    public bool bHasTriggered = false;

    public Image[] popupImageArray;
    public TMP_Text[] popupTextArray;
    protected Tweener[] popupImageTweeners;
    protected Tweener[] popupTextTweeners;
    
    public float maxTimeRemaining;
    private bool _bTimerRunning = false;
    private float _timeRemaining;
    
    public GameEvent hidePopupEvent;

    private void Start()
    {
        popupImageTweeners = new Tweener[popupImageArray.Length];
        popupTextTweeners = new Tweener[popupTextArray.Length];
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bHasTriggered)
        {
            hidePopupEvent.Raise();
            
            bHasTriggered = true;
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

    protected void StartDespawnTimer()
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
                HidePopup(0.5f);
            }
        }
    }

    public void HidePopup(float duration)
    {
        for (int index = 0; index < popupImageTweeners.Length; index++)
        {
            popupImageTweeners[index].Complete();
            popupImageArray[index].DOFade(0, duration);
        }
        for (int index = 0; index < popupTextTweeners.Length; index++)
        {
            popupTextTweeners[index].Complete();
            popupTextArray[index].DOFade(0, duration);
        }
        _bTimerRunning = false;
    }
}
