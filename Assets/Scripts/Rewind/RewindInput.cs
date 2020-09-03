using Enemies;
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

    private EnemyTracker enemyTracker;

    
    // Start is called before the first frame update
    void Start()
    {
        rewindEntity = gameObject.GetComponent<RewindEntity>();
        enemyTracker = GameManager.instance.enemyTracker;
        Debug.LogWarning(enemyTracker);
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
        //checking if true
        if (isTravelling)
        {
            rewindEntity.ResetTimeline();
            for (int i = 0; i < enemyTracker.currentEnemies.Count; i++)
            {
                enemyTracker.currentEnemies[i].gameObject.GetComponent<EnemyRewindEntity>().ApplyData();
            }
        }
        isTravelling = !isTravelling;

        //if you start rewinding BUT you where not initially rewinding this shit triggers
        rewindEntity.isTravelling = isTravelling;
       // Debug.Log("rewinding");
        if (isTravelling) 
        {

            for (int i = 0; i < enemyTracker.currentEnemies.Count; i++) 
            {
                enemyTracker.currentEnemies[i].gameObject.GetComponent<AISystem>().OnEnemyRewind();
            }
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
