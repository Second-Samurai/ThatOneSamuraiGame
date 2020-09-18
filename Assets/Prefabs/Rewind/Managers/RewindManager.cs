using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RewindManager : MonoBehaviour
{

    public delegate void StepBackEvent();
    public event StepBackEvent StepBack;

    public delegate void StepForwardEvent();
    public event StepForwardEvent StepForward;

    public float rewindDirection;
    public bool isTravelling = true;

    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(.05f);

    public List<RewindEntity> rewindObjects;

    public delegate void ResetEvent();
    public event ResetEvent Reset;

    public TimeThreasholdReferance timeThreashold;

    public float rewindTime;

    PostProcessingController postProcessingController;

    public float rewindResource = 0f, maxRewindResource = 10f;

    public RewindBar rewindUI;

    public Canvas gameOverCanvas;

    private GameOverMenu gameOverMenu;

    PlayerRewindEntity playerRewindEntity;


    // Start is called before the first frame update
    void Start()
    {

        postProcessingController = GameManager.instance.postProcessingController;
        rewindTime = Mathf.Round(timeThreashold.Variable.TimeThreashold * (1f / Time.fixedDeltaTime));
        //rewindResource = maxRewindResource;
        if (rewindUI == null)
            rewindUI = GameManager.instance.playerController.gameObject.GetComponentInChildren<RewindBar>();
        playerRewindEntity = GameManager.instance.playerController.gameObject.GetComponent<PlayerRewindEntity>();
        isTravelling = true;

        gameOverMenu = GameManager.instance.gameObject.GetComponentInChildren<GameOverMenu>();
    }

    private void Update()
    {
        IncreaseResource();

       
        //Debug.Log(isTravelling);
    }

    void UpdateRewindUI()
    {
        rewindUI.UpdateRewindAmount(rewindResource);
    }

    public void ResetRewind()
    {
       // rewindResource = maxRewindResource;
    }

    public void ReduceRewindAmount()
    {
        if(maxRewindResource > 0)
        {
            float f = rewindResource / maxRewindResource;
            maxRewindResource -= 2;
            rewindResource = maxRewindResource * f;
        }
       
        if (maxRewindResource <= 0) 
        {
            gameOverMenu.TextFadeIn();
            gameOverMenu.Invoke("ReturnToMenu", 10f);
            gameOverMenu.Invoke("TextFadeOut", 5f);
        }

    }
   
    public void IncreaseRewindAmount()
    {
        if (maxRewindResource < 10)
        {
            float f = rewindResource / maxRewindResource;
            maxRewindResource += 2;
            rewindResource = maxRewindResource * f;
        }
    }

    void IncreaseResource()
    {
        if (rewindResource < maxRewindResource && !isTravelling)
        {
            rewindResource += Time.deltaTime;
        }
        else if (rewindResource > maxRewindResource)
        {
            rewindResource = maxRewindResource;
        }
    }

    IEnumerator RewindCoroutine()
    {
        if (isTravelling && rewindDirection < 0 && rewindResource > 0 && playerRewindEntity.currentIndex < playerRewindEntity.playerDataList.Count-1)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepBack != null) StepBack();
            postProcessingController.WarpLensToTargetAmount(-.6f);
            rewindResource -= Time.deltaTime;
            if (rewindResource < 0) 
                rewindResource = 0;
                rewindUI.UpdateBarColor();


            // Debug.Log(Time.timeScale);
        }

        else if (isTravelling && rewindDirection > 0 && rewindResource < maxRewindResource && playerRewindEntity.currentIndex > 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepForward != null) StepForward();
            postProcessingController.WarpLensToTargetAmount(-.6f);
            rewindResource += Time.deltaTime;
            if (rewindResource > maxRewindResource) 
                rewindResource = maxRewindResource;
            rewindUI.UpdateBarColor();
        }

        if (isTravelling && rewindDirection == 0 && maxRewindResource != 0)
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            postProcessingController.WarpLensToTargetAmount(0f);
        }
        else if (maxRewindResource == 0) 
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        if (!isTravelling && Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            postProcessingController.WarpLensToTargetAmount(0f);
        }

        if(rewindUI != null)
            UpdateRewindUI();

        yield return null;

        StartCoroutine(RewindCoroutine());

    }

    public void StartRewind()
    {
        if (isTravelling)
        {
            foreach (RewindEntity entity in rewindObjects) 
            {
                entity.isTravelling = true;
            }
            StartCoroutine("RewindCoroutine");
            if (rewindUI != null)
                UpdateRewindUI();
        }

    }

    public void EndRewind() 
    {
        if (isTravelling)
        {
            isTravelling = false;
            foreach (RewindEntity entity in rewindObjects)
            {
                StopAllCoroutines();
              //  Debug.LogError("BREAK");
                entity.ApplyData();
                entity.isTravelling = false;
            }
            Reset();
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }

    
}
