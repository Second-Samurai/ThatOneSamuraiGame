using System;
using System.Collections.Generic;
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
        ArmourDataList = new List<ArmourTimeData>(); 

        armourPiece = gameObject.GetComponent<ArmourPiece>();
        ArmourRigidBody = gameObject.GetComponent<Rigidbody>();

        base.Start();
    }

    public override void FixedUpdate()
    { 
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

    protected new void OnDestroy()
    { 
        base.OnDestroy();
    }
}
