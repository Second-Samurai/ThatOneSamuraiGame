public class BossTimeData
{
    public int bossAttackSelector;
    public bool bCanBeStunned;
    public bool slamCol;
    public bool bHasBowDrawn;
    public int shotCount;
    public float shotTimer;

    // weapon switcher
    public bool swordDrawn;
    public bool bowDrawn;
    public bool glaiveDrawn;



    public BossTimeData(int _bossAttackSelector, bool _bCanBeStunned, bool _slamCol, bool _bHasBowDrawn, int _shotCount, float _shotTimer, bool _swordDrawn, bool _bowDrawn, bool _glaiveDrawn)
    {
        bossAttackSelector = _bossAttackSelector;
        bCanBeStunned = _bCanBeStunned;
        slamCol = _slamCol;
        bHasBowDrawn = _bHasBowDrawn;
        shotCount = _shotCount;
        shotTimer = _shotTimer;

        // weapon switcher
        swordDrawn = _swordDrawn;
        bowDrawn = _bowDrawn;
        glaiveDrawn = _glaiveDrawn;

    }
}
