using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ArmourRewindEntity : RewindEntity
{
    [SerializeField]
    public List<ArmourTimeData> ArmourDataList;

    private ArmourPiece armourPiece;
    private Rigidbody ArmourRigidBody;
    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        ArmourDataList = new List<ArmourTimeData>();
        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnEndRewind += ApplyData;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += EnableEvents;

        armourPiece = gameObject.GetComponent<ArmourPiece>();
        ArmourRigidBody = gameObject.GetComponent<Rigidbody>();

        base.Start();
    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

        }

        if (isTravelling)
        {

        }
        
    }
    public void DisableEvents()
    {
        ArmourRigidBody.isKinematic = true;

    }

    public void EnableEvents()
    {


    }

    public new void ResetTimeline()
    {
        for (int i = currentIndex; i > 0; i--)
        {
            if (currentIndex <= ArmourDataList.Count - 1)
            {
                ArmourDataList.RemoveAt(i);
            }
        }
        ArmourDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (ArmourDataList.Count > _rewindInput.rewindTime)
        {
            ArmourDataList.RemoveAt(ArmourDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        ArmourDataList.Insert(0, new ArmourTimeData(armourPiece.col.enabled, armourPiece.destroyed, ArmourRigidBody.velocity, transform.parent));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (ArmourDataList.Count > 0)
        {
            if (currentIndex < ArmourDataList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= ArmourDataList.Count - 1)
                {
                    currentIndex = ArmourDataList.Count - 1;
                }
                SetPosition();
            }
        }
    }

    public override void StepForward()
    {
        if (ArmourDataList.Count > 0)
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
        if (currentIndex <= ArmourDataList.Count - 1)
        {
        }
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {
        if (currentIndex <= ArmourDataList.Count - 1)
        {
            transform.parent = ArmourDataList[currentIndex].parent;
            armourPiece.destroyed = ArmourDataList[currentIndex].destroyed;
            armourPiece.rb.velocity = ArmourDataList[currentIndex].velocity;
            armourPiece.col.enabled = ArmourDataList[currentIndex].isEnabled;
            armourPiece.rb.isKinematic = !ArmourDataList[currentIndex].isEnabled;
        }

        //base.ApplyData();
    }
}
