public class AnimationTimeData
{

    public float currentFrame;
    public int currentClip;
    public float inputSpeed, xInput, yInput;
    public bool lockedOn;
    public bool vGuard;
    public int comboCount;
    public bool firstAttack;
    public bool secondAttack;
    public bool loopAttack;
    public bool isDead;
    public bool HeavyAttackHeld;
    public bool FinisherSetup;
    // new anim params

    public AnimationTimeData(float _currentFrame, int _currentClip, float _inputSpeed, float _xInput, float _yinput,
                                bool _lockedOn, bool _vGuard, int _comboCount, bool _firstAttack, bool _secondAttack, bool _loopAttack, bool _isDead, bool _HeavyAttackHeld, bool _FinisherSetup)
    {
        currentFrame = _currentFrame;
        currentClip = _currentClip;
        inputSpeed = _inputSpeed;
        yInput = _yinput;
        xInput = _xInput;
        lockedOn = _lockedOn;
        vGuard = _vGuard;
        comboCount = _comboCount;
        firstAttack = _firstAttack;
        secondAttack = _secondAttack;
        loopAttack = _loopAttack;
        isDead = _isDead;
        HeavyAttackHeld = _HeavyAttackHeld;
        FinisherSetup = _FinisherSetup;
    }
}
