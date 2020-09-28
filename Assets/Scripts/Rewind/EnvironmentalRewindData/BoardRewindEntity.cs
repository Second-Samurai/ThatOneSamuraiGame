using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRewindEntity : RewindEntity
{
    [SerializeField]
    public List<BoardTimeData> BoardDataList;
    private LockOnTargetManager lockOnTargetManager;

    public BoardBreak boardBreak;
    private Rigidbody boardRigidBody;
    // Start is called before the first frame update
    protected new void Start()
    {
        boardBreak = gameObject.GetComponentInParent<BoardBreak>();
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        BoardDataList = new List<BoardTimeData>();
        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnEndRewind += ApplyData;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += EnableEvents;

        boardRigidBody = gameObject.GetComponent<Rigidbody>();

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
    public  void DisableEvents()
    {
        boardRigidBody.isKinematic = true;

    }

    public  void EnableEvents()
    {
        boardRigidBody.isKinematic = false;


    }

    public new void ResetTimeline()
    {
        for (int i = currentIndex; i > 0; i--)
        {
            BoardDataList.RemoveAt(i);
        }
        BoardDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (BoardDataList.Count > _rewindInput.rewindTime)
        {
            BoardDataList.RemoveAt(BoardDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        BoardDataList.Insert(0, new BoardTimeData(boardBreak.isBuilt, boardRigidBody.velocity));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (BoardDataList.Count > 0)
        {
            if (currentIndex < BoardDataList.Count - 1)
            {
                SetPosition();
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (BoardDataList.Count > 0)
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

        boardBreak.isBuilt = BoardDataList[currentIndex].isBuilt;
        boardRigidBody.velocity = BoardDataList[currentIndex].velocity;

        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {

        boardBreak.ReBuild();


        //base.ApplyData();
    }
}
