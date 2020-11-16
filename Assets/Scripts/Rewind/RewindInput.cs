using System;
using System.Collections;
using System.Collections.Generic;
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

    PlayerInput _inputComponent;

    PlayerFunctions playerFunction;
    PlayerInputScript playerInput;
    FinishingMoveController finishingMoveController;

    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        //rewindEntity = gameObject.GetComponent<RewindEntity>();
        _inputComponent = GetComponent<PlayerInput>();

        rewindManager = GameManager.instance.rewindManager;
        playerFunction = gameObject.GetComponent<PlayerFunctions>();
        playerInput = GetComponent<PlayerInputScript>();
        finishingMoveController = GetComponentInChildren<FinishingMoveController>();

    }


    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            _inputComponent.SwitchCurrentActionMap("Gameplay");
            GameManager.instance.postProcessingController.DisableRewindColourFilter();
            isTravelling = false;
            //rewindEntity.isTravelling = false;
            rewindTut.SetActive(false);
            rewindManager.isTravelling = false;
            rewindManager.ResetRewind();
            rewindManager.EndRewind();
        }
    }

    void OnInitRewind()
    {
        if (!finishingMoveController.bIsFinishing)
        {
            //if (playerInput.camControl.bLockedOn) playerInput.OnLockOn();
            hidePopupEvent.Raise();
            hideLockOnPopupEvent.Raise();
            _inputComponent.SwitchCurrentActionMap("Rewind");
            if (!isTravelling && rewindManager.maxRewindResource != 0)
            {
                isTravelling = true;
                rewindManager.isTravelling = true;
                rewindManager.StartRewind();
                rewindTut.SetActive(true);
                GameManager.instance.postProcessingController.EnableRewindColourFilter();
                rewindManager.rewindUI.FadeIn(1f, 0f);
                rewindManager.transition = true;
                // Debug.Log("rewinding");
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

    void OnEndRewind()
    {

        if (isTravelling && !playerFunction.bIsDead)
        {
            _inputComponent.SwitchCurrentActionMap("Gameplay");
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
        //Debug.Log("DEAD");
        rewindManager.ReduceRewindAmount();
        OnInitRewind();

    }

    void OnScrub(InputValue value)
    {

        rewindManager.rewindDirection = value.Get<float>();
    }
}
