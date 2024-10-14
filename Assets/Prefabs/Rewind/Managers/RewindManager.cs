using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

public class RewindManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [Header("Rewind modifiers")]
    public float rewindDirection;
    public bool isTravelling = true;
    public bool isTransitioning;

    [Header("Time modifiers")]
    public TimeThreasholdReferance timeThreashold;
    public float rewindTime;
    [FormerlySerializedAs("rewindResource")] public float totalRewindQuantity = 0f; // Later remove header
    [FormerlySerializedAs("maxRewindResource")] public float maxRewindQuantity = 10f; // Later remove header
    [FormerlySerializedAs("invincabilityTimer")] public float invincibilityTimer;

    [Header("Entity tracking-list")]
    public List<RewindEntity> rewindObjects;
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(.05f);

    [Header("Menus / Graphics")]
    public RewindBar rewindUI;
    public Canvas gameOverCanvas;
    public CinemachineVirtualCamera rewindCam;
    private GameOverMenu gameOverMenu;
    
    private PDamageController damageController;
    PostProcessingController postProcessingController;
    PlayerRewindEntity playerRewindEntity;
    private RewindAudio _rewindAudio;

    #endregion Fields

    #region - - - - - - Events - - - - - -

    public delegate void StepBackEvent();
    public event StepBackEvent StepBack;

    public delegate void StepForwardEvent();
    public event StepForwardEvent StepForward;

    public delegate void StartRewindEvent();
    public event StartRewindEvent OnStartRewind;

    public delegate void EndRewindEvent();
    public event EndRewindEvent OnEndRewind;
    
    public delegate void ResetEvent();
    public event ResetEvent Reset;

    #endregion Events

    #region - - - - - - Unity Lifecycle Methods - - - - - -

    // private void Start()
    // {
    //     isTransitioning = false;
    //     postProcessingController = GameManager.instance.postProcessingController;
    //     rewindTime = Mathf.Round(timeThreashold.Variable.TimeThreashold * (1f / Time.fixedDeltaTime));
    //     //rewindResource = maxRewindResource;
    //     if (rewindUI == null)
    //         rewindUI = GameManager.instance.PlayerController.gameObject.GetComponentInChildren<RewindBar>();
    //     playerRewindEntity = GameManager.instance.PlayerController.gameObject.GetComponent<PlayerRewindEntity>();
    //     isTravelling = true;
    //
    //     gameOverMenu = GameManager.instance.gameObject.GetComponentInChildren<GameOverMenu>();
    //     _rewindAudio = gameObject.GetComponent<RewindAudio>();
    //     damageController = GameManager.instance.PlayerController.gameObject.GetComponent<PDamageController>();
    // }

    private void Update()
    {
        // Validate whether it should run
        if (this.rewindUI is null) return;
        
        IncreaseQuantityOfRewindTime();
        //UpdateRewindUI();
        if (totalRewindQuantity < maxRewindQuantity && !isTravelling && isTransitioning == false) 
        {
            rewindUI.FadeIn(1f, 1f); 
            isTransitioning = true;
        }
        else if (totalRewindQuantity == maxRewindQuantity && !isTravelling && maxRewindQuantity > 2f && isTransitioning == true ) 
        { 
            isTransitioning = false; 
            rewindUI.FadeOut(0f, 1f);
        }

        rewindUI.UpdateBarColor();
    }

    #endregion Unity Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    public void InitialiseRewindManager()
    {
        isTransitioning = false;
        postProcessingController = GameManager.instance.postProcessingController;
        rewindTime = Mathf.Round(timeThreashold.Variable.TimeThreashold * (1f / Time.fixedDeltaTime));
        //rewindResource = maxRewindResource;
        if (rewindUI == null)
            rewindUI = GameManager.instance.PlayerController.gameObject.GetComponentInChildren<RewindBar>();
        playerRewindEntity = GameManager.instance.PlayerController.gameObject.GetComponent<PlayerRewindEntity>();
        isTravelling = true;

        gameOverMenu = GameManager.instance.gameObject.GetComponentInChildren<GameOverMenu>();
        _rewindAudio = gameObject.GetComponent<RewindAudio>();
        damageController = GameManager.instance.PlayerController.gameObject.GetComponent<PDamageController>();
    }

    public void ResetRewind()
        => print("[LOG]: This method has no implementation. Please remove as part of future tech debt");

    public void ReduceRewindAmount()
    {
        if(maxRewindQuantity > 0)
        {
            float f = totalRewindQuantity / maxRewindQuantity;
            maxRewindQuantity -= 2;
            rewindUI.UpdateBarMax(maxRewindQuantity);
            
            if(rewindUI.rewindBar.fillAmount > maxRewindQuantity / 10) 
                rewindUI.UpdateRewindAmount(maxRewindQuantity);
            totalRewindQuantity = maxRewindQuantity * f;
        }
        
        if (maxRewindQuantity <= 3 && maxRewindQuantity > 0) 
            _rewindAudio.HeartBeat();

        if (maxRewindQuantity <= 0) 
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
        if (!(maxRewindQuantity < 10)) return;
        
        float f = totalRewindQuantity / maxRewindQuantity;
        maxRewindQuantity += 2;
        rewindUI.UpdateBarMax(maxRewindQuantity);
        totalRewindQuantity = maxRewindQuantity * f;
        if (maxRewindQuantity > 3) 
            _rewindAudio.StopSource();
    }

    void IncreaseQuantityOfRewindTime()
    {
        if (totalRewindQuantity < maxRewindQuantity && !isTravelling)
        {
            totalRewindQuantity += Time.deltaTime;
            //rewindUI.UpdateRewindAmount(Time.deltaTime);
            UpdateRewindUI();
        }
        else if (totalRewindQuantity > maxRewindQuantity)
            totalRewindQuantity = maxRewindQuantity;
    }

    private IEnumerator RewindCoroutine()
    {
        if (isTravelling && rewindDirection < 0 && totalRewindQuantity > 0 && playerRewindEntity.currentIndex < playerRewindEntity.playerDataList.Count-1)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepBack != null) StepBack();
            postProcessingController.WarpLensToTargetAmount(-.6f);
            totalRewindQuantity -= Time.deltaTime;
            // rewindUI.UpdateRewindAmount(-Time.deltaTime);
            UpdateRewindUI();

            if (totalRewindQuantity < 0)  
                totalRewindQuantity = 0;
            
            rewindUI.UpdateBarColor();
        }
        else if (isTravelling && rewindDirection > 0 && totalRewindQuantity < maxRewindQuantity && playerRewindEntity.currentIndex > 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if (StepForward != null) StepForward();
            postProcessingController.WarpLensToTargetAmount(-.6f);
            totalRewindQuantity += Time.deltaTime;
            //rewindUI.UpdateRewindAmount(Time.deltaTime);
            UpdateRewindUI();
            if (totalRewindQuantity > maxRewindQuantity) 
                totalRewindQuantity = maxRewindQuantity;
            rewindUI.UpdateBarColor();
        }

        if (isTravelling && rewindDirection == 0 && maxRewindQuantity != 0)
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            postProcessingController.WarpLensToTargetAmount(0f);
        }
        else if (maxRewindQuantity == 0) 
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
        yield return new WaitForSeconds(invincibilityTimer);
        damageController.EnableDamage();
    }

    public void StartRewind()
    {
        if (rewindCam) rewindCam.m_Priority = 20; 
        
        _rewindAudio.Freeze();
        _rewindAudio.Idle();
        _rewindAudio.audioManager.backgroundAudio.PauseMusic();
        _rewindAudio.audioManager.trackManager.PauseAll();

        if (!isTravelling) return;
        
        OnStartRewind();
        foreach (RewindEntity entity in rewindObjects) 
            entity.isTravelling = true;
        StartCoroutine("RewindCoroutine");

    }

    public void EndRewind() 
    {
        _rewindAudio.StopSource();
        
        if(rewindCam) rewindCam.m_Priority = 3;
        
        if (isTravelling)
        {
            isTravelling = false;
            foreach (RewindEntity entity in rewindObjects)
            {
                StopAllCoroutines();
                entity.isTravelling = false;
            }
            
            _rewindAudio.Resume();
            _rewindAudio.audioManager.backgroundAudio.ResumeMusic();
            _rewindAudio.audioManager.trackManager.PauseAll();
            
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            
            Reset();
            OnEndRewind();
        }
    }

    void UpdateRewindUI() 
        => rewindUI.UpdateRewindAmount(totalRewindQuantity);

    #endregion Methods

}
