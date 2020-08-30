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
        enemyDataList = new List<EnemyRewindData>();
        base.Start();

        aISystem = gameObject.GetComponent<AISystem>();
    }

    public override void FixedUpdate()
    {
        if (isTravelling == false)
        {
            RecordPast();

        }

    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (enemyDataList.Count > Mathf.Round(10f * (1f / Time.fixedDeltaTime)))
        {
            enemyDataList.RemoveAt(enemyDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        enemyDataList.Insert(0, new EnemyRewindData(aISystem.EnemyState));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (enemyDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex < enemyDataList.Count - 1)
            {
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (enemyDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex > 0)
            {
                currentIndex--;
            }
        }
    }

    public new void SetPosition()
    {
        aISystem.SetState(enemyDataList[currentIndex].enemyState);
        // needs to set the enemy targeting
        base.SetPosition();
    }
}
