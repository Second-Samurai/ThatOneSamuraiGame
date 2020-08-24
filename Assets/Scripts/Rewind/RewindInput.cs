using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewindInput : MonoBehaviour
{
   // private bool heldBack, heldForward = false;
    private float rewindDirection;
    public bool isTravelling = false;
    private RewindEntity rewindEntity;

    // Start is called before the first frame update
    void Start()
    {
        rewindEntity = gameObject.GetComponent<RewindEntity>();
    }

    private void Update()
    {
        if (isTravelling && rewindDirection < 0) 
        {
            Time.timeScale = .005f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            rewindEntity.StepBack();
            Debug.Log(rewindDirection);

        } 
        else if (isTravelling && rewindDirection > 0)
        {
            Time.timeScale = .005f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            rewindEntity.StepForward();
                 
        }
        else if (isTravelling && rewindDirection == 0) 
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        if (!isTravelling && Time.timeScale != 1f) 
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

    }

    void OnInitRewind() 
    {
        if (isTravelling) 
        {
            rewindEntity.ResetTimeline();
        }
        isTravelling = !isTravelling;
        rewindEntity.isTravelling = isTravelling;
        //Debug.Log("rewinding");
    }

    //void OnScrubForward()
    //{
    //    heldForward = !heldForward;
    //}

    //void OnScrubBackward()
    //{
    //    heldBack = !heldBack;
    //}

    void OnScrub(InputValue value) 
    {
        rewindDirection = value.Get<float>();
    }
}
