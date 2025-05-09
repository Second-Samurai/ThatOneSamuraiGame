using System;

public class EventTimer
{
    
    #region - - - - - - Fields - - - - - -

    private Action m_EndingAction;
    private float m_DeltaTime;
    private float m_TimeLeft;
    private float m_TimerLength;
    private bool m_CanRestart;
    private bool m_IsActive;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public EventTimer(
        float timerLength, 
        float deltaTime, 
        Action endingAction, 
        bool startImmediately = true, 
        bool canRestart = true)
    {
        this.m_EndingAction = endingAction;
        this.m_DeltaTime = deltaTime;
        this.m_TimeLeft = timerLength;
        this.m_TimerLength = timerLength;
        this.m_CanRestart = canRestart;
        this.m_IsActive = startImmediately;
    }

    #endregion Constructors

    #region - - - - - - Properties - - - - - -

    public float TimerLength
    {
        get => this.m_TimerLength;
        set => this.m_TimerLength = value;
    }

    public bool CanRestart
    {
        get => this.m_CanRestart;
        set => this.m_CanRestart = value;
    }
        
    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    public void TickTimer()
    {
        if (!this.m_IsActive) return;
        
        this.m_TimeLeft -= this.m_DeltaTime;

        if (!this.IsTimerComplete()) return;
        this.m_EndingAction?.Invoke();
        
        if (this.m_CanRestart)
            this.ResetTimer();
    }

    public void StartTimer()
        => this.m_IsActive = true;

    public void StopTimer()
        => this.m_IsActive = false;

    public void ResetTimer() 
        => this.m_TimeLeft = this.m_TimerLength;
    
    private bool IsTimerComplete()
        => this.m_TimeLeft <= 0;
        
    #endregion Methods
    
}