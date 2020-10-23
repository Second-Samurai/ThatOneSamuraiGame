using DG.Tweening;
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

    public delegate void StartRewindEvent();
    public event StartRewindEvent OnStartRewind;

    public delegate void EndRewindEvent();
    public event EndRewindEvent OnEndRewind;



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

    private RewindAudio _rewindAudio;

    private PDamageController damageController;
    public float invincabilityTimer;
    public bool transition;


    // Start is called before the first frame update
    void Start()
    {
        transition = false;
        postProcessingController = GameManager.instance.postProcessingController;
        rewindTime = Mathf.Round(timeThreashold.Variable.TimeThreashold * (1f / Time.fixedDeltaTime));
        //rewindResource = maxRewindResource;
        if (rewindUI == null)
        {
            rewindUI = GameManager.instance.playerController.gameObject.GetComponentInChildren<RewindBar>();
        }
        playerRewindEntity = GameManager.instance.playerController.gameObject.GetComponent<PlayerRewindEntity>();
        isTravelling = true;

        gameOverMenu = GameManager.instance.gameObject.GetComponentInChildren<GameOverMenu>();

        _rewindAudio = gameObject.GetComponent<RewindAudio>();

        damageController = GameManager.instance.playerController.gameObject.GetComponent<PDamageController>();

        
    }

    private void Update()
    {
        IncreaseResource();
        //UpdateRewindUI();
        if (rewindResource < maxRewindResource && !isTravelling && transition == false) 
        {
            rewindUI.FadeIn(1f, 1f); 
            transition = true;
        }
        else if (rewindResource == maxRewindResource && !isTravelling && maxRewindResource > 2f && transition == true ) 
        { 
            transition = false; 
            rewindUI.FadeOut(0f, 1f);
        }

        rewindUI.UpdateBarColor();
    //Debug.Log(isTravelling);
    }

    void UpdateRewindUI()
    {
        //Debug.LogWarning(rewindResource);
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
            rewindUI.UpdateBarMax(maxRewindResource);
            if(rewindUI.rewindBar.fillAmount > maxRewindResource / 10) rewindUI.UpdateRewindAmount(maxRewindResource);
            rewindResource = maxRewindResource * f;
        }
        if (maxRewindResource <= 3 && maxRewindResource > 0) 
        {
            _rewindAudio.HeartBeat();
        }

        if (maxRewindResource <= 0) 
        {
            _rewindAudio.StopSource();
            _rewindAudio.DeathSFX();
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
            rewindUI.UpdateBarMax(maxRewindResource);
            rewindResource = maxRewindResource * f;
            if (maxRewindResource > 3)
            {
                _rewindAudio.StopSource();
            }
        }
    }

    void IncreaseResource()
    {
        if (rewindResource < maxRewindResource && !isTravelling)
        {
            rewindResource += Time.deltaTime;
            //rewindUI.UpdateRewindAmount(Time.deltaTime);
            UpdateRewindUI();
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
            // rewindUI.UpdateRewindAmount(-Time.deltaTime);
            UpdateRewindUI();

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
            //rewindUI.UpdateRewindAmount(Time.deltaTime);
            UpdateRewindUI();
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

        //if (rewindUI != null)
        //    UpdateRewindUI();

        yield return null;

        StartCoroutine(RewindCoroutine());

    }

    public IEnumerator BecomeInvincible()
    {
        damageController.DisableDamage();
        //Debug.Log("become invinvible");
        yield return new WaitForSeconds(invincabilityTimer);

        damageController.EnableDamage();
        //Debug.Log("disable invinvible");

    }

    public void StartRewind()
    {
        _rewindAudio.Freeze();
        _rewindAudio.Idle();
        _rewindAudio.audioManager.backgroundAudio.PauseMusic();
        if (isTravelling)
        {
            
            OnStartRewind();
            foreach (RewindEntity entity in rewindObjects) 
            {
                entity.isTravelling = true;
            }
            StartCoroutine("RewindCoroutine");
            //if (rewindUI != null)
            //    UpdateRewindUI();
        }

    }

    public void EndRewind() 
    {
        _rewindAudio.StopSource();

        if (isTravelling)
        {
            OnEndRewind();
            isTravelling = false;
            foreach (RewindEntity entity in rewindObjects)
            {
                StopAllCoroutines();
              //  Debug.LogError("BREAK");
                //entity.ApplyData();
                entity.isTravelling = false;
            }
            _rewindAudio.Resume();
            _rewindAudio.audioManager.backgroundAudio.ResumeMusic();
            Reset();
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }


}
