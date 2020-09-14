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
    PlayerInput _inputComponent;
    // Start is called before the first frame update
    void Start()
    {
        //rewindEntity = gameObject.GetComponent<RewindEntity>();
        _inputComponent = GetComponent<PlayerInput>();

        rewindManager = GameManager.instance.rewindManager;
    }

  

    void OnInitRewind() 
    {
        _inputComponent.SwitchCurrentActionMap("Rewind");
        if (!isTravelling)
        {
            isTravelling = true;
            rewindManager.isTravelling = true;
            rewindManager.StartRewind();
            rewindTut.SetActive(true);
            GameManager.instance.postProcessingController.EnableRewindColourFilter();
            // Debug.Log("rewinding");
        }

    }

    void OnEndRewind()
    {

        if (isTravelling)
        {
            _inputComponent.SwitchCurrentActionMap("Gameplay");
            GameManager.instance.postProcessingController.DisableRewindColourFilter();
            isTravelling = false;
            //rewindEntity.isTravelling = false;
            rewindManager.EndRewind();
            rewindTut.SetActive(false);
            rewindManager.isTravelling = false;
        }
    }

    public void DeathRewind()
    {
        Debug.Log("DEAD");
            OnInitRewind();

    }

    void OnScrub(InputValue value)
    {

        rewindManager.rewindDirection = value.Get<float>();
    }
}
