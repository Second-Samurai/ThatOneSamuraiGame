using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewindInput : MonoBehaviour
{
    public delegate void StepBackEvent();
    public event StepBackEvent StepBack;

    public delegate void StepForwardEvent();
    public event StepForwardEvent StepForward;

    // private bool heldBack, heldForward = false;
    private float rewindDirection;
    public bool isTravelling = false;
    private RewindEntity rewindEntity;
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(.05f);

    // Start is called before the first frame update
    void Start()
    {
        rewindEntity = gameObject.GetComponent<RewindEntity>();
    }

    IEnumerator RewindCoroutine() 
    {
        if (isTravelling && rewindDirection < 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepBack != null) StepBack();
           // Debug.Log(Time.timeScale);
        }

        else if (isTravelling && rewindDirection > 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if(StepForward != null) StepForward();

        }

        if (isTravelling && rewindDirection == 0)
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        if (!isTravelling && Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        yield return null;

        StartCoroutine(RewindCoroutine());
        
    }

    void OnInitRewind() 
    {
        if (isTravelling)
        {
            rewindEntity.ResetTimeline();
        }
        isTravelling = !isTravelling;
        rewindEntity.isTravelling = isTravelling;
       // Debug.Log("rewinding");
        if (isTravelling) 
        {
            StartCoroutine("RewindCoroutine");    
        }
    }

    public void DeathRewind()
    {
        isTravelling = true;
        rewindEntity.isTravelling = isTravelling;
        // Debug.Log("rewinding");
        if (isTravelling)
        {
            StartCoroutine("RewindCoroutine");
        }
    }

    void OnScrub(InputValue value)
    {

        rewindDirection = value.Get<float>();
    }
}
