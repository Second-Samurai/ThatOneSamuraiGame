using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRewindEntity : AIAnimationRewindEntity
{

    public List<EnemyRewindData> enemyDataList;

    private AISystem aISystem;
    public Collider swordCollider;

    public Rigidbody gameObjectRigidbody;
    private EnemyTracker _enemyTracker;


    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        _enemyTracker = GameManager.instance.enemyTracker;
        enemyDataList = new List<EnemyRewindData>();
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
            enemyDataList.RemoveAt(i);
        }
        enemyDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (enemyDataList.Count > _rewindInput.rewindTime)
        {
            enemyDataList.RemoveAt(enemyDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        enemyDataList.Insert(0, new EnemyRewindData(aISystem.EnemyState, swordCollider.enabled,
                                                    aISystem.eDamageController.enemyGuard.canGuard, aISystem.eDamageController.enemyGuard.canParry, aISystem.eDamageController.enemyGuard.isStunned,
                                                    aISystem.eDamageController.enemyGuard.statHandler.CurrentGuard, aISystem.bIsDead, aISystem.bIsUnblockable, _enemyTracker.currentEnemies));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (enemyDataList.Count > 0)
        {
            if (currentIndex < enemyDataList.Count - 1)
            {
                SetPosition();
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (enemyDataList.Count > 0)
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
        aISystem.eDamageController.enemyGuard.canGuard = enemyDataList[currentIndex].canGuard;
        
        aISystem.eDamageController.enemyGuard.isStunned = enemyDataList[currentIndex].isStunned;
        aISystem.eDamageController.enemyGuard.statHandler.CurrentGuard = enemyDataList[currentIndex].currentGuard;
        aISystem.bIsDead = enemyDataList[currentIndex].bIsDead;

        if (aISystem.bIsUnblockable != enemyDataList[currentIndex].bIsUnblockable)
        {
            aISystem.bIsUnblockable = enemyDataList[currentIndex].bIsUnblockable;
            if (aISystem.bIsUnblockable)  aISystem.BeginUnblockable();
               else aISystem.EndUnblockable();
        }

        if (aISystem.eDamageController.enemyGuard.canParry != enemyDataList[currentIndex].canParry) 
        {
            aISystem.eDamageController.enemyGuard.canParry = enemyDataList[currentIndex].canParry;
            if (aISystem.eDamageController.enemyGuard.canParry) aISystem.swordEffects.BeginBlockEffect();
               else aISystem.swordEffects.EndBlockEffect();
        }
        //Debug.LogError(enemyDataList[currentIndex].bIsDead);
        
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData() 
    {
        aISystem.SetState(enemyDataList[currentIndex].enemyState);
        swordCollider.enabled = enemyDataList[currentIndex].swordCollider;
        if (!aISystem.bIsDead)
        {
            aISystem.eDamageController.EnableDamage();
        }
        _enemyTracker.currentEnemies = enemyDataList[currentIndex].trackedCurrentEnemies.ToList<Transform>();
    }

    protected new void OnDestroy()
    {
        Debug.LogError("UNSUB");
        _rewindInput.Reset -= ResetTimeline; 
        _rewindInput.OnEndRewind -= EnableEvents;
        _rewindInput.OnStartRewind -= DisableEvents;
        _rewindInput.OnEndRewind -= ApplyData;
        base.OnDestroy();
    }

}
