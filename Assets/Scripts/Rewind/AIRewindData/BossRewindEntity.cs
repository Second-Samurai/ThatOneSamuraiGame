using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using UnityEngine;

public class BossRewindEntity : EnemyRewindEntity
{

    public List<BossTimeData> bossTimeList;

    private AISystem aISystem;

    private EnemyTracker _enemyTracker;


    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        _enemyTracker = GameManager.instance.EnemyTracker;
        bossTimeList = new List<BossTimeData>();
        base.Start();

        _rewindInput.Reset += ResetTimeline;
        aISystem = gameObject.GetComponent<AISystem>();

        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += ApplyData;


        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();


    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

        }
        DisableCollider();

    }

    //setting rigidbodys to kinimatic

    public new void DisableEvents()
    {
        gameObjectRigidbody.isKinematic = true;
        base.DisableEvents();
    }

    public new void EnableEvents()
    {
        gameObjectRigidbody.isKinematic = false;

        base.EnableEvents();
    }


    public new void ResetTimeline()
    {
        for (int i = currentIndex; i > 0; i--)
        {
            if (currentIndex <= bossTimeList.Count - 1)
            {
                bossTimeList.RemoveAt(i);
            }
        }
        bossTimeList.TrimExcess();
    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (bossTimeList.Count > _rewindInput.rewindTime)
        {
            bossTimeList.RemoveAt(bossTimeList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        bossTimeList.Insert(0, new BossTimeData(aISystem.bossAttackSelector, aISystem.bCanBeStunned, aISystem.slamCol.enabled, 
                                aISystem.bHasBowDrawn, aISystem.shotCount, aISystem.shotTimer, aISystem.weaponSwitcher.swordHand.enabled, aISystem.weaponSwitcher.bowHand.activeSelf, 
                                aISystem.weaponSwitcher.glaiveHand.activeSelf));


        base.RecordPast();
    }

    public override void StepBack()
    {

        if (bossTimeList.Count > 0)
        {
            if (currentIndex < bossTimeList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= bossTimeList.Count - 1)
                {
                    currentIndex = bossTimeList.Count - 1;
                }
                SetPosition();
            }
        }
    }

    public override void StepForward()
    {
        if (bossTimeList.Count > 0)
        {
            if (currentIndex > 0)
            {
                SetPosition();
                currentIndex--;
            }
        }
    }

    // can be called in rewind input if needed incase jaiden yells at me for using update
    public void DisableCollider()
    {
        if (_rewindInput.isTravelling == true)
        {
            swordCollider.enabled = false;
        }

    }

    public new void SetPosition()
    {
        if (currentIndex <= bossTimeList.Count - 1)
        {
            aISystem.weaponSwitcher.EnableSword(bossTimeList[currentIndex].swordDrawn);
            aISystem.weaponSwitcher.EnableBow(bossTimeList[currentIndex].bowDrawn);
            aISystem.weaponSwitcher.EnableGlaive(bossTimeList[currentIndex].glaiveDrawn);
           
        }
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {
        if (currentIndex <= bossTimeList.Count - 1)
        {
            aISystem.bossAttackSelector = bossTimeList[currentIndex].bossAttackSelector;
            aISystem.bCanBeStunned = bossTimeList[currentIndex].bCanBeStunned;
            aISystem.slamCol.enabled = bossTimeList[currentIndex].slamCol;
            aISystem.bHasBowDrawn = bossTimeList[currentIndex].bHasBowDrawn;
            aISystem.shotCount = bossTimeList[currentIndex].shotCount;
            aISystem.shotTimer = bossTimeList[currentIndex].shotTimer;

        }
    }

    protected new void OnDestroy()
    {
        _rewindInput.Reset -= ResetTimeline;
        _rewindInput.OnEndRewind -= EnableEvents;
        _rewindInput.OnStartRewind -= DisableEvents;
        _rewindInput.OnEndRewind -= ApplyData;
        base.OnDestroy();
    }

}
