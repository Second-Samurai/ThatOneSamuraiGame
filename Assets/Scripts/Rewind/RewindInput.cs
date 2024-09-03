using System;
using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewindInput : MonoBehaviour
{
    private RewindManager rewindManager;
    //public delegate void StepBackEvent();
    //public event StepBackEvent StepBack;

    //public delegate void StepForwardEvent();
    //public event StepForwardEvent StepForward;

    // private bool heldBack, heldForward = false;
    public bool isTravelling = false;
    public GameObject rewindTut;
    public GameObject rewindBar;

    PlayerFunctions playerFunction;
    FinishingMoveController finishingMoveController;

    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        //rewindEntity = gameObject.GetComponent<RewindEntity>();
        rewindManager = GameManager.instance.RewindManager;
        playerFunction = gameObject.GetComponent<PlayerFunctions>();
        finishingMoveController = GetComponentInChildren<FinishingMoveController>();
    }


    private void Update()
    {
        // Note: This should be made event based instead of being tied to Unity's runtime events.
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            IInputManager _InputManager = GameManager.instance.InputManager;
            _InputManager.SwitchToMenuControls();

            GameManager.instance.postProcessingController.DisableRewindColourFilter();
            isTravelling = false;
            //rewindEntity.isTravelling = false;
            rewindTut.SetActive(false);
            rewindManager.isTravelling = false;
            rewindManager.ResetRewind();
            rewindManager.EndRewind();
        }
    }

    public void OnInitRewind()
    {
        if (!finishingMoveController.bIsFinishing)
        {
            //if (playerInput.camControl.bLockedOn) playerInput.OnLockOn();
            hidePopupEvent.Raise();
            hideLockOnPopupEvent.Raise();

            IInputManager _InputManager = GameManager.instance.InputManager;
            _InputManager.SwitchToRewindControls();
            
            if (!isTravelling && rewindManager.maxRewindResource != 0)
            {
                isTravelling = true;
                rewindManager.isTravelling = true;
                rewindManager.StartRewind();
                rewindTut.SetActive(true);
                GameManager.instance.postProcessingController.EnableRewindColourFilter();
                rewindManager.rewindUI.FadeIn(1f, 0f);
                rewindManager.transition = true;
            }
            else if (!isTravelling && rewindManager.maxRewindResource == 0)
            {
                isTravelling = true;
                rewindManager.isTravelling = true;
                rewindManager.StartRewind();
                rewindManager.isTravelling = false;
                GameManager.instance.postProcessingController.EnableRewindColourFilter();
            }
        }

    }

    public void OnEndRewind()
    {

        if (isTravelling && !playerFunction.bIsDead)
        {
            IInputManager _InputManager = GameManager.instance.InputManager;
            _InputManager.SwitchToGameplayControls();
            
            GameManager.instance.postProcessingController.DisableRewindColourFilter();
            isTravelling = false;
            //rewindEntity.isTravelling = false;
            rewindManager.EndRewind();
            rewindTut.SetActive(false);
            rewindManager.isTravelling = false;
            rewindManager.ResetRewind();
        }
    }

    public void DeathRewind()
    {
        rewindManager.ReduceRewindAmount();
        OnInitRewind();
    }
    
}
