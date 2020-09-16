using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRewindEntity : AIAnimationRewindEntity
{

    public List<EnemyRewindData> enemyDataList;

    private AISystem aISystem;

    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        enemyDataList = new List<EnemyRewindData>();
        base.Start();

        _rewindInput.Reset += ResetTimeline;
        aISystem = gameObject.GetComponent<AISystem>();
    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

        }

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
        enemyDataList.Insert(0, new EnemyRewindData(aISystem.EnemyState, aISystem.statHandler));

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

    public new void SetPosition()
    {
        aISystem.statHandler = enemyDataList[currentIndex].statHandler;
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData() 
    {
        aISystem.SetState(enemyDataList[currentIndex].enemyState);
    }
}
