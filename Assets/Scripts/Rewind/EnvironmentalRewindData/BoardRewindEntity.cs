using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardRewindEntity : RewindEntity
{
    [SerializeField]
    public List<BoardTimeData> BoardDataList;

    public BoardBreak boardBreak;
    private Rigidbody boardRigidBody;
    // Start is called before the first frame update
    protected new void Start()
    {
        boardBreak = gameObject.GetComponentInParent<BoardBreak>(); 
        BoardDataList = new List<BoardTimeData>(); 
        boardRigidBody = gameObject.GetComponent<Rigidbody>();

        base.Start();
    }

    public override void FixedUpdate()
    { 
    }

    public  void DisableEvents()
    {
        boardRigidBody.isKinematic = true;

    }

    public  void EnableEvents()
    {
        boardRigidBody.isKinematic = false; 
    }

    [Obsolete("Deprecated", false)]
    public override void StepBack()
    {
         
    }

    [Obsolete("Deprecated", false)]
    public override void StepForward()
    {
         
    }

    [Obsolete("Deprecated", false)]
    public override void ApplyData()
    { 
    }
}
