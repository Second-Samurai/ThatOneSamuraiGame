using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempArcherTimeEntity : AIAnimationRewindEntity
{
    public List<TempArcherTimeData> tempArcherDataList;

    private BasicArcher basicArcher;

    // Start is called before the first frame update
    protected new void Start()
    {
        basicArcher = gameObject.GetComponent<BasicArcher>();

        tempArcherDataList = new List<TempArcherTimeData>();
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (isTravelling == false)
        {
            RecordPast();

        }

        if (isTravelling)
        {

        }

    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (tempArcherDataList.Count > Mathf.Round(10f * (1f / Time.fixedDeltaTime)))
        {
            tempArcherDataList.RemoveAt(tempArcherDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        tempArcherDataList.Insert(0, new TempArcherTimeData((int)basicArcher.currentState, basicArcher.shotDirection, basicArcher.shotTimer, basicArcher.aimCounter, basicArcher.lineRenderer));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (tempArcherDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex < tempArcherDataList.Count - 1)
            {
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (tempArcherDataList.Count > 0)
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
        basicArcher.currentState = (BasicArcher.CurrentState)tempArcherDataList[currentIndex].currentState;
        basicArcher.shotDirection = tempArcherDataList[currentIndex].shotDirection;
        basicArcher.shotTimer = tempArcherDataList[currentIndex].shotTimer;
        basicArcher.aimCounter = tempArcherDataList[currentIndex].aimCounter;
        basicArcher.lineRenderer.enabled = tempArcherDataList[currentIndex].lineRenderer;
        // needs to set the enemy targeting
        base.SetPosition();
    }
}
